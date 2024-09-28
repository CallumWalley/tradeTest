using Godot;
using System;

public partial class UIAccordianPlanetarySystem : UIAccordian
{
	public PlanetarySystem planetarySystem {get;set;}
	public CanvasLayer canvasLayer {get; set;}
	public Camera camera {get; set;}
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/SatelliteSystem/UIAccordianSatelliteSystem.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		base._Ready();
		Button button = GetNode<Button>("Button");
		Container container = GetNode<Container>("Container");
		camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
		button.Text = planetarySystem.Name;
		button.Flat = true;
		button.ButtonPressed = false;
		button.Alignment = HorizontalAlignment.Left;
		container.Visible = false;
		VBoxContainer vb = new VBoxContainer();
		foreach (SatelliteSystem ss in planetarySystem)
		{	
			UIAccordianSatelliteSystem ui_ss = (UIAccordianSatelliteSystem)prefab_UIAccordian.Instantiate<UIAccordian>();
			ui_ss.satelliteSystem = ss;
			ui_ss.canvasLayer=canvasLayer;
			vb.AddChild(ui_ss);
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
		camera.Center(planetarySystem);
	}	
}
