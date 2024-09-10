using Godot;
using System;

/// summary
/// UI element for possible build options.
public partial class UIPanelFeatureFactoryFull : UIPanel
{
	public Features.BasicFactory feature;
	UIRename name;
	Label type;
	RichTextLabel description;
	HFlowContainer tags;
	UIListResources factors;

	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");

	public override void _Ready()
	{
		base._Ready();
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		tags = GetNode<HFlowContainer>("PanelContainer/Details/Tags");


		foreach (string tag in feature.NeedsTags)
		{
			UIPill pill = prefab_pill.Instantiate<UIPill>();
			pill.tag = tag;
			tags.AddChild(pill);
		}
		// factors.Init(feature.FactorsGlobal);
		// GetNode<VBoxContainer>("PanelContainer/Details").AddChild(factors);
		// factors.Update();
	}

	public override void _Draw()
	{
		base._Draw();
		description.Text = feature.Description;
		name.node = feature;
	}
}
