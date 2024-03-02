using Godot;
using System;

public partial class UITopLeft : Panel
{
	static readonly PackedScene prefab_TradeWindow = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/TabContainer/UITabContainerTrade.tscn");

	Button buttonTrade;
	UITabContainerTrade windowTrade;
	CanvasLayer screen;


	public override void _Ready()
	{
		screen = GetNode<CanvasLayer>("/root/Global/Screen");

		// Button open trade window.
		buttonTrade = GetNode<Button>("HBoxContainer/ButtonTrade");
		buttonTrade.Connect("pressed", new Callable(this, "OpenTrade"));

		windowTrade = GetNode<UITabContainerTrade>("../UITradeWindow");
		windowTrade.toggleButton = buttonTrade;
		windowTrade.Position = (Vector2I)Position + new Vector2I(100, 100);
	}

	public void OpenTrade()
	{
		windowTrade.Visible = buttonTrade.ButtonPressed;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
