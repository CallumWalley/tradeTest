using Godot;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Game;

public partial class UIActionFullSetIndustrySize : UIActionFull, Lists.IListable<Entities.IAction>
{
    public ActionSetIndustrySize Action;
    public FeatureBase Feature;

    [Export]
    VBoxContainer vBoxContainer;
    [Export]
    HBoxContainer hBoxContainer;
    [Export]
    SpinBox spinBox;
    [Export]
    Button confirmButton;
    UIListResources uIListResources = new UIListResources();
    Resource.RDict<Resource.RStatic> costEstimate;

    public override void _Ready()
    {
        base._Ready();
        Action = new ActionSetIndustrySize();
        Action.Feature = (FeatureBase)Feature;
        spinBox.Value = Feature.Scale;
        spinBox.MinValue = 0;
        button.Text = "Confirm";
        confirmButton.Connect("pressed", new Callable(this, "OnButtonConfirmPressed"));
        spinBox.Connect("value_changed", new Callable(this, "OnSpinboxValueChanged"));

        // hBoxContainer.AddChild(spinBox);
        // hBoxContainer.AddChild(confirmButton);
        // vBoxContainer.AddChild(hBoxContainer);
        button.TooltipText = "Click to expand details";

        costEstimate = new Resource.RDict<Resource.RStatic>(Feature.Template.ConstructionInputRequirements.Select(x => new Resource.RStatic((int)x.Key, 0, 0, "Construction Cost", "How much this costs to build")));
        uIListResources.Init(costEstimate);
        hBoxContainer.AddChild(uIListResources);
        hBoxContainer.MoveChild(uIListResources, 1);
        // OnSpinboxValueChanged(spinBox.Value);
        Update();
    }

    void OnButtonConfirmPressed()
    {
        Action.OnAction();
        Visible = false;
        QueueFree();
    }
    void OnSpinboxValueChanged(double value)
    {
        if (spinBox.Value != Feature.Scale)
        {
            button.Disabled = true;
            button.TooltipText = "No change";
            return;
        }
        if (Action.Active)
        {
            button.Disabled = true;
            return;
        }
        ((ActionSetIndustrySize)Action).NewSize = spinBox.Value;
        Update();
    }
    public override void Update()
    {
        base.Update();
        foreach (KeyValuePair<Variant, Variant> kvp in Feature.Template.ConstructionInputRequirements)
        {
            costEstimate[(int)kvp.Key].Sum = (float)kvp.Value * Feature.Template.ConstructionCost * (spinBox.Value - Feature.Scale);
        }
        uIListResources.Update();
        button.Text = Action.Name;
        richTextLabelDetails.Text = Action.Description;
    }
}