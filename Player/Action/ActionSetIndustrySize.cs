using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionSetIndustrySize : ActionBase
{
    public FeatureBase Feature { get; set; }
    public override bool Visible { get { return Active; } }
    public override string Description { get; set; } = "Expand, Downsize, or dismantle this industry.";
    public override StringName Name { get; set; } = "Resize industry";
    public override bool Active
    {
        get
        {
            if (Feature == null) { return false; }
            if (Feature.UnderConstruction) { return false; }
            if (Feature.Template == null) { return false; }
            return true;
        }
    }
    public override Godot.Vector2 CameraPosition { get { return Feature.CameraPosition; } }
    public override float CameraZoom { get { return Feature.CameraZoom; } }
    public double NewScale;

    public override void OnAction()
    {
        ConditionConstruction underConstruction = new ConditionConstruction();
        underConstruction.Name = "Under Construction";
        underConstruction.Description = "Opening Soon...";
        underConstruction.NewScale = NewScale;

        Feature.AddCondition(underConstruction);
    }

}

