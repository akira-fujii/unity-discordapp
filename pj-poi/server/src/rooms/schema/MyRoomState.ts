import { MapSchema, Schema, ArraySchema, Context, type } from "@colyseus/schema";
// room sync
export class Player extends Schema {
    @type("number") x: number = 0;
    @type("number") y: number = 0;
    @type("string") name: string = "";
    @type("string") avatar: string = "";
    @type("boolean") isReady: boolean = false;
    @type("number") hp: number = 0;
    @type("number") point: number = 0;
    @type("string") currentTargetId: string = "";
    @type(["string"]) hand = new ArraySchema<string>();
    @type(["string"]) currentCardIds = new ArraySchema<string>();
    @type("number") currentTotalPower: number = 0;
    @type("number") currentTotalDefense: number = 0;
    @type("uint8") currentElement: number = 0;
}

export class Card extends Schema {

    @type("uint8") state: number = 0;
    @type("number") x: number = 0;
    @type("number") y: number = 0;
    @type("number") z: number = 0;
    @type("string") cardId: string = "";
    @type("string") owner: string = "";
    
}

export class GamePhase extends Schema {
    @type("uint8") current: number = 0;
    @type("uint8") readonly start: number = 0;
    @type("uint8") readonly playing: number = 1;
    @type("uint8") readonly solve: number = 2;
    @type("uint8") readonly end: number = 3;
}

export class CardElement extends Schema {
    @type("uint8") readonly None: number = 0;
    @type("uint8") readonly Physical : number = 1;
    @type("uint8") readonly Mental: number = 2;
    @type("uint8") readonly Technique: number = 3;
}

export class MyRoomState extends Schema {
    @type("string") readonly ready: string = "ready";
    @type("string") readonly setPlayerName: string = "setName";
    @type("string") readonly cardPos: string = "cardPos";
    @type("string") readonly playCard: string = "play";
    @type("string") readonly cardResult: string = "cardResult";
    @type("string") readonly onChangeCardPos: string = "onChangeCardPos";
    @type("string") readonly requestDrawRandomCard: string = "draw";
    @type("string") readonly requestDrawCardId: string = "drawId";
    @type("string") readonly solved: string = "solved";
    @type("string") readonly heal : string = "heal";
    @type("string") readonly damage = "damage";
    @type("string") readonly win = "win";
    @type("string") readonly surrender = "surrender";
    @type("string") readonly setCsv = "setCsv";
    
    @type("uint8") readonly playerMove: number = 0;
    
	@type({map: Player})
	players = new MapSchema<Player>();
    
    @type([Card])
    cards = new ArraySchema<Card>()
    
    @type(GamePhase)
    gamePhase: GamePhase = new GamePhase();
    
    @type("uint8") turn: number = 0;
    @type("string") winnerId: string = "";
}
