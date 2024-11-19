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

public partial class FeatureBase : Node, Entities.IFeature
{
    /// <summary>
    ///  Contains factors pooled with parent rp.
    ///  Currently 000-800
    /// </summary>
    ///

    public IEnumerable<Entities.IAction> Actions { get; private set; } = new List<Entities.IAction>();

    public Entities.IPosition Site { get { return (Entities.IPosition)GetParent(); } } // parent reference.

    [Export]
    public double Scale
    {
        get
        {
            return FactorsSingle[901].Sum;
        }
        set
        {
            FactorsSingle[901].Sum = value;
        }
    }
    [Export(PropertyHint.Range, "0,1,")]
    public double Condition
    {
        get
        {
            return FactorsSingle[902].Sum;
        }
        set
        {
            FactorsSingle[902].Sum = value;
        }
    }
    [Export(PropertyHint.Range, "0,1,")]

    public double CapabilityActual
    {
        get
        {
            return FactorsSingle[903].Sum;
        }
        set
        {
            FactorsSingle[903].Sum = value;
        }
    }
    [Export(PropertyHint.Range, "0,1,")]

    public double CapabilityTarget
    {
        get
        {
            return FactorsSingle[903].Request;
        }
        set
        {
            FactorsSingle[903].Request = value;
        }
    }


    // public double Cap
    public IEnumerator<Entities.ICondition> GetEnumerator()
    {
        foreach (Entities.ICondition f in GetChildren())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsOutput { get; set; } = new();
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsInput { get; set; } = new();
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
    public Texture2D IconMedium { get; set; }

    [Export(PropertyHint.File, "*.png")]
    public string SplashScreenPath { get; set; }

    // [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
    [Export]
    public string TypeSlug { get; set; } = "unset";
    // public string TypeName { get { return ttype.Name; } }
    [Export]
    public Godot.Collections.Array<string> Tags { get; set; } = new Godot.Collections.Array<string>();

    public override void _EnterTree()
    {
        // Add bare minimum factors for this feature.
        FactorsSingle[901].Name = "Size";
        FactorsSingle[901].Details = "How large this is.";
        FactorsSingle[903].ValueFormat = "{0:F1} Mm^2";
        FactorsSingle[902].Name = "Condition";
        FactorsSingle[902].Details = "How well maintained this is.";
        FactorsSingle[902].ValueFormat = "{0:P0}";
        FactorsSingle[903].Name = "Capacity";
        FactorsSingle[903].Request = 1;
        FactorsSingle[903].Sum = 1;
        FactorsSingle[903].Details = "What percentage of total capacity is this operating at.";
        FactorsSingle[903].ValueFormat = "{0:P0}";
    }

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
        foreach (Resource.RGroup<Resource.IResource> rgroup in FactorsOutput)
        {
            rgroup.Name = Name;
        }
        foreach (Resource.RGroup<Resource.IResource> rgroup in FactorsInput)
        {
            rgroup.Name = Name;
        }
    }
}