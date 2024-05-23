import config from "@colyseus/tools";

import { WebSocketTransport } from "@colyseus/ws-transport";
import { monitor } from "@colyseus/monitor";
import { playground } from "@colyseus/playground";

// import { RedisDriver } from "@colyseus/redis-driver";
// import { RedisPresence } from "@colyseus/redis-presence";

/**
 * Import your Room files
 */
import { MyRoom } from "./rooms/MyRoom";
import auth from "./config/auth";
export default config({
    options: {
        // devMode: true,
        // driver: new RedisDriver(),
        // presence: new RedisPresence(),
    },

    initializeTransport: (options) => new WebSocketTransport(options),

    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
        gameServer.define('card', MyRoom);
    },

    initializeExpress: (app) => {
        /**
         * Bind your custom express routes here:
         */
        app.get("/", (req, res) => {
            res.send(`Instance ID => ${process.env.NODE_APP_INSTANCE ?? "NONE"}`);
        });

        /**
         * Bind @colyseus/monitor
         * It is recommended to protect this route with a password.
         * Read more: https://docs.colyseus.io/tools/monitor/
         */
        app.use("/colyseus", monitor());

        // Bind "playground"
        app.use("/playground", playground);

        // Bind auth routes
        app.use(auth.prefix, auth.routes());

        //
        // Discord Embedded SDK: Retrieve user token when under Discord/Embed
        //
        app.post('/api/discord_token', async (req, res) => {
            try {
                // retrieve access token from Discord API
                const response = await fetch(`https://discord.com/api/oauth2/token`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: new URLSearchParams({
                        client_id: process.env.DISCORD_CLIENT_ID,
                        client_secret: process.env.DISCORD_CLIENT_SECRET,
                        grant_type: 'authorization_code',
                        code: req.body.code,
                    }),
                });
                const { access_token } = await response.json();

                // retrieve user data from Discord API
                const profile = await (await fetch(`https://discord.com/api/users/@me`, {
                    method: "GET",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'Authorization': `Bearer ${access_token}`,
                    }
                })).json();
                
                // upsert user into the database
                console.log(profile);

                res.send({
                    access_token, // Discord Access Token
                    profile // User data
                });

            } catch (e: any) {
                res.status(400).send({ error: e.message });
            }
        });
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});
