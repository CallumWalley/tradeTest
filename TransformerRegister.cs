using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public partial class IndustryRegister : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    List<IndustryType> list;

    public partial class IndustryType
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Superclass { get; set; }
        public string Subclass { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int defaultPrioroty;
        public Dictionary<int, double> Consumption { get; set; }
        public Dictionary<int, double> Production { get; set; }
        public Dictionary<int, double> Storage { get; set; }

    }

    // public class IndustryInputTypeLoader
    // {
    //     public int Type;
    //     public double Value;
    //     public IndustryInputTypeLoader(int i, double v, string tip)
    //     {

    //     }
    // }

    // Special Industry type for trade route;
    public static IndustryType TradeRoute
    {
        get
        {
            IndustryType tradeRoute = new IndustryType();
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
        list = new List<IndustryType>(LoadFromFile());
    }

    IEnumerable<IndustryType> LoadFromFile()
    {
        //GD.Print(System.IO.Directory.GetFiles("Industrys", ".json"));
        foreach (string file in System.IO.Directory.GetFiles("Industrys", "*.json"))
        {
            using (StreamReader fi = System.IO.File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                foreach (IndustryType tt in (List<IndustryType>)serializer.Deserialize(fi, typeof(List<IndustryType>)))
                {
                    yield return tt;
                }
            }
        }
    }

    public IndustryType GetFromSlug(string slug)
    {
        return list.Find(x => x.Slug == slug);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(double delta)
    //  {
    //      
    //  }
}
