using Godot;
using System;
using System.Numerics;


public partial class Nav : VBoxContainer
{
	static readonly PackedScene prefab_UIAccordian = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIAccordian.tscn");


	Map map;
	public override void _Ready()
	{
		map = GetNode<Map>("/root/Global/Map");
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
			vb.AddChild(DrawPlanetarySystem(ps));
		}
		HBoxContainer hb = new HBoxContainer();
		hb.AddChild(new HSeparator());
		hb.AddChild(vb);

		a.AddChild(hb);
		return a;
	}
	public UIAccordian DrawPlanetarySystem(PlanetarySystem ps)
	{
		UIAccordian ui_ps = prefab_UIAccordian.Instantiate<UIAccordian>();
		ui_ps.GetNode<Button>("Button").Text = ps.Name;
		ui_ps.GetNode<Button>("Button").Flat = true;
		ui_ps.GetNode<Button>("Button").ButtonPressed = false;
		ui_ps.GetNode<Container>("Container").Visible = false;
		VBoxContainer vb = new VBoxContainer();
		foreach (SatelliteSystem ss in ps)
		{
			vb.AddChild(DrawSatelliteSystem(ss));
		}
		HBoxContainer hb = new HBoxContainer();
		HSeparator hs = new HSeparator();
		hs.SizeFlagsVertical = SizeFlags.ShrinkBegin;
		hb.AddChild(hs);
		hb.AddChild(vb);
		ui_ps.GetNode<Container>("Container").AddChild(hb);
		return ui_ps;
	}
	public UIAccordian DrawSatelliteSystem(SatelliteSystem ss)
	{
		UIAccordian ui_d = prefab_UIAccordian.Instantiate<UIAccordian>();
		ui_d.GetNode<Button>("Button").Text = ss.Name;
		ui_d.GetNode<Button>("Button").Flat = true;
		ui_d.GetNode<Button>("Button").ButtonPressed = false;
		ui_d.GetNode<Container>("Container").Visible = false;
		VBoxContainer vb = new VBoxContainer();
		foreach (Domain domain in ss.GetChildren())
		{
			UIDomainNav uiw = new UIDomainNav();
			uiw.domain = (domain);
			uiw.Flat = true;
			uiw.canvasLayer = GetParent<CanvasLayer>();
			vb.AddChild(uiw);
		}
		HBoxContainer hb = new HBoxContainer();
		HSeparator hs = new HSeparator();
		hs.SizeFlagsVertical = SizeFlags.ShrinkBegin;
		hb.AddChild(hs);
		hb.AddChild(vb);
		ui_d.GetNode<Container>("Container").AddChild(hb);
		return ui_d;
	}
}
