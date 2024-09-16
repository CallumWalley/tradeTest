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
	UIListResources globalFactorsInput = new();
	UIListResources globalFactorsOutput = new();

	UIListResources localFactors = new();
	UIListResources singularFactors = new();

	UIList<ConditionBase> conditions = new();


	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");
	static readonly PackedScene prefab_conditionTiny = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/Condition/UIConditionTiny.tscn");


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
		singularFactors.Vertical = false;
		localFactors.Vertical = false;
		globalFactorsInput.Vertical = false;
		globalFactorsOutput.Vertical = false;
		conditions.Vertical = true;

		singularFactors.ShowBreakdown = false;
		localFactors.ShowBreakdown = true;
		globalFactorsInput.ShowBreakdown = true;
		globalFactorsOutput.ShowBreakdown = true;

		singularFactors.Init(feature.FactorsSingle);
		localFactors.Init(feature.FactorsLocal);
		globalFactorsInput.Init(feature.FactorsGlobalInput);
		globalFactorsOutput.Init(feature.FactorsGlobalOutput);
		conditions.Init(feature.Conditions, prefab_conditionTiny);

		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(singularFactors);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(localFactors);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(globalFactorsInput);
		GetNode<VBoxContainer>("PanelContainer/Details/Factors/VBoxContainer").AddChild(globalFactorsOutput);

		GetNode<VBoxContainer>("PanelContainer/Details/Conditions/VBoxContainer").AddChild(conditions);

		OnEFrameUpdate();


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
		globalFactorsInput.Update();
		globalFactorsOutput.Update();
		localFactors.Update();
		conditions.Update();
	}
}
