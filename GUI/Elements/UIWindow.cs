using Godot;
using System;

public partial class UIWindow : Window
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("close_requested", new Callable(this, "OnCloseRequested"));
	}
	protected virtual void OnCloseRequested()
	{
		GD.Print("Close Requested");
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right)
		{
			OnCloseRequested();
		}
	}
}
