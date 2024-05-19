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
        protected JsonSerializer serializer = new ();

        public virtual BaseCondition Instantiate(){ return new BaseCondition();}
    }

    public partial class FulfilmentFactory : BaseConditionFactory
    {
        // For features representing a transformation of resources.
        public new string Name { get; set; }
        public new string Description { get; set; }
        public Resource.RList<Resource.RStatic> inputs;
        public Resource.RList<Resource.RStatic> outputs;
        public Resource.RStatic fulfilment;
        public FulfilmentFactory(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            inputs = new Resource.RList<Resource.RStatic>();
            foreach (KeyValuePair<int, double> kvp in x["input"])
            {
                Resource.RStatic newResource = new(kvp.Key, kvp.Value);
                inputs.Add(newResource);
            }
            outputs = new Resource.RList<Resource.RStatic>();
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RStatic newResource = new (kvp.Key, kvp.Value);
                outputs.Add(newResource);
            }
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RStatic newResource = new (kvp.Key, kvp.Value);
                outputs.Add(newResource);
            }
        }

        public override BaseCondition Instantiate()
        {
            Fulfilment fulfilment = new();
            fulfilment.Name = Name;
            fulfilment.Description = Description;
            fulfilment.inputs = inputs.Clone();
            fulfilment.outputs = outputs.Clone();
            fulfilment.fulfilment = new();

            return fulfilment;
        }
    }


    public partial class BaseCondition{
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public partial class Fulfilment : BaseCondition{
        public Resource.RList<Resource.RStatic> inputs;
        public Resource.RList<Resource.RStatic> outputs;
        public Resource.RStatic fulfilment;
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
