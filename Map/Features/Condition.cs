using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;

public partial class Condition
{
    // Condition is some logic affecting a feature, evaluated every EFrame
    public partial class BaseConditionFactory
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

        public virtual void Init()
        {

        }
        public virtual void OnEFrame() { }

        public virtual BaseConditionFactory Clone(){}
    }

    public partial class FulfilmentFactory : BaseConditionFactory
    {
        // For features representing a transformation of resources.
        public new string Name { get; set; }
        public new string Description { get; set; }
        public List<Resource.RStatic> inputs;
        public List<Resource.RStatic> outputs;
        public Resource.RGroupRequester<Resource.IResource> fulfilment;
        Features.FeatureBase Feature;

        public FulfilmentFactory(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            inputs = new List<Resource.RStatic>();
            foreach (KeyValuePair<int, double> kvp in x["input"])
            {
                Resource.RStatic newResource = new Resource.RStatic(kvp.Key, kvp.Value);
                inputs.Add(newResource);
            }
            outputs = new List<Resource.RStatic>();
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RStatic newResource = new Resource.RStatic(kvp.Key, kvp.Value);
                outputs.Add(newResource);
            }
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RStatic newResource = new Resource.RStatic(kvp.Key, kvp.Value);
                outputs.Add(newResource);
            }
        }

        public void Init()
        {
            foreach (Resource.IResource i in inputs)
            {
                Feature.FactorsGlobal[i.Type].Add(i);
            }
        }

        public override void OnEFrame()
        {
            // Feature.FactorsGlobal[1].Add()
        }
        public void FromString()
        {

        }
        public virtual BaseConditionFactory Clone(){}

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
