using Godot;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Game;

public partial class UIActionFullSetIndustrySize : UIActionFull<ActionSetIndustrySize>
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
        Action.NewScale = Feature.Scale;
        spinBox.Value = Feature.Scale;
        button.Text = "Confirm";
        confirmButton.Connect("pressed", new Callable(this, "OnButtonConfirmPressed"));
        spinBox.Connect("value_changed", new Callable(this, "OnSpinboxValueChanged"));

        // hBoxContainer.AddChild(spinBox);
        // hBoxContainer.AddChild(confirmButton);
        // vBoxContainer.AddChild(hBoxContainer);
        button.TooltipText = "Click to expand details";
        spinBox.MinValue = 0;
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
    }


    void OnSpinboxValueChanged(double value)
    {
        if (spinBox.Value == Feature.Scale)
        {
            confirmButton.Disabled = true;
            confirmButton.TooltipText = "No change";
        }
        else if (!Action.Active)
        {
            confirmButton.Disabled = true;
        }
        else
        {
            confirmButton.Disabled = false;
            confirmButton.TooltipText = "Start Construction.";
            Action.NewScale = spinBox.Value;
        }
        Update();
    }
    public override void Update()
    {
        // Minimum value is currently operating.

        foreach (KeyValuePair<Variant, Variant> kvp in Feature.Template.ConstructionInputRequirements)
        {
            costEstimate[(int)kvp.Key].Sum = ((float)kvp.Value * Feature.Template.ConstructionCost * (spinBox.Value - Feature.Scale));
        }
        uIListResources.Update();
        if (Feature.UnderConstruction)
        {
            Disabled = true;
            Expanded = false;

            button.TooltipText = "Can only have one construction order at a time.";
        }
        else
        {
            Disabled = false;
            button.TooltipText = "Click to expand details";
        }
        button.Text = Action.Name;
        richTextLabelDetails.Text = Action.Description;
    }
}