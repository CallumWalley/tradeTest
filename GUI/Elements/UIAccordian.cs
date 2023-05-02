using Godot;
using System;
using System.Collections.Generic;

public partial class UIAccordian : Control
{
	Button button;
	Container container;

	public bool Expanded { get { return container.Visible; } set { container.Visible = value; } }
	public override void _Ready()
	{
		base._Ready();
		container = GetNode<Container>("Container");
		button = GetNode<Button>("Button");

		button.Connect("toggled", new Callable(this, "ShowDetails"));
	}
	public void ShowDetails(bool toggled)
	{
		Expanded = toggled;
	}
}
