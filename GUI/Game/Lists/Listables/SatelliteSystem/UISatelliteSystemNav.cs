using Godot;
using System;

public partial class UISatelliteSystemNav : Button
{
	public SatelliteSystem satelliteSystem;
	public CanvasLayer canvasLayer;
	static readonly PackedScene prefab_UITabContainerPool = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/TabContainers/UITabContainerPool.tscn");
	static readonly PackedScene prefab_UIWindow = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIWindow.tscn");

	public override void _Ready()
	{
		Connect("pressed", new Callable(this, "OnButtonPressed"));
	}
	public override void _Draw()
	{
		Text = satelliteSystem.Name;
	}
	public void OnButtonPressed()
	{
		UIWindow uiw = prefab_UIWindow.Instantiate<UIWindow>();
		UITabContainerPool uit = prefab_UITabContainerPool.Instantiate<UITabContainerPool>();

		uit.Init(satelliteSystem);
		uiw.AddChild(uit);
		uiw.Driven.Add(uit);
		canvasLayer.AddChild(uiw);
	}
}
