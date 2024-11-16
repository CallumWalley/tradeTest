using Godot;
using System;
using System.Collections.Generic;
namespace Game;

/// summary
/// UI element for possible build options.
public partial class UIPanelFeatureTemplateFull : UIPanel
{
	public PlayerFeatureTemplate template;
	[Export]
	UIRename name;
	[Export]
	TextureRect splashScreen;

	[Export]
	Label type;
	[Export]
	RichTextLabel description;
	[Export]
	HFlowContainer tags;
	[Export]
	MarginContainer factorMarginContainer;

	[Export]
	MarginContainer costMarginContainer;

	UIListResources factors;
	UIListResources cost;


	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");

	public override void _Ready()
	{
		base._Ready();
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		tags = GetNode<HFlowContainer>("PanelContainer/Details/Tags");

		splashScreen = GetNode<TextureRect>("PanelContainer/Details/SplashScreen");

		if (ResourceLoader.Exists(template.Feature.SplashScreenPath, "*.png"))
		{
			splashScreen.Texture = GD.Load<Texture2D>(template.Feature.SplashScreenPath);
		}

		foreach (string tag in template.NeedsTags)
		{
			UIPill pill = prefab_pill.Instantiate<UIPill>();
			pill.tag = tag;
			tags.AddChild(pill);
		}
		foreach (KeyValuePair<Variant, Variant> r in template.ConstructionInputRequirements)
		{
			cost = new UIListResources();
			//factors.Init(template.FactorsGlobal);
		}

		// GetNode<VBoxContainer>("PanelContainer/Details").AddChild(factors);
		// factors.Update();
	}

	public override void _Draw()
	{
		base._Draw();
		description.Text = template.Feature.Description;
		name.entity = template.Feature;
	}
}
