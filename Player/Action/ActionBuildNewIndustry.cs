using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionBuildNewIndustry : ActionBase
{
    public Entities.IPosition Position { get; set; }
    public PlayerFeatureTemplate Template { get; set; }
    public FeatureBase NewFeature { get; set; }
    public override bool Visible { get { return Active; } }
    public override string Description { get; set; } = "Plan a new industry.";
    public new StringName Name { get; set; } = "New Industry";
    public override bool Active
    {
        get
        {
            if (Position == null) { return false; }
            if (Template == null) { return false; }
            return true;
        }
    }
    public override Godot.Vector2 CameraPosition { get { return Position.CameraPosition; } }
    public override float CameraZoom { get { return Position.CameraZoom; } }
    public double NewScale;

    public override void OnAction()
    {
        FeatureBase newFeature = Template.Instantiate();
        newFeature.Scale = 0;
        newFeature.CapabilityActual = 0;
        newFeature.Condition = 1;
        Position.AddFeature(newFeature);

        // Create the Resize action, and execute it.
        ActionSetIndustrySize resizeAction = new ActionSetIndustrySize();
        resizeAction.Feature = newFeature;
        resizeAction.NewScale = NewScale;
        resizeAction.OnAction();
    }

}

