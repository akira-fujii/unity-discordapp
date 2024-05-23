import { Room, Client } from "colyseus";
import {Card, MyRoomState, Player} from "./schema/MyRoomState";
import { v4 as uuidv4 } from 'uuid';
import { GameModel } from "./GameModel";
export type PositionMessage = {
  id: string,
  x: number,
  y: number,
  z: number,
}

export type ReadyMessage = {
    isReady: boolean,
}

export type PlayCardMessage = {
    cardId: string,
    targetId: string,
}

// let isSinglePlayer = true;
let isSinglePlayer = true;

export class MyRoom extends Room<MyRoomState> {
  gameModel: GameModel;

  isPlaying (){
    return this.state.gamePhase.current === this.state.gamePhase.playing || this.state.gamePhase.current === this.state.gamePhase.solve;
  }
  onCreate (options: {id : string}) {
    this.roomId = options.id;
    this.setPrivate(true);
    this.setState(new MyRoomState());
    // set game state
    this.state.gamePhase.current = this.state.gamePhase.start;
    console.log(`create room ${this.state.ready} id=${this.roomId}`);
    // create game model
    this.gameModel = new GameModel();
    // this.gameModel.initialize();
  }
  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
    // 試合中は観戦のみ、player作成しない
    if (this.isPlaying()){
      console.log("game is already started");
      return;
    }
    
    this.state.players.set(client.sessionId, this.gameModel.createPlayer());

    // Send welcome message to the client.
    client.send("welcomeMessage", "Welcome to Colyseus!");
    
    // ready message
    this.onMessage(this.state.ready, async (client, message: ReadyMessage) => {
      const isReady = message.isReady;
      console.log(`set ready ${isReady} type=${typeof isReady}`);
      // check game phase
      if (this.state.gamePhase.current !== this.state.gamePhase.start){
        console.log("game is already started");
        return;
      }
      const player = this.state.players.get(client.sessionId);
      player.isReady = isReady;
      
      // check all players are ready
      if (this.state.players.size < 2){
        console.log("not enough players");
        if (!isSinglePlayer) return;
      }
      
      let allReady = true;
      this.state.players.forEach((player)=>{
        if (player.isReady !== true) allReady = false;
      });
      
      if (allReady){
        console.log("all players are ready");
        
        // wait until gameModel is initialized
        if (!this.gameModel.isInitialized) {
          console.log("gameModel is not initialized");
        }
        while (!this.gameModel.isInitialized){
          await new Promise(resolve => setTimeout(resolve, 500));
        }
        
        // spawn cards for every players
        for (const sessionId of this.state.players.keys()){
          this.state.players.get(sessionId).isReady = false;
          for (let i = 0; i < 5; i++) {
            this.gameModel.dealRandomCard(this.state, sessionId);
          }
          // this.gameModel.dealAllCard(this.state, ownerId);
          
          // console.log({ownerId})
          // let c = new Card();
          // c.cardId = uuidv4();
          // c.owner = ownerId;
          // this.state.cards.push(c);
        }
        this.state.turn = 1;
        this.state.gamePhase.current = this.state.gamePhase.playing;
        console.log("game start state = ", this.state.gamePhase.current);
        // game start
        // this.broadcast("gamePhase", this.state.gamePhase);
      }
    });
    
    this.onMessage(this.state.setCsv, (client, message: {csvUrl: string}) => {
      this.gameModel.initialize(message.csvUrl);
    });
    
    this.onMessage(this.state.cardPos, (client, cardPos: PositionMessage) => {
      if (this.state.gamePhase.current !== this.state.gamePhase.playing){
        console.log("game is not started");
        return;
      }
      console.log("set card pos " + {cardPos})
      // get card by id
      const card = this.state.cards.find((card)=>card.cardId === cardPos.id);
      card.x = cardPos.x;
      card.y = cardPos.y;
      card.z = cardPos.z;
      this.broadcast(this.state.onChangeCardPos, {id: card.cardId, x: card.x, y: card.y, z:card.z});
    });
    
    // Listen to position changes from the client.
    this.onMessage("position", (client, position: PositionMessage) => {
      const player = this.state.players.get(client.sessionId);
      player.x = position.x;
      player.y = position.y;
      console.log({position})
    });
    
    this.onMessage(this.state.setPlayerName, (client, message: {name: string}) => {
      console.log("onMessage: setPlayerName:",message.name);
      // set player name
      const player = this.state.players.get(client.sessionId);
      player.name = message.name;
        console.log("change player name");
    });
    
    this.onMessage(this.state.playCard, (client, message: { cardIds: string[], targetId: string}) => {
      console.log(`onMessage: playCard: ${message.cardIds[0]} -> ${message.targetId}`);
      // check game state
      if (this.state.gamePhase.current !== this.state.gamePhase.playing){
          console.log("game is not started");
          return;
      }
      // check message
      if (message.targetId === "" || message.targetId === undefined){
        message.targetId = client.sessionId;
        // console.log("target is not set");
        // return;
      }
      
      // set player hand
      const player = this.state.players.get(client.sessionId);
      if (player.hp <= 0){
        console.log("player is dead");
        return;
      }
      
      // set current card
      message.cardIds.forEach((cardId)=>{
        player.currentCardIds.push(cardId);
        player.hand.deleteAt(player.hand.indexOf(cardId));
      });
      player.currentTargetId = message.targetId;
      player.isReady = true;
      
      this.gameModel.setPlayCardData(this.state, player.currentCardIds, client.sessionId);
      
      // check all players has card
      let allPlayersHasCard = true;
      this.state.players.forEach((player)=>{
        const isPlayerAlive = player.hp > 0;
        if (player.currentCardIds.length == 0 && isPlayerAlive) allPlayersHasCard = false;
      });
      
      if (!allPlayersHasCard){
        console.log("all players not has card");
        return;
      }
      
      this.solveCards();
    });
    
    // draw
    this.onMessage(this.state.requestDrawRandomCard, (client, message: {num : number}) => {
      console.log(`onMessage: requestDrawRandomCard ${message.num}`);
      // check game state
      if (this.state.gamePhase.current !== this.state.gamePhase.playing){
          console.log("game is not started");
          return;
      }
      if (message.num <= 0){
        console.log("invalid num");
        return;
      }
      for (let i = 0; i < message.num; i++) {
        this.gameModel.dealRandomCard(this.state, client.sessionId);
      }
    })

    this.onMessage(this.state.requestDrawCardId, (client, message: {cardId: string}) => {
      console.log(`onMessage:  requestDrawCardId ${message.cardId}`);
      // check game state
      if (this.state.gamePhase.current !== this.state.gamePhase.playing){
        console.log("game is not started");
        return;
      }
      this.gameModel.dealCard(this.state, client.sessionId, message.cardId);
    })
    
    // all client solve view end
    this.onMessage(this.state.solved, (client, message: any) => {
      if (this.state.gamePhase.current !== this.state.gamePhase.solve){
        console.log("game is not solve phase");
        return;
      }

      for (const [sessionId, player] of this.state.players.entries()){
        player.currentCardIds.clear();
        player.currentTargetId = "";
        // deal 1 card
        this.gameModel.dealRandomCard(this.state, sessionId);
        // console.log(`PlayCard Execute player ${player.currentCardId} -> ${player.hp}`);
      }
      
      const gameState = this.gameModel.getGameState(this.state);
      if (gameState.playerCount == 1) {
        this.state.gamePhase.current = this.state.gamePhase.end;
        console.log(`Game End! Winner: ${gameState.winnerId}`);
        this.state.winnerId = gameState.winnerId;
      } else {
        this.state.turn += 1;
        this.state.gamePhase.current = this.state.gamePhase.playing;
      }
      
    });
    this.onMessage("*", (client, type, message) => {
      //
      // Triggers when any other type of message is sent,
      // excluding "action", which has its own specific handler defined above.
      //
      // console.log(client.sessionId, "sent", type, message);
      this.broadcast(type, message);
    });
  }
  
  solveCards(){
    if (this.state.gamePhase.current !== this.state.gamePhase.playing){
      console.log("game is not started");
      return;
    }
    console.log("solveCards");
    this.state.gamePhase.current = this.state.gamePhase.solve;
    let results = [];
    let i = 0;
    for (const [sessionId, player] of this.state.players.entries()){
      player.isReady = false;
      const isPlayerAlive = player.hp > 0;
      if (!isPlayerAlive) continue;
      
      // console.log(`PlayCard Execute player ${player.currentCardId} -> ${player.currentTargetId}`);
      const targetPlayer = this.state.players.get(player.currentTargetId);
      const result = this.gameModel.solveCard(this.state, i, sessionId, player.currentTargetId);
      // this.broadcast(this.state.cardResult, result);
      // this.gameModel.dealRandomCard(this.state, clientId);
      this.gameModel.dealRandomCard(this.state, player.currentTargetId);
      results.push(result);
      console.log(`PlayCard Execute player ${result.targetHpBefore} -> ${result.targetHpAfter}`);
      i++;
    }
    this.broadcast(this.state.cardResult, results);
    
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
    if (this.state.players.has(client.sessionId)) {
      // ロビー・終了画面では削除
      if (this.state.gamePhase.current === this.state.gamePhase.end || this.state.gamePhase.current === this.state.gamePhase.start){
        this.state.players.delete(client.sessionId);
      }
      // ゲーム中はHPを0にして対応する
      else{
        const player = this.state.players.get(client.sessionId);
        player.hp = 0;
      }
    }
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }
  
}
