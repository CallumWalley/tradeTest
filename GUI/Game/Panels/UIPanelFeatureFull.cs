using Godot;
using System;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public Features.FeatureBase feature;
	UIRename name;
	Label type;
	RichTextLabel description;
	UIListResources factors;
	public override void _Ready()
	{
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		factors = new UIListResources();
		factors.Vertical = false;

		name.node = feature;
		
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
