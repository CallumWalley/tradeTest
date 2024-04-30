using Godot;
using System;

public partial class UIWindowPoolFeaturePlan : UIWindow
{	
	[Export]
	public ResourcePool resourcePool;

	[Export]
	public UIPanelBuildTemplate templateList;

	[Export]
	public Button addButton;

	public override void _Ready()
	{
		base._Ready();
		addButton.Connect("pressed", new Callable(this, "OnButtonPressed"));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnButtonPressed(){
		Features.FeatureBase newFeature = templateList.selected.NewFeatureFromTemplate();
		resourcePool.AddFeature(newFeature);
		templateList.OnItemListItemSelected(newFeature.GetIndex());
		OnCloseRequested();
	}
}
