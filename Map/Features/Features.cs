using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
public partial class Features : Node, IEnumerable<Features.FeatureBase>
{
    // All types of features are stored in here.
    public partial class FeatureBase : Node
    {
        public Resource.RList<Resource.RGroupRequester<Resource.IRequestable>> FactorsLocal { get; set; } = new();
        public Resource.RList<Resource.RGroupRequester<Resource.IRequestable>> FactorsGlobal { get; set; } = new();
        public List<Condition.BaseCondition> Conditions { get; set; } = new();

        public FeatureBase Template { get; set; } = null;
        public Texture2D iconMedium;

        [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
        public string TypeSlug { get; set; } = "unset";
        // public string TypeName { get { return ttype.Name; } }
        public HashSet<FeatureTag> Tags { get; set; }
        public string Description { get; set; }

        public bool IsBuildable(){
            if (Template is null){return true;}
            // Hard code only buildable on planet.
            return (Tags.Contains(featureTags["planetary"]));
        }

        public void AddCondition(Condition.BaseCondition s)
        {
            Conditions.Add(s);
        }
        public FeatureBase NewFeatureFromTemplate()
        {
            FeatureBase newFeature = new();
            newFeature.Template = this;
            newFeature.Name = Name;
            newFeature.Description = Description;
            foreach (var c in Conditions)
            {
                newFeature.AddCondition(c);
                
            }
            newFeature.Tags = new(Tags);
            newFeature.iconMedium = iconMedium;
            return newFeature;
        }
    }

    public IEnumerator<Features.FeatureBase> GetEnumerator()
    {
        foreach (Features.FeatureBase f in GetChildren())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override void _Ready()
    {
        foreach (string file in System.IO.Directory.GetFiles("Map/Features/Templates", "*.json"))
        {
            using (StreamReader fi = System.IO.File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                foreach (FeatureConstructor tt in (List<FeatureConstructor>)serializer.Deserialize(fi, typeof(List<FeatureConstructor>)))
                {
                    //CallDeferred("add_child", tt.Make());
                    AddChild(tt.Make());
                    tt.QueueFree();
                }
            }
        }
    }
    public partial class FeatureConstructor : Node
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

        /// <summary>
        /// Converts feature constructor to actual feature.
        /// </summary>
        /// <returns></returns>
        public FeatureBase Make()
        {
            FeatureBase featureBase = new();

            featureBase.TypeSlug = Slug;
            featureBase.Name = Name;
            featureBase.Conditions = new(GetConditionsFromTemplate(Conditions));
            featureBase.Tags = new();
            foreach (string _tag in Tags)
            {
                if (! featureTags.ContainsKey(_tag) ){
                    GD.Print($"Trying to add non existant tag {_tag}");
                    continue;
                }
                featureBase.Tags.Add(featureTags[_tag]);
            }
            featureBase.Description = Description;
            featureBase.FactorsGlobal = new();
            featureBase.FactorsLocal = new();

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
                    yield return new Condition.Fulfilment(kvp.Value);
                }
                else
                {
                    GD.Print("Condition class not found");
                }
            }
        }
    }

    /// <summary>
    /// Tags to identify feature types.
    /// </summary>
    public class FeatureTag
    {
        public string Slug;
        public string Name;
        public string Description;

        public FeatureTag(string _slug, string _name, string _description)
        {
            Slug = _slug;
            Name = _name;
            Description = _description;
        }
    }
    /// <summary>
    /// Dictionary of feature types.
    /// </summary>
    public static Dictionary<string, FeatureTag> featureTags = new Dictionary<string, FeatureTag>(){
        {"orbital", new FeatureTag("orbital", "Orbital", "Must be built in orbit")},
        {"planetary", new FeatureTag("planetary", "Planetary", "Must be built on the surface of a planet")}
    };
    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }
}
