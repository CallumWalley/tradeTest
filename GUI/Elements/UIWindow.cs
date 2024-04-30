using Godot;
using System;
using System.Collections.Generic;

public partial class UIWindow : Window
{
	public override void _Ready()
	{
		base._Ready();
		Connect("close_requested", new Callable(this, "OnCloseRequested"));
	}
	protected virtual void OnCloseRequested()
	{
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right)
		{
			OnCloseRequested();
		}
	}

	// public virtual void OnEFrameUI()
	// {
	// 	if (Driven == null){return;}
	// 	foreach (Control c in Driven)
	// 	{
	// 		((UIInterfaces.IEFrameUpdatable)c).OnEFrameUpdate();
	// 	}
	// }
}
