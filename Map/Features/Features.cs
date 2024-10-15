using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Helper class for features
/// </summary>
public partial class Features : Node
{
    // public class IFeatureConverter : JsonConverter
    // {
    //     public override bool CanConvert(Type objectType)
    // {
    //     return typeof(IFeatureConverter).IsAssignableFrom(objectType);
    // }

    // public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    // {
    //     JObject itemsJson = JObject.Load(reader);
    //     // You can use a property in the JSON to determine the type, like a "type" field
    //     List<Conditions.IConditionable> items = new();

    //     foreach (var property in itemsJson.Properties())
    //     {
    //         string typeName = property.Name;
    //         JObject itemObject = (JObject)property.Value;


    //         Conditions.IConditionable target = typeName switch
    //         {
    //             "simpleIndustry" => new SimpleIndustry(),
    //             _ => throw new NotSupportedException($"Unknown class type : {typeName}")
    //         };

    //         items.Add(target);
    //     }
    //     return items;
    // }

    // public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    // {
    //     throw new NotImplementedException();
    // }
    //}
    public override void _Ready()
    {
        // foreach (string file in System.IO.Directory.GetFiles("Map/Features/Templates", "*.json"))
        // {
        //     // using StreamReader fi = ;
        //     using JsonTextReader jsonReader = new JsonTextReader(System.IO.File.OpenText(file));


        //     JsonSerializerSettings settings = new JsonSerializerSettings
        //     {
        //         TypeNameHandling = TypeNameHandling.None,
        //         Converters = new List<JsonConverter> { new IFeatureConverter() }
        //     };

        //     //JObject partial = JObject.Parse(fi);
        //     foreach (BasicFactory tt in JsonSerializer.Create(settings).Deserialize<IEnumerable<BasicFactory>>(jsonReader))
        //     {
        //         AddChild(tt);
        //     }
        // }
    }



    /// <summary>
    /// Factory for basic feature type.
    /// </summary>
    // public partial class BasicFactory : Node
    // {
    //     [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h2o,planet_mine_minerals,planet_mine_h2o,reclaim,cfuel_water")]
    //     public string Slug { get; set; }
    //     [Export]
    //     public Godot.Collections.Array<string> NeedsTags { get; set; } = new();

    //     // [Export]
    //     public List<Conditions.IConditionable> Conditions { get; set; } = new();
    //     [Export]
    //     public string Description { get; set; }
    //     [Export]
    //     public string Splash { get; set; }

    //     [Export]
    //     public Texture2D iconMedium;

    //     /// <summary>
    //     /// Converts feature constructor to actual feature.
    //     /// </summary>
    //     // /// <returns></returns>
    //     // public FeatureBase Instantiate()
    //     // {
    //     //     FeatureBase featureBase = new();

    //     //     featureBase.FactorsGlobalOutput = new();
    //     //     featureBase.FactorsGlobalInput = new();

    //     //     featureBase.FactorsLocal = new();

    //     //     featureBase.TypeSlug = Slug;
    //     //     featureBase.Name = Name;
    //     //     featureBase.Conditions = new();
    //     //     foreach (Condition.BaseCondition condition in GetContitionsFromTemplate(Conditions))
    //     //     {
    //     //         featureBase.AddCondition(condition);
    //     //     }
    //     //     featureBase.NeedsTags = NeedsTags;
    //     //     featureBase.Description = Description;
    //     //     // Give factors sensible names.
    //     //     foreach (Resource.RGroup<Resource.IResource> f in featureBase.FactorsGlobalInput)
    //     //     {
    //     //         f.Name = $"{Name} Input";
    //     //     }
    //     //     foreach (Resource.RGroup<Resource.IResource> f in featureBase.FactorsGlobalOutput)
    //     //     {
    //     //         f.Name = $"{Name} Output";
    //     //     }
    //     //     return featureBase;
    //     // }
    //     IEnumerable<ConditionBase> GetContitionsFromTemplate(Godot.Collections.Dictionary<string, string> template)
    //     {
    //         if (template == null) { yield break; }
    //         // foreach (KeyValuePair<string, object> kvp in template)
    //         // {
    //         //     // Select type based on key
    //         //     // There must be a better way to do this.
    //         //     if (kvp.Key == "scaleCondition")
    //         //     {
    //         //         yield return new Condition.ScaleCondition(kvp.Value);
    //         //     }
    //         //     else if (kvp.Key == "simpleIndustry")
    //         //     {
    //         //         yield return new Condition.SimpleIndustry(kvp.Value);
    //         //     }
    //         //     else if (kvp.Key == "slowStart")
    //         //     {
    //         //         yield return new Condition.SlowStart(kvp.Value);
    //         //     }
    //         //     else
    //         //     {
    //         //         GD.Print($"Condition class '{kvp.Key}' not found");
    //         //     }
    //         // }
    //     }
    // }

    public Godot.Collections.Array<Node> Children { get { return GetChildren(true); } }
}
