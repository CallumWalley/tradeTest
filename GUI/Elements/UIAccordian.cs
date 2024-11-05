using Godot;
using System;
using System.Collections.Generic;

public partial class UIAccordian : Control
{
	[Export]
	public Button button;
	[Export]
	public Container container;
	[Signal]
	public delegate void ShowDetailsEventHandler();
	public bool Expanded { get { return container.Visible; } set { container.Visible = value; } }
	public override void _Ready()
	{
		base._Ready();

		button.Connect("toggled", new Callable(this, "OnToggled"));
	}
	public virtual void OnToggled(bool toggled)
	{
		EmitSignal(SignalName.ShowDetails);
		Expanded = toggled;
	}
}
