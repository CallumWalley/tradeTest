using Godot;
using System;
namespace Game;

public partial class UIRename : LineEdit
{
	public Entities.IEntityable entity;
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
			Text = entity.Name ?? "Unknown";
		}
	}

	public void OnTextSubmitted(string s)
	{
		entity.Name = s;
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
