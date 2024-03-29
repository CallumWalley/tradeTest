using Godot;
using System;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public Feature feature;
	TextEdit name;
	Label type;
	RichTextLabel description;
	UIListResources factors;
	public override void _Ready()
	{
		name = GetNode<TextEdit>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		factors = new UIListResources();
		factors.Vertical=false;

		factors.Init(feature.Factors);
		GetNode<VBoxContainer>("PanelContainer/Details").AddChild(factors);	
		factors.Update();
	}

	public override void _Draw()
	{
		name.Text = feature.Name;
		type.Text = feature.ttype.Name;
		description.Text = feature.Description;

		base._Draw();
	}

	public void OnEFrameUpdate(){
		factors.Update();
	}
}
