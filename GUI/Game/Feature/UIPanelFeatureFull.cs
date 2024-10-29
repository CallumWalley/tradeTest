using Godot;
using System;
namespace Game;

public partial class UIPanelFeatureFull : UIPanel
{
	// Called when the node enters the scene tree for the first time.
	public FeatureBase feature;
	UIRename name;

	Player player;
	private CanvasLayer screen;

	TextureRect splashScreen;
	Label type;
	RichTextLabel description;
	HFlowContainer tags;
	UIListResources globalFactorsInput = new();
	UIListResources globalFactorsOutput = new();

	UIListResources localFactors = new();
	UIListResources singularFactors = new();

	UIList<ConditionBase> conditions = new();

	TextureButton templateButton;
	public Godot.Collections.Array<string> NeedsTags { get; set; } = new Godot.Collections.Array<string>();

	static readonly PackedScene prefab_pill = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIPill.tscn");
	static readonly PackedScene prefab_conditionTiny = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Condition/UIConditionTiny.tscn");


	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");
		screen = GetNode<CanvasLayer>("/root/Global/Screen");
		name = GetNode<UIRename>("PanelContainer/Details/MarginContainer/VBoxContainer/Name");
		description = GetNode<RichTextLabel>("PanelContainer/Details/MarginContainer/VBoxContainer/Description");
		tags = GetNode<HFlowContainer>("PanelContainer/Details/MarginContainer/VBoxContainer/Tags");
		splashScreen = GetNode<TextureRect>("PanelContainer/Details/SplashScreen");
		templateButton = GetNode<TextureButton>("PanelContainer/Details/MarginContainer/VBoxContainer/HBoxContainer/Type");


		templateButton.Connect("pressed", new Callable(this, "OnTemplateButtonPressed"));


		if (ResourceLoader.Exists(feature.SplashScreenPath, "*.png"))
		{
			splashScreen.Texture = GD.Load<Texture2D>(feature.SplashScreenPath);
		}
		foreach (string tag in feature.Tags)
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


		name.entity = feature;
		VBoxContainer vbcf = GetNode<VBoxContainer>("PanelContainer/Details/TabContainer/Factors/VBoxContainer");
		VBoxContainer vbcc = GetNode<VBoxContainer>("PanelContainer/Details/TabContainer/Conditions/VBoxContainer");

		vbcf.AddChild(singularFactors);
		vbcf.AddChild(localFactors);
		vbcf.AddChild(globalFactorsInput);
		vbcf.AddChild(globalFactorsOutput);

		vbcc.AddChild(conditions);

		OnEFrameUpdate();
	}

	public override void _Draw()
	{
		base._Draw();
		description.Text = feature.Description;
	}

	public void OnTemplateButtonPressed()
	{
		UIWindow templateWindow = screen.GetNode<UIWindow>("UIIndustriesWindow");
		templateWindow.Popup();
		//UIPanelFeatureTemplateList templateList = templateWindow.GetNode<UIPanelFeatureTemplateList>("Industry Templates");
		//templateList.OnItemListItemSelected(templateList.featureList.IndexOf(feature.Template));
	}

	public override void OnEFrameUpdate()
	{
		base.OnEFrameUpdate();
		singularFactors.Update();
		globalFactorsInput.Update();
		globalFactorsOutput.Update();
		localFactors.Update();
		conditions.Update();

		if (feature.Template != null)
		{
			templateButton.TooltipText = $"Template: {feature.Template.Name}";
			templateButton.Disabled = false;
		}
		else
		{
			templateButton.TooltipText = $"No template";
			templateButton.Disabled = true;

		}

	}
}
