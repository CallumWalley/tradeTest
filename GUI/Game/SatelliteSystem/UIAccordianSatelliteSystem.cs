using Godot;
using System;
namespace Game;

public partial class UIAccordianSatelliteSystem : UIAccordian
{
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/SatelliteSystem/UIAccordianSatelliteSystem.tscn");
	static readonly PackedScene prefab_UITabContainerDomain = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Domain/UITabContainerDomain.tscn");
	static readonly PackedScene prefab_UIWindow = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIWindow.tscn");


	public CanvasLayer canvasLayer { get; set; }

	public Camera camera { get; set; }
	public SatelliteSystem satelliteSystem { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Button button = GetNode<Button>("Button");
		Container container = GetNode<Container>("Container");
		camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
		button.Text = satelliteSystem.Name;
		button.Flat = true;
		button.ButtonPressed = false;
		button.Alignment = HorizontalAlignment.Left;
		container.Visible = false;
		VBoxContainer vb = new VBoxContainer();

		foreach (var domain in satelliteSystem.GetChildren())
		{
			if (typeof(Domain) == domain.GetType())
			{
				UIDomainNav uiw = new UIDomainNav();
				uiw.domain = ((Domain)domain);
				uiw.Flat = true;
				uiw.canvasLayer = canvasLayer;
				vb.AddChild(uiw);
			}
		}
		HBoxContainer hb = new HBoxContainer();
		HSeparator hs = new HSeparator();
		hb.AddChild(hs);
		hs.SizeFlagsVertical = SizeFlags.ShrinkBegin;
		hb.AddChild(vb);
		container.ThemeTypeVariation = "PanelContainerTransparent";
		container.AddChild(hb);
	}


	public override void ShowDetails(bool toggled)
	{
		base.ShowDetails(toggled);
		camera.Center(satelliteSystem);
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
			UITabContainerDomain uit = prefab_UITabContainerDomain.Instantiate<UITabContainerDomain>();

			uit.Init(satelliteSystem);
			uiw.AddChild(uit);
			canvasLayer.AddChild(uiw);
		}
	}
}