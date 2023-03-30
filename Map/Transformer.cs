using Godot;
using System;
using System.Collections.Generic;

public class Transformer : EcoNode
{ 
    public Resource output;

    [Export]
    public string slug;

    // [Export]
    public List<Resource> costUpkeep;
    // [Export]
    public List<Resource> costOperation;
    // [Export]
    public List<Resource> costProduction;
    public TransformerRegister.TransformerType ttype;
    public string TypeName {get{return ttype.Name;}}
    public string TypeSlug {get{return ttype.Slug;}}
    public string TypeClass {get{return ttype.Superclass;}}
    public string TypeSubclass {get{return ttype.Subclass;}}
    public string TypeImage {get{return ttype.Image;}}

    //public string TypeRequirements{get{ttype.Requiremnts}}

    public string[] Tags {get;set;}
    public string Description {get;set;}
    public int Prioroty {get;set;}

        // "slug": "f_dockyard",
        // "upkeep":{},
        // "operation":{"1":-1},
        // "production": {"901": 12},
        // "superclass": "trade",
        // "subclass":"freighter",
        // "tags":[],
        // "description": "Freighter Dockyard",
        // "image":"",
        // "defaultPrioroty":0,
        // "requirements":{
        //     "player":[],
        //     "installation":["orbital"]
        // },
        // "contructionCost":{},
        // "maxConstructionRate":1
    
    public override void _Ready()
    {
        base._Ready();
        ttype = GetNode<TransformerRegister>("/root/Global/TransformerRegister").GetFromSlug(slug);
        costUpkeep = new List<Resource>(GetFromTemplate(ttype.Upkeep));
        costOperation = new List<Resource>(GetFromTemplate(ttype.Operation));
        costProduction = new List<Resource>(GetFromTemplate(ttype.Production));
    }

    public IEnumerable<Resource> Upkeep(){
        return costUpkeep;
    }

    public IEnumerable<Resource> OperationCost(){
        return costOperation;
    }

    public virtual IEnumerable<Resource> Production(){
        return  costProduction;
    }

    public void Create(){
        
    }

    public IEnumerable<Resource> GetFromTemplate(Dictionary<int, float> template){
        foreach (KeyValuePair<int, float> kvp in template){
            yield return new ResourceStatic(kvp.Key, kvp.Value);
        }
    }
}
