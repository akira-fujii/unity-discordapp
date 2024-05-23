// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class MyRoomState : Schema {
	[Type(0, "string")]
	public string ready = default(string);

	[Type(1, "string")]
	public string setPlayerName = default(string);

	[Type(2, "string")]
	public string cardPos = default(string);

	[Type(3, "string")]
	public string playCard = default(string);

	[Type(4, "string")]
	public string cardResult = default(string);

	[Type(5, "string")]
	public string onChangeCardPos = default(string);

	[Type(6, "string")]
	public string requestDrawRandomCard = default(string);

	[Type(7, "string")]
	public string requestDrawCardId = default(string);

	[Type(8, "string")]
	public string solved = default(string);

	[Type(9, "string")]
	public string heal = default(string);

	[Type(10, "string")]
	public string damage = default(string);

	[Type(11, "string")]
	public string win = default(string);

	[Type(12, "string")]
	public string surrender = default(string);

	[Type(13, "string")]
	public string setCsv = default(string);

	[Type(14, "uint8")]
	public byte playerMove = default(byte);

	[Type(15, "map", typeof(MapSchema<Player>))]
	public MapSchema<Player> players = new MapSchema<Player>();

	[Type(16, "array", typeof(ArraySchema<Card>))]
	public ArraySchema<Card> cards = new ArraySchema<Card>();

	[Type(17, "ref", typeof(GamePhase))]
	public GamePhase gamePhase = new GamePhase();

	[Type(18, "uint8")]
	public byte turn = default(byte);

	[Type(19, "string")]
	public string winnerId = default(string);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<string> __readyChange;
	public Action OnReadyChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.ready));
		__readyChange += __handler;
		if (__immediate && this.ready != default(string)) { __handler(this.ready, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(ready));
			__readyChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __setPlayerNameChange;
	public Action OnSetPlayerNameChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.setPlayerName));
		__setPlayerNameChange += __handler;
		if (__immediate && this.setPlayerName != default(string)) { __handler(this.setPlayerName, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(setPlayerName));
			__setPlayerNameChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __cardPosChange;
	public Action OnCardPosChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.cardPos));
		__cardPosChange += __handler;
		if (__immediate && this.cardPos != default(string)) { __handler(this.cardPos, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(cardPos));
			__cardPosChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __playCardChange;
	public Action OnPlayCardChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playCard));
		__playCardChange += __handler;
		if (__immediate && this.playCard != default(string)) { __handler(this.playCard, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playCard));
			__playCardChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __cardResultChange;
	public Action OnCardResultChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.cardResult));
		__cardResultChange += __handler;
		if (__immediate && this.cardResult != default(string)) { __handler(this.cardResult, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(cardResult));
			__cardResultChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __onChangeCardPosChange;
	public Action OnOnChangeCardPosChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.onChangeCardPos));
		__onChangeCardPosChange += __handler;
		if (__immediate && this.onChangeCardPos != default(string)) { __handler(this.onChangeCardPos, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(onChangeCardPos));
			__onChangeCardPosChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __requestDrawRandomCardChange;
	public Action OnRequestDrawRandomCardChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.requestDrawRandomCard));
		__requestDrawRandomCardChange += __handler;
		if (__immediate && this.requestDrawRandomCard != default(string)) { __handler(this.requestDrawRandomCard, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(requestDrawRandomCard));
			__requestDrawRandomCardChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __requestDrawCardIdChange;
	public Action OnRequestDrawCardIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.requestDrawCardId));
		__requestDrawCardIdChange += __handler;
		if (__immediate && this.requestDrawCardId != default(string)) { __handler(this.requestDrawCardId, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(requestDrawCardId));
			__requestDrawCardIdChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __solvedChange;
	public Action OnSolvedChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.solved));
		__solvedChange += __handler;
		if (__immediate && this.solved != default(string)) { __handler(this.solved, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(solved));
			__solvedChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __healChange;
	public Action OnHealChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.heal));
		__healChange += __handler;
		if (__immediate && this.heal != default(string)) { __handler(this.heal, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(heal));
			__healChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __damageChange;
	public Action OnDamageChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.damage));
		__damageChange += __handler;
		if (__immediate && this.damage != default(string)) { __handler(this.damage, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(damage));
			__damageChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __winChange;
	public Action OnWinChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.win));
		__winChange += __handler;
		if (__immediate && this.win != default(string)) { __handler(this.win, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(win));
			__winChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __surrenderChange;
	public Action OnSurrenderChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.surrender));
		__surrenderChange += __handler;
		if (__immediate && this.surrender != default(string)) { __handler(this.surrender, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(surrender));
			__surrenderChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __setCsvChange;
	public Action OnSetCsvChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.setCsv));
		__setCsvChange += __handler;
		if (__immediate && this.setCsv != default(string)) { __handler(this.setCsv, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(setCsv));
			__setCsvChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __playerMoveChange;
	public Action OnPlayerMoveChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerMove));
		__playerMoveChange += __handler;
		if (__immediate && this.playerMove != default(byte)) { __handler(this.playerMove, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerMove));
			__playerMoveChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<MapSchema<Player>> __playersChange;
	public Action OnPlayersChange(PropertyChangeHandler<MapSchema<Player>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.players));
		__playersChange += __handler;
		if (__immediate && this.players != null) { __handler(this.players, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(players));
			__playersChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArraySchema<Card>> __cardsChange;
	public Action OnCardsChange(PropertyChangeHandler<ArraySchema<Card>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.cards));
		__cardsChange += __handler;
		if (__immediate && this.cards != null) { __handler(this.cards, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(cards));
			__cardsChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<GamePhase> __gamePhaseChange;
	public Action OnGamePhaseChange(PropertyChangeHandler<GamePhase> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.gamePhase));
		__gamePhaseChange += __handler;
		if (__immediate && this.gamePhase != null) { __handler(this.gamePhase, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(gamePhase));
			__gamePhaseChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __turnChange;
	public Action OnTurnChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.turn));
		__turnChange += __handler;
		if (__immediate && this.turn != default(byte)) { __handler(this.turn, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(turn));
			__turnChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __winnerIdChange;
	public Action OnWinnerIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.winnerId));
		__winnerIdChange += __handler;
		if (__immediate && this.winnerId != default(string)) { __handler(this.winnerId, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(winnerId));
			__winnerIdChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(ready): __readyChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(setPlayerName): __setPlayerNameChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(cardPos): __cardPosChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(playCard): __playCardChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(cardResult): __cardResultChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(onChangeCardPos): __onChangeCardPosChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(requestDrawRandomCard): __requestDrawRandomCardChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(requestDrawCardId): __requestDrawCardIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(solved): __solvedChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(heal): __healChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(damage): __damageChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(win): __winChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(surrender): __surrenderChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(setCsv): __setCsvChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(playerMove): __playerMoveChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(players): __playersChange?.Invoke((MapSchema<Player>) change.Value, (MapSchema<Player>) change.PreviousValue); break;
			case nameof(cards): __cardsChange?.Invoke((ArraySchema<Card>) change.Value, (ArraySchema<Card>) change.PreviousValue); break;
			case nameof(gamePhase): __gamePhaseChange?.Invoke((GamePhase) change.Value, (GamePhase) change.PreviousValue); break;
			case nameof(turn): __turnChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(winnerId): __winnerIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			default: break;
		}
	}
}

