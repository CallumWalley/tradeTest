using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionSetIndustrySize : Node, Entities.IAction
{
    public FeatureBase Feature { get; set; }
    public bool Visible { get { return Active; } }
    public string Description { get; set; } = "Expand, Downsize, or dismantle this industry.";
    public new StringName Name { get; set; } = "Resize industry";
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
    public double NewSize;

    public virtual void OnAction()
    {
        ConditionConstruction underConstruction = new ConditionConstruction();
        underConstruction.Name = "Under Construction";
        underConstruction.Description = "Opening Soon...";
        underConstruction.NewSize = NewSize;
        underConstruction.InputRequirements = Feature.Template.ConstructionInputRequirements;
        underConstruction.Cost = Feature.Template.ConstructionCost;

        Feature.AddCondition(underConstruction);
    }

}

