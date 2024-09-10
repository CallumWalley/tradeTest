using Godot;
using System;

public partial class UIButtonToggleWindow : UIButton
{
	// Button used to show/hide a window panel.
	[Export]
	public UIWindow window;
	public override void _Ready()
	{
		Connect("pressed", new Callable(this, "OnButtonPressed"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void OnButtonPressed()
	{

		base.OnButtonPressed();
		window.Visible = ButtonPressed;
	}

	public override void _Draw()
	{
		base._Draw();
		ButtonPressed = window.Visible;
	}
}
