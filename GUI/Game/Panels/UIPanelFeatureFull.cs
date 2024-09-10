using Godot;
using System;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public FeatureBase feature;
	UIRename name;
	Label type;
	RichTextLabel description;
	HFlowContainer tags;
	UIListResources globalFactors = new();
	UIListResources localFactors = new();
	UIListResources singularFactors = new();

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
		singularFactors.Vertical = false;
		localFactors.Vertical = false;
		globalFactors.Vertical = false;

		singularFactors.ShowBreakdown = false;
		localFactors.ShowBreakdown = true;
		globalFactors.ShowBreakdown = true;

		singularFactors.Init(feature.FactorsSingle);
		localFactors.Init(feature.FactorsLocal);
		globalFactors.Init(feature.FactorsGlobal);

		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(singularFactors);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(localFactors);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(globalFactors);

		singularFactors.Update();
		localFactors.Update();
		globalFactors.Update();
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
		singularFactors.Update();
		globalFactors.Update();
		localFactors.Update();

	}
}
