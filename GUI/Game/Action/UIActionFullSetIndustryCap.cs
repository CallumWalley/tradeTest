using Godot;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Game;
public partial class UIActionFullSetIndustryCap : UIActionFull<ActionSetIndustryCap>
{

    public Entities.IAction Action;
    public FeatureBase Feature;

    [Export]
    VBoxContainer vBoxContainer;
    [Export]
    HBoxContainer hBoxContainer;
    [Export]
    Slider slider;

    [Export]
    Label label;

    // cbf for mo
    // Resource.RDict<Resource.RStatic> costEstimate;

    public override void _Ready()
    {
        base._Ready();
        Action = new ActionSetIndustryCap();
        ((ActionSetIndustryCap)Action).Feature = (FeatureBase)Feature;
        slider.Value = Feature.CapabilityTarget * 100;

        label.Text = string.Format("{0:P0}", Feature.CapabilityTarget);

        slider.Connect("value_changed", new Callable(this, "OnSliderValueChanged"));

        slider.Connect("drag_ended", new Callable(this, "OnSliderDragEnded"));

        // hBoxContainer.AddChild(spinBox);
        // hBoxContainer.AddChild(confirmButton);
        // vBoxContainer.AddChild(hBoxContainer);
        // OnSpinboxValueChanged(spinBox.Value);
        Update();
    }

    void OnSliderDragEnded(bool value)
    {
        ((ActionSetIndustryCap)Action).NewTarget = (slider.Value / 100);
        Action.OnAction();
        Update();
    }
    void OnSliderValueChanged(double value)
    {
        label.Text = string.Format("{0:P0}", value / 100);
    }


    public override void Update()
    {
        base.Update();
    }
}
