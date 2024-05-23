// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class Card : Schema {
	[Type(0, "uint8")]
	public byte state = default(byte);

	[Type(1, "number")]
	public float x = default(float);

	[Type(2, "number")]
	public float y = default(float);

	[Type(3, "number")]
	public float z = default(float);

	[Type(4, "string")]
	public string cardId = default(string);

	[Type(5, "string")]
	public string owner = default(string);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<byte> __stateChange;
	public Action OnStateChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.state));
		__stateChange += __handler;
		if (__immediate && this.state != default(byte)) { __handler(this.state, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(state));
			__stateChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __xChange;
	public Action OnXChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.x));
		__xChange += __handler;
		if (__immediate && this.x != default(float)) { __handler(this.x, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(x));
			__xChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __yChange;
	public Action OnYChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.y));
		__yChange += __handler;
		if (__immediate && this.y != default(float)) { __handler(this.y, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(y));
			__yChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __zChange;
	public Action OnZChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.z));
		__zChange += __handler;
		if (__immediate && this.z != default(float)) { __handler(this.z, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(z));
			__zChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __cardIdChange;
	public Action OnCardIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.cardId));
		__cardIdChange += __handler;
		if (__immediate && this.cardId != default(string)) { __handler(this.cardId, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(cardId));
			__cardIdChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __ownerChange;
	public Action OnOwnerChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.owner));
		__ownerChange += __handler;
		if (__immediate && this.owner != default(string)) { __handler(this.owner, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(owner));
			__ownerChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(state): __stateChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(x): __xChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(y): __yChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(z): __zChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(cardId): __cardIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(owner): __ownerChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			default: break;
		}
	}
}

