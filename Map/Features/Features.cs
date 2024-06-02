using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;
public partial class Features : Node, IEnumerable<Features.BasicFactory>
{
    public override void _Ready()
    {
        foreach (string file in System.IO.Directory.GetFiles("Map/Features/Templates", "*.json"))
        {
            using StreamReader fi = System.IO.File.OpenText(file);
            JsonSerializer serializer = new();
            foreach (BasicFactory tt in (List<BasicFactory>)serializer.Deserialize(fi, typeof(List<BasicFactory>)))
            {
                AddChild(tt);
            }
        }
    }
    public IEnumerator<Features.BasicFactory> GetEnumerator()
    {
        foreach (Features.BasicFactory f in GetChildren().Cast<BasicFactory>())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    // All types of features are stored in here.
    public partial class Basic : Node
    {
        public Resource.RList<Resource.RGroup<Resource.IResource>> FactorsLocal { get; set; } = new();
        public Resource.RList<Resource.RGroupRequests<Resource.RRequest>> FactorsGlobal { get; set; } = new();
        public List<Condition.BaseCondition> Conditions { get; set; } = new();

        public Basic Template { get; set; } = null;
        public Texture2D iconMedium;

        [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
        public string TypeSlug { get; set; } = "unset";
        // public string TypeName { get { return ttype.Name; } }
        public List<string> Tags { get; set; }
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




    /// <summary>
    /// Factory for basic feature type.
    /// </summary>
    public partial class BasicFactory : Node
    {
        [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
        public string Slug { get; set; }
        [Export]
        public Godot.Collections.Array<string> Tags { get; set; } = new();

        [Export]
        public Godot.Collections.Dictionary<string, string> Conditions { get; set; } = new();
        [Export]
        public string Description { get; set; }
        [Export]
        public string Splash { get; set; }
        [Export]
        public Godot.Collections.Dictionary<int, double> FactorsGlobal { get; set; }
        public Godot.Collections.Dictionary<int, double> FactorsLocal { get; set; }

        [Export]
        public Texture2D iconMedium;


        /// <summary>
        /// Converts feature constructor to actual feature.
        /// </summary>
        /// <returns></returns>
        public Basic Instantiate()
        {
            Basic featureBase = new();

            featureBase.FactorsGlobal = new();
            featureBase.FactorsLocal = new();

            featureBase.TypeSlug = Slug;
            featureBase.Name = Name;
            featureBase.Conditions = new();
            foreach (Condition.BaseCondition condition in GetConditionsFromTemplate(Conditions))
            {
                featureBase.AddCondition(condition);
            }
            featureBase.Tags = Tags.ToList();

            featureBase.Description = Description;


            return featureBase;
        }
        IEnumerable<Resource.IRequestable> GetFactorsFromTemplate(Godot.Collections.Dictionary<int, double> template)
        {
            if (template == null) { yield break; }
            foreach (KeyValuePair<int, double> kvp in template)
            {
                yield return new Resource.RGroupRequests<Resource.IRequestable>(new Resource.RRequest(kvp.Key, kvp.Value, "Base Yield", "Base Yield", true), Name, Description);
            }
        }
        IEnumerable<Condition.BaseCondition> GetConditionsFromTemplate(Godot.Collections.Dictionary<string, string> template)
        {
            if (template == null) { yield break; }
            foreach (KeyValuePair<string, string> kvp in template)
            {
                // Select type based on key
                if (kvp.Key == "fulfilment")
                {
                    yield return new Condition.FulfilmentFactory(kvp.Value).Instantiate();
                }
                if (kvp.Key == "constant")
                {
                    yield return new Condition.ConstantFactory(kvp.Value).Instantiate();
                }
                else
                {
                    GD.Print("Condition class not found");
                }
            }
        }
    }

    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }
}
