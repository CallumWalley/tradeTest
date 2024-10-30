using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Linq;

namespace Game;

/// <summary>
/// Base class for features.
/// </summary>

public partial class FeatureBase : Node, Entities.IEntityable
{
    /// <summary>
    ///  Contains factors pooled with parent rp.
    ///  Currently 000-800
    /// </summary>
    ///     

    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalOutput { get; set; } = new();
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalInput { get; set; } = new();
    [Export]
    public bool UnderConstruction { get; set; } = false;
    [Export]
    public string Description { get; set; }
    /// <summary>
    ///  Contains factors not pooled with parent rp.
    ///  Currently 801-900
    /// </summary>
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsLocal { get; set; } = new();
    public Godot.Vector2 CameraPosition { get { throw new NotImplementedException(); } }

    public float CameraZoom { get { throw new NotImplementedException(); } }
    /// <summary>
    ///  Contains factors that are a single value.
    ///  Currently 900+
    /// </summary>
    public Resource.RDict<Resource.RStatic> FactorsSingle { get; set; } = new();

    public List<ConditionBase> Conditions { get { return GetChildren().Cast<ConditionBase>().ToList(); } }

    [Export]
    public PlayerFeatureTemplate Template { get; set; } = null;

    [Export]
    public Texture2D iconMedium;

    [Export(PropertyHint.File, "*.png")]
    public string SplashScreenPath;

    // [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
    [Export]
    public string TypeSlug { get; set; } = "unset";
    // public string TypeName { get { return ttype.Name; } }
    [Export]
    public Godot.Collections.Array<string> Tags { get; set; } = new Godot.Collections.Array<string>();

    public override void _Ready()
    {
        base._Ready();
        foreach (ConditionBase conditionBase in Conditions)
        {
            conditionBase.OnAdd();
        }
    }
    public void AddCondition(ConditionBase s)
    {
        AddChild(s);
        s.OnAdd();
    }
    public void RemoveCondition(ConditionBase s)
    {
        RemoveChild(s);
        s.OnRemove();
    }
    /// <summary>
    /// Called when setting requests, if custom logic required.
    /// </summary>
    public virtual void OnRequestSet()
    {
        foreach (ConditionBase c in Conditions)
        {
            c.OnRequestSet();
        }
    }

    /// <summary>
    /// OnResolve
    /// </summary>
    public void OnEFrame()
    {
        foreach (ConditionBase c in Conditions)
        {
            c.OnEFrame();
        }
        // Give thigns a nice name.
        foreach (Resource.RGroup<Resource.IResource> rgroup in FactorsGlobalOutput)
        {
            rgroup.Name = Name;
        }
        foreach (Resource.RGroup<Resource.IResource> rgroup in FactorsGlobalInput)
        {
            rgroup.Name = Name;
        }
    }

    [GameAttributes.Command]
    public void ChangeSize(double deltaSize)
    {
        if (UnderConstruction)
        {
            throw new Exception("Cannot start new construction, already under construction.");
        }
        if (deltaSize == 0)
        {
            throw new Exception("This does nothing");
        }
        // Cannot make a negative size.
        deltaSize = Math.Max(-FactorsSingle[901].Sum, deltaSize);


        ConditionConstruction underConstruction = new ConditionConstruction();
        underConstruction.Name = "Under Construction";
        underConstruction.Description = "Opening Soon...";
        underConstruction.Addition = deltaSize;
        underConstruction.InputRequirements = Template.ConstructionInputRequirements;
        underConstruction.Cost = Template.ConstructionCost;

        AddCondition(underConstruction);
    }
}