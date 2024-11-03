using Godot;
using System;
namespace Game;

public partial class UINavSatelliteSystem : Button
{
	public SatelliteSystem satelliteSystem;
	public CanvasLayer canvasLayer;
	static readonly PackedScene prefab_UITabContainerDomain = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UITabContainerDomain.tscn");
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
			existingWindow.Position += new Vector2I(0, 100);
		}
		else
		{
			UIWindow uiw = prefab_UIWindow.Instantiate<UIWindow>();
			uiw.Name = $"UIWindow-{satelliteSystem.Name}";
			UITabContainerSatelliteSystem uit = prefab_UITabContainerDomain.Instantiate<UITabContainerSatelliteSystem>();

			uit.satelliteSystem = satelliteSystem;
			uiw.AddChild(uit);
			canvasLayer.AddChild(uiw);
		}
		GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D").Center(satelliteSystem);
	}
}
