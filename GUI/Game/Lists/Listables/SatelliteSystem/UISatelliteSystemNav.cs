using Godot;
using System;

public partial class UISatelliteSystemNav : Button
{
	public SatelliteSystem satelliteSystem;
	public CanvasLayer canvasLayer;
	static readonly PackedScene prefab_UITabContainerDomain = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/TabContainers/UITabContainerDomain.tscn");
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
		UIWindow existingWindow = canvasLayer.GetNodeOrNull<UIWindow>($"UIWindow-{satelliteSystem.Name}");
		if (existingWindow != null)
		{
			existingWindow.Popup();
		}
		else
		{
			UIWindow uiw = prefab_UIWindow.Instantiate<UIWindow>();
			uiw.Name = $"UIWindow-{satelliteSystem.Name}";
			UITabContainerDomain uit = prefab_UITabContainerDomain.Instantiate<UITabContainerDomain>();

			uit.Init(satelliteSystem);
			uiw.AddChild(uit);
			canvasLayer.AddChild(uiw);
		}

	}
}
