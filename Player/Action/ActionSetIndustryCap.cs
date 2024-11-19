using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionSetIndustryCap : Node, Entities.IAction
{
    public FeatureBase Feature { get; set; }
    public bool Visible { get { return Active; } }
    public string Description { get; set; } = "Shut down, or restart this industry.";
    public new StringName Name { get; set; } = "Capacity Industry";
    public bool Active
    {
        get
        {
            if (Feature == null) { return false; }
            if (Feature.UnderConstruction) { return false; }
            if (Feature.Template == null) { return false; }
            return true;
        }
    }
    public Godot.Vector2 CameraPosition { get { return Feature.CameraPosition; } }
    public float CameraZoom { get { return Feature.CameraZoom; } }
    public double NewTarget;

    public virtual void OnAction()
    {
        Feature.CapabilityTarget = NewTarget;
    }

}

