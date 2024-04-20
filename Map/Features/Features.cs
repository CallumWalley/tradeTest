using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public partial class Features : Node
{
    // All types of features are stored in here.
    public Dictionary<string, FeatureBase> index = new();

    public partial class FeatureBase : Node
    {
        public Resource.RList<Resource.IRequestable> FactorsLocal { get; set; }
        public Resource.RList<Resource.IRequestable> FactorsGlobal { get; set; }
        public List<Condition.BaseCondition> Conditions { get; protected set; }
        public Texture2D iconMedium;

        public FeatureRegister.FeatureType ttype;

        [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h20,mine_surf_old,mine_h20_surf_old,reclaim,cfuel_water")]
        public string TypeSlug { get; set; } = "unset";
        public string TypeName { get { return ttype.Name; } }
        public string[] Tags { get; set; }
        public string Description { get; set; }

        public override void _Ready()
        {
            FactorsLocal = new();
            FactorsGlobal = new();
            Conditions = new();
        }

        public void AddCondition(Condition.BaseCondition s)
        {
            Conditions.Add(s);
        }
    }


    public override void _Ready()
    {

        foreach (string file in System.IO.Directory.GetFiles("Economy/Features", "*.json"))
        {
            using (StreamReader fi = System.IO.File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                foreach (FeatureConstructor tt in (List<FeatureConstructor>)serializer.Deserialize(fi, typeof(List<FeatureConstructor>)))
                {
                    index[tt.Slug] = tt.Make();
                }
            }
        }
        index["unset"] = new FeatureBase();
    }
    public partial class FeatureConstructor : Node
    {
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Dictionary<int, double> Factors { get; set; }

        public FeatureBase Make()
        {
            FeatureBase featureBase = new FeatureBase();
            featureBase.Name = Name;
            featureBase.Tags = Tags;
            featureBase.Description = Description;
            featureBase.FactorsGlobal = new(GetFactorsFromTemplate(Factors));
            return featureBase;
        }
        IEnumerable<Resource.IRequestable> GetFactorsFromTemplate(Dictionary<int, double> template)
        {
            if (template == null) { yield break; }
            foreach (KeyValuePair<int, double> kvp in template)
            {
                yield return new Resource.RGroupRequests<Resource.IRequestable>(new Resource.RRequest(kvp.Key, kvp.Value, "Base Yield", "Base Yield", true), Name, Description);
            }
        }
    }

    public FeatureBase GetFromSlug(string slug)
    {
        if (slug == null)
        {
            GD.PrintErr("Feature type unset");
            return new FeatureBase();
        }
        else
        {
            if (!index.ContainsKey(slug))
            {
                GD.PrintErr($"Feature type {slug} not found");
                return new FeatureBase();
            }
            return index[slug];
        }
    }
}
