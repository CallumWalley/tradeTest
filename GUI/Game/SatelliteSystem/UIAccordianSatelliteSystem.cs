using Godot;
using System;

public partial class UIAccordianSatelliteSystem : UIAccordian
{
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/SatelliteSystem/UIAccordianSatelliteSystem.tscn");
	public CanvasLayer canvasLayer {get; set;}

	public Camera camera {get; set;}
	public SatelliteSystem satelliteSystem {get;set;}

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

		foreach (Domain domain in satelliteSystem.GetChildren())
		{
			UIDomainNav uiw = new UIDomainNav();
			uiw.domain = (domain);
			uiw.Flat = true;
			uiw.canvasLayer = canvasLayer;
			vb.AddChild(uiw);
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
	}	
}