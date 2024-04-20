using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[Tool]
public partial class FeatureRegister : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Dictionary<string, FeatureType> types = new();
    FeatureType defaultFeature = new FeatureType();
    public partial class FeatureType
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Superclass { get; set; }
        public string Subclass { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int defaultPrioroty;
        public Dictionary<int, double> Factors { get; set; }

    }


    public interface IFeatureable
    {

        //////////////////////////
        // Internal state
        /////////////////////////

        // Output 

        public FeatureRegister.FeatureType ttype { get; set; }
        public string TypeName { get { return ttype.Name; } }
        public string TypeSlug { get { return ttype.Slug; } }
        public string TypeClass { get { return ttype.Superclass; } }
        public string TypeSubclass { get { return ttype.Subclass; } }
        public string TypeImage { get { return ttype.Image; } }


        //public string TypeRequirements{get{ttype.Requiremnts}
        public string[] Tags { get; set; }
        public string Description { get; set; }
    }


    // public class InputTypeLoader
    // {
    //     public int Type;
    //     public double Value;
    //     public InputTypeLoader(int i, double v, string tip)
    //     {

    //     }
    // }

    // Special Industry type for trade route;
    public static FeatureType TradeRoute
    {
        get
        {
            FeatureType tradeRoute = new FeatureType();
            tradeRoute.Name = "Trade Route";
            tradeRoute.Slug = "trade_route";
            tradeRoute.Superclass = "special";
            tradeRoute.Subclass = "trade_route";
            return tradeRoute;
        }
    }

    // public class TempOutType{

    // }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        defaultFeature.Description = "Very mysterious";
        defaultFeature.Slug = "Unknown";
        defaultFeature.Name = "Unknown";

        foreach (FeatureType t in LoadFromFile())
        {
            types[t.Slug] = t;
        }
        types["unset"] = defaultFeature;
    }

    IEnumerable<FeatureType> LoadFromFile()
    {
        //GD.Print(System.IO.Directory.GetFiles("Industrys", ".json"));
        foreach (string file in System.IO.Directory.GetFiles("Economy/Features", "*.json"))
        {
            using (StreamReader fi = System.IO.File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                foreach (FeatureType tt in (List<FeatureType>)serializer.Deserialize(fi, typeof(List<FeatureType>)))
                {
                    yield return tt;
                }
            }
        }
    }



    public FeatureType GetFromSlug(string slug)
    {
        if (slug == null)
        {
            GD.PrintErr("Feature type unset");
            return defaultFeature;
        }
        else
        {
            if (!types.ContainsKey(slug))
            {
                GD.PrintErr($"Feature type {slug} not found");
                return defaultFeature;
            }
            return types[slug];
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
