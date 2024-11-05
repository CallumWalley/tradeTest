using Godot;
using System;
using System.Numerics;

namespace Game;

public partial class Nav : VBoxContainer
{
	//static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/PlanetarySystem/UIAccordianPlanetarySystem.tscn");
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIAccordian.tscn");
	static readonly PackedScene prefab_UITabContainerPosition = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Position/UITabContainerPosition.tscn");
	Map map;

	CanvasLayer canvasLayer;

	Camera camera;

	public override void _Ready()
	{
		map = GetNode<Map>("/root/Global/Map");
		canvasLayer = GetNode<CanvasLayer>("../");
		camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
		foreach (Galaxy g in map.GetChildren())
		{
			AddChild(DrawGalaxy(g));
		}
	}

	public HBoxContainer DrawGalaxy(Galaxy g)
	{
		HBoxContainer a = new HBoxContainer();
		VBoxContainer vb = new VBoxContainer();
		foreach (PlanetarySystem ps in g)
		{
			//UIAccordianPlanetarySystem ui_ps = (UIAccordianPlanetarySystem)prefab_UIAccordian.Instantiate<UIAccordianPlanetarySystem>();
			UIAccordian ui_ps = prefab_UIAccordian.Instantiate<UIAccordian>();
			//ui_ps.Connect("ShowDetails", Callable.From(() => camera.Center(ps)));
			Button button = ui_ps.GetNode<Button>("Button");
			Container container = ui_ps.GetNode<Container>("Container");
			button.Text = ps.Name;
			button.Flat = true;
			button.ButtonPressed = true;
			button.Alignment = HorizontalAlignment.Left;
			container.Visible = false;
			VBoxContainer vb2 = new VBoxContainer();
			container.AddChild(vb2);
			//ui_ps.Expanded = true;
			// ui_ps
			// ui_ps.planetarySystem = ps;
			// ui_ps.canvasLayer = canvasLayer;
			foreach (SatelliteSystem ss in ps)
			{
				// UIButton uIButton = new UIButton();
				UIAccordian ui_ss = prefab_UIAccordian.Instantiate<UIAccordian>();

				ui_ss.button.Connect("pressed", Callable.From(() => camera.Center(ss)));
				ui_ss.button.Text = ss.Name;
				ui_ss.Name = $"{ss.Name}-nav";
				ui_ss.button.Flat = true;
				ui_ss.button.ButtonPressed = false;
				ui_ss.button.Alignment = HorizontalAlignment.Left;
				VBoxContainer vb1 = new VBoxContainer();
				ui_ss.container.AddChild(vb1);
				// // container2.Visible = false;
				// vb2.AddChild(uIButton);
				foreach (Entities.IPosition p in ss)
				{
					UIButton uIButton = new UIButton();
					uIButton.Name = $"{ss.Name}-nav";

					uIButton.Connect("pressed", Callable.From(() => camera.Center(ss)));


					uIButton.Text = p.Name;
					uIButton.Flat = true;
					uIButton.ButtonPressed = false;
					uIButton.Alignment = HorizontalAlignment.Left;
					vb1.AddChild(uIButton);
				}
				ui_ps.AddChild(ui_ss);
			}
			vb.AddChild(ui_ps);
		}
		HBoxContainer hb = new HBoxContainer();
		hb.AddChild(new HSeparator());
		hb.AddChild(vb);
		a.AddChild(hb);
		return a;
	}
	void OpenNewWindow(Entities.IPosition target)
	{
		UIWindow existingWindow = canvasLayer.GetNodeOrNull<UIWindow>($"UIWindow-{target.Name}");
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
	}
}
