using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;

public partial class Condition
{
    // Condition is some logic affecting a feature, evaluated every EFrame
    public partial class BaseCondition
    {
        // Base class for conditions.
        public string Name { get; set; }
        public string Description { get; set; }
        protected JsonSerializer serializer = new JsonSerializer();
        // public BaseCondition(Features.FeatureBase _Feature, string _name = "Unknown", string _description = "This doesn't concern you.")
        // {
        //     Feature = _Feature;
        //     Name = _name;
        //     Description = _description;
        // }

        public virtual void Init(){

        }
        public virtual void OnEFrame(){}
    }

    public partial class Fulfillment : BaseCondition
    {
        // For features representing a transformation of resources.
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource.IResource cause;
        public readonly Resource.IResource effect;
        Features.FeatureBase Feature;

        public Fulfillment(string str){
            var def = new {input=new{}, output=new{}};
            var x = JsonConvert.DeserializeAnonymousType(str, def);
        }

        public void Deserialize(string s){

        }

        public override void Init(){

        }

        public override void OnEFrame(){
            // Feature.FactorsGlobal[1].Add()
        }
        public void FromString(){

        }
        // public Fulfillment(Resource.IResource _cause, Resource.IResource _effect)
        // {
        //     string _name = "Resource Shortfall", 
        //     string _description = "Output is being affected by resource shortfall"

        //     cause = _cause;
        //     effect = _effect;
        // }
    }


    // public static Dictionary<string, BaseCondition> condtion_index = new Dictionary<string, BaseCondition>(){
    //     {"orbital", new OutputModifier("orbital", "Orbital", "Must be built in orbit")},
    // };
    // public class Resource.IResourceShortage : BaseFeature{
    //     Feature.Requester requester;
    //     Resource.RList outputs;
    //     public Resource.IResourceShortage(Feature _Feature, Feature.Requester _requester, List<Resource> _outputs) : base(_Feature){
    //         outputs = _outputs;
    //         requester = _requester;
    //     }
    // }
}
