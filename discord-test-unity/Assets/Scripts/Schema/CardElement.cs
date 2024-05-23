// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class CardElement : Schema {
	[Type(0, "uint8")]
	public byte None = default(byte);

	[Type(1, "uint8")]
	public byte Physical = default(byte);

	[Type(2, "uint8")]
	public byte Mental = default(byte);

	[Type(3, "uint8")]
	public byte Technique = default(byte);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<byte> __NoneChange;
	public Action OnNoneChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.None));
		__NoneChange += __handler;
		if (__immediate && this.None != default(byte)) { __handler(this.None, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(None));
			__NoneChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __PhysicalChange;
	public Action OnPhysicalChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.Physical));
		__PhysicalChange += __handler;
		if (__immediate && this.Physical != default(byte)) { __handler(this.Physical, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(Physical));
			__PhysicalChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __MentalChange;
	public Action OnMentalChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.Mental));
		__MentalChange += __handler;
		if (__immediate && this.Mental != default(byte)) { __handler(this.Mental, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(Mental));
			__MentalChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __TechniqueChange;
	public Action OnTechniqueChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.Technique));
		__TechniqueChange += __handler;
		if (__immediate && this.Technique != default(byte)) { __handler(this.Technique, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(Technique));
			__TechniqueChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(None): __NoneChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(Physical): __PhysicalChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(Mental): __MentalChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(Technique): __TechniqueChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			default: break;
		}
	}
}

