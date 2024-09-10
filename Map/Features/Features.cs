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
    public IEnumerator<BasicFactory> GetEnumerator()
    {
        foreach (BasicFactory f in GetChildren().Cast<BasicFactory>())
        {
            yield return f;
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
        public Texture2D iconMedium;

        /// <summary>
        /// Converts feature constructor to actual feature.
        /// </summary>
        /// <returns></returns>
        public FeatureBase Instantiate()
        {
            FeatureBase featureBase = new();

            featureBase.FactorsGlobal = new();
            featureBase.FactorsLocal = new();

            featureBase.TypeSlug = Slug;
            featureBase.Name = Name;
            featureBase.Conditions = new();
            foreach (Condition.BaseCondition condition in GetContitionsFromTemplate(Conditions))
            {
                featureBase.AddCondition(condition);
            }
            featureBase.Tags = Tags;

            featureBase.Description = Description;
            // Give factors sensible names.
            foreach (Resource.RGroup<Resource.IResource> f in featureBase.FactorsGlobal)
            {
                f.Name = Name;
            }

            return featureBase;
        }
        IEnumerable<Condition.BaseCondition> GetContitionsFromTemplate(Godot.Collections.Dictionary<string, string> template)
        {
            if (template == null) { yield break; }
            foreach (KeyValuePair<string, string> kvp in template)
            {
                // Select type based on key
                if (kvp.Key == "inputFulfilment")
                {
                    yield return new Condition.InputFulfilment(kvp.Value);
                }
                else if (kvp.Key == "fulfilmentOutput")
                {
                    yield return new Condition.FulfilmentOutput(kvp.Value);
                }
                else if (kvp.Key == "outputConstant")
                {
                    yield return new Condition.OutputConstant(kvp.Value);
                }
                else if (kvp.Key == "scalable")
                {
                    yield return new Condition.Scalable(kvp.Value);
                }
                else
                {
                    GD.Print($"Condition class '{kvp.Key}' not found");
                }
            }
        }
    }

    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }
}
