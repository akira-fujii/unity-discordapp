// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class MessageType : Schema {
	[Type(0, "string")]
	public string ready = default(string);

	[Type(1, "string")]
	public string cardPos = default(string);

	[Type(2, "string")]
	public string onChangeCardPos = default(string);

	[Type(3, "uint8")]
	public byte playerMove = default(byte);

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

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(ready): __readyChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(cardPos): __cardPosChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(onChangeCardPos): __onChangeCardPosChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(playerMove): __playerMoveChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			default: break;
		}
	}
}

