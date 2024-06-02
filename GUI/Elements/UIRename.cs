using Godot;
using System;

public partial class UIRename : LineEdit
{
	public Node node;
	public override void _Ready()
	{
		base._Ready();
		Connect("text_submitted", new Callable(this, "OnTextSubmitted"));
	}

	public override void _Draw()
	{
		base._Draw();
		if (!HasFocus())
		{
			Text = node.Name;
		}
	}

	public void OnTextSubmitted(string s)
	{
		node.Name = s;
		ReleaseFocus();
	}

	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event.IsActionPressed("ui_accept"))
	// 	{
	// 		GD.Print("Deselect");
	// 		ReleaseFocus();
	// 	}
	// }
}
