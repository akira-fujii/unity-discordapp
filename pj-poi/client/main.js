import { DiscordSDK } from "@discord/embedded-app-sdk";
import {patchUrlMappings} from '@discord/embedded-app-sdk';
import rocketLogo from '/rocket.png';
import "./style.css";

// Will eventually store the authenticated user's access_token
let auth;
// discord_id: profile.id,
// name: profile.global_name || profile.username || profile.login,
// locale: profile.locale || "",
// email: profile.email,
let discord_profile;

const discordSdk = new DiscordSDK(import.meta.env.VITE_DISCORD_CLIENT_ID);

setupDiscordSdk().then(() => {
    console.log("Discord SDK is authenticated");

    // appendVoiceChannelName();
    // appendGuildAvatar();
});
async function setupDiscordSdk() {
    await discordSdk.ready();
    console.log("Discord SDK is ready");
    // Authorize with Discord Client
    const { code } = await discordSdk.commands.authorize({
        client_id: import.meta.env.VITE_DISCORD_CLIENT_ID,
        response_type: "code",
        state: "",
        prompt: "none",
        scope: [
            "identify",
            "guilds",
        ],
    });

    // Retrieve an access_token from your activity's server
    const response = await fetch("/api/discord_token", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            code,
        }),
    });
    const { access_token, profile } = await response.json();
    
    // Authenticate with Discord client (using the access_token)
    auth = await discordSdk.commands.authenticate({
        access_token,
    });
    console.log("get discord_profile", profile);
    discord_profile = profile;

    if (auth == null) {
        throw new Error("Authenticate command failed");
    }else{
        console.log(`Discord SDK is authenticated\nuser_name=${profile.global_name}, id=${profile.id}`);
    }
}

// document.querySelector('#app').innerHTML = `
//   <div>
//     <img src="${rocketLogo}" class="logo" alt="Discord" />
//     <h1>Hello, World!</h1>
//   </div>
// `;

async function appendVoiceChannelName() {
    const app = document.querySelector('#app');

    let activityChannelName = 'Unknown';

    // Requesting the channel in GDMs (when the guild ID is null) requires
    // the dm_channels.read scope which requires Discord approval.
    if (discordSdk.channelId != null && discordSdk.guildId != null) {
        // Over RPC collect info about the channel
        const channel = await discordSdk.commands.getChannel({channel_id: discordSdk.channelId});
        if (channel.name != null) {
            activityChannelName = channel.name;
        }
    }

    // Update the UI with the name of the current voice channel
    const textTagString = `Init Activity Channel: "${activityChannelName}"`;
    const textTag = document.createElement('p');
    console.log(textTagString);
    textTag.innerHTML = textTagString;
    app.appendChild(textTag);
}

async function appendGuildAvatar() {
    const app = document.querySelector('#app');

    // 1. From the HTTP API fetch a list of all of the user's guilds
    const guilds = await fetch(`https://discord.com/api/v10/users/@me/guilds`, {
        headers: {
            // NOTE: we're using the access_token provided by the "authenticate" command
            Authorization: `Bearer ${auth.access_token}`,
            'Content-Type': 'application/json',
        },
    }).then((response) => response.json());

    // 2. Find the current guild's info, including it's "icon"
    const currentGuild = guilds.find((g) => g.id === discordSdk.guildId);

    // 3. Append to the UI an img tag with the related information
    if (currentGuild != null) {
        const guildImg = document.createElement('img');
        guildImg.setAttribute(
            'src',
            // More info on image formatting here: https://discord.com/developers/docs/reference#image-formatting
            `https://cdn.discordapp.com/icons/${currentGuild.id}/${currentGuild.icon}.webp?size=128`
        );
        guildImg.setAttribute('width', '128px');
        guildImg.setAttribute('height', '128px');
        guildImg.setAttribute('style', 'border-radius: 50%;');
        app.appendChild(guildImg);
    }
}

// call from Unity
async function GetChannel(parameter){
    console.log(parameter);

    let activityChannelName = 'Unknown';

    // Requesting the channel in GDMs (when the guild ID is null) requires
    // the dm_channels.read scope which requires Discord approval.
    if (discordSdk.channelId != null && discordSdk.guildId != null) {
        // Over RPC collect info about the channel
        console.log("checking channel name");
        const channel = await discordSdk.commands.getChannel({channel_id: discordSdk.channelId});
        const textTagString = `Get Channel: "${discordSdk.channelId}"`;
        if (channel.name != null) {
            activityChannelName = channel.name;
        }
    }
    // Update the UI with the name of the current voice channel
    const textTagString = `Activity Channel: "${activityChannelName}"`;
    console.log(textTagString);
    
    window.unityInstance.SendMessage("DiscordSDKManager", "GetChannelCallback", activityChannelName);
    // window.unityInstance.SendMessage("ColyseusManager", "GetServerUrlCallback", "1219662530054590555.discordsays.com/cute");
}

// called from Unity
async function GetUserInfo(){
    while (auth === undefined || auth === null) {
        console.log("waiting for auth");
        await new Promise(resolve => setTimeout(resolve, 100));
    }
    let userName = auth.user.username;
    let nickName = "None";
    console.log("GetUserInfo string\n" + userName);
    if (discordSdk.channelId != null && discordSdk.guildId != null) {
        // Over RPC collect info about the channel
        const channel = await discordSdk.commands.getChannel({channel_id: discordSdk.channelId});
        channel.voice_states.forEach((voiceState) => {
            if (voiceState.user == null) {
                return;
            }
            if (voiceState.user.id === auth.user.id) {
                console.log("voiceState\n" + voiceState.nick);
                nickName = voiceState.nick;
            }
        });
    }
    console.log("GetUserInfo string\n" + userName + ":" + nickName);
    window.unityInstance.SendMessage("DiscordSDKManager", "GetUserInfoCallback", JSON.stringify({"id": auth.user.id, "username": nickName, "avatar": auth.user.avatar}));
}
async function GetServerUrl(parameter){
    console.log(parameter);
    console.log(parameter);

    let activityChannelId = 'Unknown';
    let activityGuildId = 'Unknown';
    let activityChannelName = 'Unknown';
    if (discordSdk.channelId != null && discordSdk.guildId != null) {
        activityChannelId = discordSdk.channelId;
        activityGuildId = discordSdk.guildId;
        const channel = await discordSdk.commands.getChannel({channel_id: discordSdk.channelId});
        if (channel.name != null) {
            activityChannelName = channel.name;
        }
    }
    const gameInfo = {"channelName": activityChannelName, "channelId": activityChannelId, "guildId": activityGuildId};
    window.unityInstance.SendMessage("DiscordSDKManager", "GetServerUrlCallback", `${import.meta.env.VITE_DISCORD_CLIENT_ID}.discordsays.com`);
    window.unityInstance.SendMessage("DiscordSDKManager", "GetGameInfoCallback", JSON.stringify(gameInfo));
}

async function initiateImageUpload(){
    await discordSdk.commands.initiateImageUpload();
}

async function openInviteDialog(){
    await discordSdk.commands.openInviteDialog();   
}

function recieveMessage(event) {    
    var data = JSON.parse(event.detail)
    var methodName = data.methodName
    var parameter = data.parameter
    try {
        parameter = JSON.parse(parameter)
    } catch (e) {
        parameter = null
    }
    // C#から指定されているメソッドを呼び出しparameterを渡す
    eval(`${methodName}(parameter)`)
}

// unityMessageというCustomEventを受け取る
window.addEventListener('unityMessage', recieveMessage, false)