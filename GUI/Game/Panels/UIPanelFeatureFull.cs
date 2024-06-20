using Godot;
using System;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public Features.Basic feature;
	UIRename name;
	Label type;
	RichTextLabel description;
	HFlowContainer tags;
	UIListResources globalFactors = new();
	UIListResources localFactors = new();

	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");

	public override void _Ready()
	{
		base._Ready();
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/HBoxContainer/Name");
		type = GetNode<Label>("PanelContainer/Details/MarginContainer/HBoxContainer/Type");
		description = GetNode<RichTextLabel>("PanelContainer/Details/Description");
		tags = GetNode<HFlowContainer>("PanelContainer/Details/Tags");


		foreach (string tag in feature.Tags)
		{
			UIPill pill = prefab_pill.Instantiate<UIPill>();
			pill.tag = tag;
			tags.AddChild(pill);
		}

		globalFactors.Vertical = false;
		localFactors.Vertical = false;

		globalFactors.ShowBreakdown = true;
		localFactors.ShowBreakdown = true;

		globalFactors.Init(feature.FactorsGlobal);
		localFactors.Init(feature.FactorsLocal);

		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(globalFactors);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(localFactors);

		globalFactors.Update();
		localFactors.Update();
	}

	public override void _Draw()
	{
		base._Draw();
		description.Text = feature.Description;
		name.node = feature;
	}



	public override void OnEFrameUpdate()
	{
		base.OnEFrameUpdate();
		globalFactors.Update();
		localFactors.Update();

	}
}
