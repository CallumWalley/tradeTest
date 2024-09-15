using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;

/// <summary>
/// Base class for features.
/// </summary>

public partial class FeatureBase : Entity
{
    /// <summary>
    ///  Contains factors pooled with parent rp.
    ///  Currently 000-800
    /// </summary>
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalOutput { get; set; } = new();
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalInput { get; set; } = new();

    /// <summary>
    ///  Contains factors not pooled with parent rp.
    ///  Currently 801-900
    /// </summary>
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsLocal { get; set; } = new();

    /// <summary>
    ///  Contains factors that are a single value.
    ///  Currently 900+
    /// </summary>
    public Resource.RDict<Resource.RStatic> FactorsSingle { get; set; } = new();

    public List<Conditions.IConditionable> Conditions { get { return GetChildren().Cast<Conditions.IConditionable>().ToList(); } }

    public FeatureBase Template { get; set; } = null;
    public Texture2D iconMedium;

    [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
    public string TypeSlug { get; set; } = "unset";
    // public string TypeName { get { return ttype.Name; } }
    [Export(PropertyHint.Enum, "planetary")]
    public Godot.Collections.Array<string> NeedsTags { get; set; } = new Godot.Collections.Array<string>();

    public bool IsBuildable()
    {
        if (Template is null) { return true; }
        // Hard code only buildable on planet.
        return (NeedsTags.Contains("planetary"));
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
    public void OnEFrame()
    {
        foreach (Conditions.IConditionable c in Conditions)
        {
            c.OnEFrame();
        }
    }
    // public Basic NewFeatureFromTemplate()
    // {
    //     Basic newFeature = new();
    //     newFeature.Template = this;
    //     newFeature.Name = Name;
    //     newFeature.Description = Description;
    //     foreach (var c in Conditions)
    //     {
    //         newFeature.AddCondition(c.Instantiate());   
    //     }
    //     newFeature.Tags = new(Tags);
    //     newFeature.iconMedium = iconMedium;
    //     return newFeature;
    // }
}