using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionSetIndustryCap : ActionBase
{
    public FeatureBase Feature { get; set; }
    public override bool Visible { get { return Active; } }
    public override string Description { get; set; } = "Shut down, or restart this industry.";
    public override StringName Name { get; set; } = "Capacity Industry";
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
    public double NewTarget;

    public override void OnAction()
    {
        Feature.CapabilityTarget = NewTarget;
    }

}

