using Godot;
using System;

public partial class UIDomainNav : Button
{
	public Domain domain;
	public CanvasLayer canvasLayer;
	static readonly PackedScene prefab_UITabContainerDomain = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/TabContainers/UITabContainerDomain.tscn");
	static readonly PackedScene prefab_UIWindow = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIWindow.tscn");

	public override void _Ready()
	{
		Connect("pressed", new Callable(this, "OnButtonPressed"));
	}
	public override void _Draw()
	{
		Text = domain.Name;
	}
	public void OnButtonPressed()
	{
		UIWindow existingWindow = canvasLayer.GetNodeOrNull<UIWindow>($"UIWindow-{domain.Name}");
		if (existingWindow != null)
		{
			existingWindow.Popup();
		}
		else
		{
			UIWindow uiw = prefab_UIWindow.Instantiate<UIWindow>();
			uiw.Name = $"UIWindow-{domain.Name}";
			UITabContainerDomain uit = prefab_UITabContainerDomain.Instantiate<UITabContainerDomain>();

			uit.Init(domain);
			uiw.AddChild(uit);
			canvasLayer.AddChild(uiw);
		}
		GetNode<Godot.Camera2D>("/root/Global/Map/Galaxy/Sol/Camera2D").Position = domain.Position;
	}
}