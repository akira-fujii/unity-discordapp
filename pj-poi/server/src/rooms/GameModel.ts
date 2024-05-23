import {Card, MyRoomState, Player} from "./schema/MyRoomState";
import fs from 'fs';
import csvParser from 'csv-parser';
import process from "process";
import path from "path";
import * as https from 'https';

const handUnlimited = false; 

enum Element {
  None = 0,
  Physical = 1,
  Mental = 2,
  Technique = 3,
}

// 相性マトリックス
const compatibilityMatrix: number[][] = [
  // None, Physical, Mental, Technique
  [1,       1,        1,        1], // None
  [1,       1,        2,        0.5], // Physical
  [1,       0.5,      1,        2], // Mental
  [1,       2,        0.5,      1]  // Technique
];

export type CardData = {
  masterId: string,
  name: string,
  description: string,
  attack: number,
  defense: number,
  element: number,
  cost: number,
}

export type PositionMessage = {
  id: string,
  x: number,
  y: number,
  z: number,
}

export type PlayCardResultMessage = {
  results: PlayCardResult[],
}

export type PlayCardResult = {
  index: number,
  attackerId: string,
  targetId: string,
  damage: number,
  attackerPower: number,
  attackerElement: number,
  targetPower: number,
  targetElement: number,
  attackerHpBefore: number,
  attackerHpAfter: number,
  targetHpBefore: number,
  targetHpAfter: number,
  damageRatio: number,
  attackerCardIds: string[],
  targetCardIds: string[]
}

export type GameStateInfo ={
  playerCount : number,
  winnerId: string | null,
}
export class CardDataReader {
  private cardDataMap: Map<string, CardData> = new Map();

  // CSVファイルからデータを読み込むメソッド
  public async readCsvFile(filePath: string): Promise<void> {
    return new Promise((resolve, reject) => {
      fs.createReadStream(filePath)
          .pipe(csvParser())
          .on('data', (data: any) => {
            // CSVの各行をCardDataに変換してマップに追加
            const cardData: CardData = {
              masterId: data.masterId,
              name: data.name,
              attack: parseInt(data.attack),
              defense: parseInt(data.defense),
              element: parseInt(data.element),
              cost: parseInt(data.cost),
              description: data.description,
            };
            this.cardDataMap.set(cardData.masterId, cardData);
          })
          .on('end', () => {
            resolve();
          })
          .on('error', (error: any) => {
            reject(error);
          });
    });
  }

  public async readCsvFromUrl(url: string): Promise<void> {
    return new Promise((resolve, reject) => {
      https.get(url, (response) => {
        if (response.statusCode !== 200) {
          reject(new Error(`Failed to fetch CSV from URL. Status code: ${response.statusCode}`));
          return;
        }

        let rawData = '';
        response.on('data', (chunk) => {
          rawData += chunk;
        });

        response.on('end', () => {
          try {
            const csvData = rawData.toString();
            const rows = csvData.split('\n');
            let isFirstRow = true;
            rows.forEach((row) => {
              if (isFirstRow) {
                isFirstRow = false;
                return; // ヘッダー行をスキップ
              }
              const columns = row.split(',').map(cell => cell.replace(/^"|"$/g, ''));
              const cardData: CardData = {
                masterId: columns[0],
                name: columns[1],
                description: columns[2],
                cost: parseInt(columns[3]),
                attack: parseInt(columns[4]),
                defense: parseInt(columns[5]),
                element: parseInt(columns[6]),
              };
              this.cardDataMap.set(cardData.masterId, cardData);
            });
            resolve();
          } catch (error) {
            reject(error);
          }
        });

        response.on('error', (error) => {
          reject(error);
        });
      });
    });
  }

  // マップを取得するメソッド
  public getCardDataMap(): Map<string, CardData> {
    return this.cardDataMap;
  }
}

// stateは全てroomに持たせる
export class GameModel {
  cardDataReader: CardDataReader = new CardDataReader();
  isInitialized: boolean = false;
  
  initialize(csvUrl: string){
    this.cardDataReader = new CardDataReader();
    const appDirectory = process.cwd();
    // const csvFilePath = path.join(appDirectory, process.env.CARD_CSV_URL)
    console.log(`csv file path ${csvUrl}`);
    this.cardDataReader.readCsvFromUrl(csvUrl).then(r => {
      const count = this.cardDataReader.getCardDataMap().size;
      console.log(`read csv file ${count}`);
    });
    this.isInitialized = true;
  }
  
  setPlayCardData(state: MyRoomState, cardIds: string[], playerId: string): void{
    const player = state.players.get(playerId);
    player.currentTotalPower = 0;
    player.currentTotalDefense = 0;
    player.currentElement = Element.None;
    cardIds.forEach((cardId) => {
      const cardData = this.cardDataReader.getCardDataMap().get(cardId);
      player.currentTotalPower += cardData.attack;
      player.currentTotalDefense += cardData.defense;
      player.currentElement = cardData.element;
    });
  }
  solveCard(state: MyRoomState, index: number, attackerId: string, targetId: string): PlayCardResult{
    let amount = 0;
    const target = state.players.get(targetId);
    const attacker = state.players.get(attackerId);
    // calc damage
    const hpTmp = target.hp;
    const damage = this.calcDamage(attacker.currentTotalPower, target.currentTotalDefense, attacker.currentElement, target.currentElement);
    const damageRatio = this.getElementRatio(attacker.currentElement, target.currentElement);
    target.hp = Math.max(0, target.hp - damage);
    console.log(`onPlayCard result: ${attackerId} used ${attacker.currentTotalPower}(${attacker.currentElement}:${attacker.currentElement}) to ${targetId}(${target.currentTotalDefense}:${target.currentElement}), Damage: ${damage} HP: ${hpTmp} -> ${target.hp}`);
    // remove card from hand
    // const player = state.players.get(attackerId);
    //
    // player.currentCardIds.forEach((cardId) => {
    //   const cardIndex = player.hand.indexOf(cardId);
    //   player.hand.splice(cardIndex, 1);});
    return {
        index: index,
        attackerId: attackerId,
        targetId: targetId,
        damage: damage,
        attackerPower: attacker.currentTotalPower,
        attackerElement: attacker.currentElement,
        targetPower: target.currentTotalDefense,
        targetElement: target.currentElement,
        attackerHpBefore: attacker.hp,
        attackerHpAfter: attacker.hp,
        targetHpBefore: hpTmp,
        targetHpAfter: target.hp,
        damageRatio: damageRatio,
        attackerCardIds: attacker.currentCardIds,
        targetCardIds: target.currentCardIds,
        };
  }
  getElementRatio(attacker: Element, defender: Element): number {
    return compatibilityMatrix[attacker][defender];
  }
  
  calcDamage(attackerPower: number, defenderPower: number, attackerElement: Element, defenderElement: Element): number {
    return Math.max(attackerPower - defenderPower, 0) * this.getElementRatio(attackerElement, defenderElement);
  }
  
  getGameState(state: MyRoomState): GameStateInfo {
    let winnerId: string | null = null;
    // check last player
    let alivePlayers = 0;
    let lastPlayerId = "";
    state.players.forEach((player, playerId) => {
      if (player.hp <= 0) {
        console.log(`player ${playerId} is dead`);
      }else{
        alivePlayers++;
        lastPlayerId = playerId;
      }
    });
    
    if (alivePlayers === 1){
      winnerId = lastPlayerId;
    }
    
    return {
      playerCount: alivePlayers,
      winnerId: winnerId,
    };
  }

  dealCard(state: MyRoomState, playerId: string, cardId: string) {
    const player = state.players.get(playerId);
    console.log(`dealCard: ${cardId} -> ${playerId}`);
    player.hand.push(cardId);
  }
  dealRandomCard(state: MyRoomState, playerId: string) {
    const playerHandCount = state.players.get(playerId).hand.length;
    if (!handUnlimited && playerHandCount >= 10) {
      console.log(`dealRandomCard: ${playerId} hand is full`);
      return;
    }
    const cardDataMap = this.cardDataReader.getCardDataMap();
    const cardDataArray = Array.from(cardDataMap.values());
    const randomIndex = Math.floor(Math.random() * cardDataArray.length);
    const randomCardData = cardDataArray[randomIndex];
    const player = state.players.get(playerId);
    console.log(`dealRandomCard: ${randomCardData.masterId} -> ${playerId}`);
    player.hand.push(randomCardData.masterId);
  }
  
  dealAllCard(state: MyRoomState, playerId: string) {
    const cardDataMap = this.cardDataReader.getCardDataMap();
    const cardDataArray = Array.from(cardDataMap.values());
    const player = state.players.get(playerId);
    cardDataArray.forEach((cardData) => {
      player.hand.push(cardData.masterId);
    });
  }
  
  createPlayer(): Player{
    const p =  new Player();
    p.hp = 30;
    p.point = 0;
    return p;
  }
}
