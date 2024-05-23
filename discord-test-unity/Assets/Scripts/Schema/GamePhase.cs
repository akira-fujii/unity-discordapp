// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.29
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class GamePhase : Schema {
	[Type(0, "uint8")]
	public byte current = default(byte);

	[Type(1, "uint8")]
	public byte start = default(byte);

	[Type(2, "uint8")]
	public byte playing = default(byte);

	[Type(3, "uint8")]
	public byte solve = default(byte);

	[Type(4, "uint8")]
	public byte end = default(byte);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<byte> __currentChange;
	public Action OnCurrentChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.current));
		__currentChange += __handler;
		if (__immediate && this.current != default(byte)) { __handler(this.current, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(current));
			__currentChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __startChange;
	public Action OnStartChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.start));
		__startChange += __handler;
		if (__immediate && this.start != default(byte)) { __handler(this.start, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(start));
			__startChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __playingChange;
	public Action OnPlayingChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playing));
		__playingChange += __handler;
		if (__immediate && this.playing != default(byte)) { __handler(this.playing, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playing));
			__playingChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __solveChange;
	public Action OnSolveChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.solve));
		__solveChange += __handler;
		if (__immediate && this.solve != default(byte)) { __handler(this.solve, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(solve));
			__solveChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<byte> __endChange;
	public Action OnEndChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.end));
		__endChange += __handler;
		if (__immediate && this.end != default(byte)) { __handler(this.end, default(byte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(end));
			__endChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(current): __currentChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(start): __startChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(playing): __playingChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(solve): __solveChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			case nameof(end): __endChange?.Invoke((byte) change.Value, (byte) change.PreviousValue); break;
			default: break;
		}
	}
}

