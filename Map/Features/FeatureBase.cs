using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;
// All types of features are stored in here.
public partial class FeatureBase : Node
{
    /// <summary>
    ///  Contains factors pooled with parent rp.
    ///  Currently 000-800
    /// </summary>
    public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobal { get; set; } = new();

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


    public List<Condition.BaseCondition> Conditions { get; set; } = new();

    public FeatureBase Template { get; set; } = null;
    public Texture2D iconMedium;

    [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
    public string TypeSlug { get; set; } = "unset";
    // public string TypeName { get { return ttype.Name; } }
    [Export(PropertyHint.Enum, "planetary")]
    public Godot.Collections.Array<string> Tags { get; set; } = new Godot.Collections.Array<string>();
    public string Description { get; set; }

    public bool IsBuildable()
    {
        if (Template is null) { return true; }
        // Hard code only buildable on planet.
        return (Tags.Contains("planetary"));
    }

    public void AddCondition(Condition.BaseCondition s)
    {
        Conditions.Add(s);
        s.Feature = this;
        s.OnAdd();
    }

    public void OnEFrame()
    {
        foreach (Condition.BaseCondition c in Conditions)
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