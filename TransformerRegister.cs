using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class TransformerRegister : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    List<TransformerType> list;

    public class TransformerType{
        public string Name { get; set; }
        public string Slug{ get; set; }
        public string Superclass{ get; set; }
        public string Subclass{ get; set; }
        public string[] Tags{ get; set; }
        public string Description{ get; set; }
        public string Image{ get; set; }
        public int defaultPrioroty;

        public Dictionary<int, float> Upkeep{ get; set; }
        public Dictionary<int, float> Operation{ get; set; }
        public Dictionary<int, float> Production{ get; set; }
        public Dictionary<int, float> Storage{ get; set; }

    }

    // Special transformer type for trade route;
    public static TransformerType TradeRoute{get{
        TransformerType tradeRoute = new TransformerType();
        tradeRoute.Name="Trade Route";
        tradeRoute.Slug="trade_route";
        tradeRoute.Superclass = "special";
        tradeRoute.Subclass = "trade_route";
        return tradeRoute;
    }}

    // public class TempOutType{

    // }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {   
        list = new List<TransformerType>(LoadFromFile());
    }

IEnumerable<TransformerType> LoadFromFile(){
    foreach (var file in System.IO.Directory.GetFiles("Transformers", "*.json")){
        using (StreamReader fi =  System.IO.File.OpenText(file)){
            JsonSerializer serializer = new JsonSerializer();
            foreach (TransformerType tt in (List<TransformerType>)serializer.Deserialize(fi, typeof(List<TransformerType>))){
                yield return tt;
            }
            
        }
    }
}

public TransformerType GetFromSlug(string slug){
    return list.Find(x => x.Slug == slug);
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
