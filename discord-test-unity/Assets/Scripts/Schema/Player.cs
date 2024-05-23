// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class Player : Schema {
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);

	[Type(2, "string")]
	public string name = default(string);

	[Type(3, "string")]
	public string avatar = default(string);

	[Type(4, "boolean")]
	public bool isReady = default(bool);

	[Type(5, "number")]
	public float hp = default(float);

	[Type(6, "number")]
	public float point = default(float);

	[Type(7, "string")]
	public string currentTargetId = default(string);

	[Type(8, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> hand = new ArraySchema<string>();

	[Type(9, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> currentCardIds = new ArraySchema<string>();

	[Type(10, "number")]
	public float currentTotalPower = default(float);

	[Type(11, "number")]
	public float currentTotalDefense = default(float);

	[Type(12, "uint8")]
	public byte currentElement = default(byte);

	/*
	 * Support for individual property change callbacks below...
	 */

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

	protected event PropertyChangeHandler<string> __nameChange;
	public Action OnNameChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.name));
		__nameChange += __handler;
		if (__immediate && this.name != default(string)) { __handler(this.name, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(name));
			__nameChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __avatarChange;
	public Action OnAvatarChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.avatar));
		__avatarChange += __handler;
		if (__immediate && this.avatar != default(string)) { __handler(this.avatar, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(avatar));
			__avatarChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<bool> __isReadyChange;
	public Action OnIsReadyChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.isReady));
		__isReadyChange += __handler;
		if (__immediate && this.isReady != default(bool)) { __handler(this.isReady, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(isReady));
			__isReadyChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __hpChange;
	public Action OnHpChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.hp));
		__hpChange += __handler;
		if (__immediate && this.hp != default(float)) { __handler(this.hp, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(hp));
			__hpChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __pointChange;
	public Action OnPointChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.point));
		__pointChange += __handler;
		if (__immediate && this.point != default(float)) { __handler(this.point, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(point));
			__pointChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __currentTargetIdChange;
	public Action OnCurrentTargetIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentTargetId));
		__currentTargetIdChange += __handler;
		if (__immediate && this.currentTargetId != default(string)) { __handler(this.currentTargetId, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentTargetId));
			__currentTargetIdChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArraySchema<string>> __handChange;
	public Action OnHandChange(PropertyChangeHandler<ArraySchema<string>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.hand));
		__handChange += __handler;
		if (__immediate && this.hand != null) { __handler(this.hand, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(hand));
			__handChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArraySchema<string>> __currentCardIdsChange;
	public Action OnCurrentCardIdsChange(PropertyChangeHandler<ArraySchema<string>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentCardIds));
		__currentCardIdsChange += __handler;
		if (__immediate && this.currentCardIds != null) { __handler(this.currentCardIds, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentCardIds));
			__currentCardIdsChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __currentTotalPowerChange;
	public Action OnCurrentTotalPowerChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentTotalPower));
		__currentTotalPowerChange += __handler;
		if (__immediate && this.currentTotalPower != default(float)) { __handler(this.currentTotalPower, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentTotalPower));
			__currentTotalPowerChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __currentTotalDefenseChange;
	public Action OnCurrentTotalDefenseChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentTotalDefense));
		__currentTotalDefenseChange += __handler;
		if (__immediate && this.currentTotalDefense != default(float)) { __handler(this.currentTotalDefense, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentTotalDefense));
			__currentTotalDefenseChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __currentElementChange;
	public Action OnCurrentElementChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentElement));
		__currentElementChange += __handler;
		if (__immediate && this.currentElement != default(byte)) { __handler(this.currentElement, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentElement));
			__currentElementChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(x): __xChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(y): __yChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(name): __nameChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(avatar): __avatarChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(isReady): __isReadyChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			case nameof(hp): __hpChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(point): __pointChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(currentTargetId): __currentTargetIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(hand): __handChange?.Invoke((ArraySchema<string>) change.Value, (ArraySchema<string>) change.PreviousValue); break;
			case nameof(currentCardIds): __currentCardIdsChange?.Invoke((ArraySchema<string>) change.Value, (ArraySchema<string>) change.PreviousValue); break;
			case nameof(currentTotalPower): __currentTotalPowerChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(currentTotalDefense): __currentTotalDefenseChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(currentElement): __currentElementChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			default: break;
		}
	}
}

