using Godot;
using System;

public partial class UIPanelFeatureFull : UITabContainer
{
	// Called when the node enters the scene tree for the first time.
	public Feature feature;
	TextEdit name;
	Label type;
	RichTextLabel description;
	public override void _Ready()
	{
		name = GetNode<TextEdit>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
	}

	public override void _Draw()
	{
		name.Text = feature.Name;
		type.Text = feature.ttype.Name;
		type.Text = feature.Description;
		base._Draw();
	}
}
