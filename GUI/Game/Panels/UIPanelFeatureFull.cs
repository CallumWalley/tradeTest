using Godot;
using System;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public Features.FeatureBase feature;
	UIRename name;
	Label type;
	RichTextLabel description;
	HBoxContainer tags;
	UIListResources factors;

	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");

	

	public override void _Ready()
	{
		base._Ready();
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		tags = GetNode<HBoxContainer>("PanelContainer/Details/Tags");
		factors = new UIListResources();
		factors.Vertical = false;

		name.node = feature;

		foreach (Features.FeatureTag tag in feature.Tags)
		{
			UIPill pill = prefab_pill.Instantiate<UIPill>();
			pill.tag = tag;
			tags.AddChild(pill);
		}
		
		factors.Init(feature.FactorsGlobal);
		GetNode<VBoxContainer>("PanelContainer/Details").AddChild(factors);
		factors.Update();
	}

	public override void _Draw()
	{	
		base._Draw();
		description.Text = feature.Description;
	}


	public override void OnEFrameUpdate()
	{
		base.OnEFrameUpdate();
		factors.Update();
	}
}
