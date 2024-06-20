using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class Condition
{
    // Condition is some logic affecting a feature, evaluated every EFrame
    protected JsonSerializer serializer = new();
    // public partial class BaseConditionFactory
    // {
    //     // Base class for conditions.
    //     public string Name { get; set; }
    //     public string Description { get; set; }
    //     protected JsonSerializer serializer = new();

    //     public virtual BaseCondition Instantiate() { return new BaseCondition(); }
    // }
    // public partial class ConstantFactory : BaseConditionFactory
    // {
    //     public Resource.RList<Resource.RRequest> outputs;
    //     public ConstantFactory(string str)
    //     {
    //         Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
    //         outputs = new Resource.RList<Resource.RRequest>();
    //         foreach (KeyValuePair<int, double> kvp in x["output"])
    //         {
    //             Resource.RRequest newResource = new(kvp.Key, kvp.Value, _fulfilled: true);
    //             outputs.Add(newResource);
    //         }

    //     }
    //     public override BaseCondition Instantiate()
    //     {
    //         Constant condition = new();
    //         condition.Name = Name;
    //         condition.Description = Description;
    //         condition.outputs = outputs.Clone();

    //         return condition;
    //     }
    // }


    // public partial class FulfilmentFactory : BaseConditionFactory
    // {
    //     // // For features representing a transformation of resources.
    //     // public new string Name { get; set; }
    //     // public new string Description { get; set; }
    //     public Resource.RList<Resource.RRequest> inputs = new();
    //     public Resource.RGroupList<Resource.RGroupRequests<Resource.RRequest>> outputs = new();
    //     public Resource.RStatic fulfilment;
    //     public FulfilmentFactory(string str)
    //     {
    //         Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
    //         foreach (KeyValuePair<int, double> kvp in x["input"])
    //         {
    //             Resource.RRequest newResource = new(kvp.Key, kvp.Value);
    //             inputs.Add(newResource);
    //         }
    //         foreach (KeyValuePair<int, double> kvp in x["output"])
    //         {
    //             Resource.RGroupRequests<Resource.RRequest> newResourceGroup = new(new Resource.RRequest(kvp.Key, kvp.Value));
    //             outputs.Add(newResourceGroup);
    //         }

    //     }



    //     // public override BaseCondition Instantiate()
    //     // {
    //     //     // Fulfilment fulfilment = new();
    //     //     // fulfilment.Name = Name;
    //     //     // fulfilment.Description = Description;
    //     //     // fulfilment.inputs = inputs.Clone();
    //     //     // fulfilment.outputs = outputs.Clone();
    //     //     // fulfilment.fulfilment = new();

    //     //     // return fulfilment;
    //     // }
    // }

    public partial class BaseCondition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Features.Basic Feature { get; set; } // parent reference.

        public BaseCondition() { }

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnEFrame() { } //Called every eframe.

    }
    public partial class Fulfilment : BaseCondition
    {

        // All Resources in outputs will have their quantity scaled based on percentage of input fullfilled.
        Dictionary<Resource.RRequest, Resource.RRequest> inputFullfillments = new();
        public Resource.RList<Resource.RRequest> outputs = new Resource.RList<Resource.RRequest>();

        public Fulfilment(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            //double input_fraction = 1 / x["input"].Count;
            foreach (KeyValuePair<int, double> kvp in x["input"])
            {
                Resource.RRequest newr = new(kvp.Key, kvp.Value, "Input", "Required input");
                Resource.RRequest newf = new Resource.RRequest(801, 1, $"{Resource.Name(kvp.Key)} Fullfilment.", $"How much of the requested resource was delivered");
                inputFullfillments[newr] = newf;
            }
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                outputs.Add(new Resource.RRequest(kvp.Key, kvp.Value, "Base", "Expected Yield", true));
            }

        }
        public override void OnAdd()
        {
            Feature.FactorsLocal[801].Name = "Input Fulfillment";
            Feature.FactorsLocal[801].Add(new Resource.RRequest(801, 1, "Base", "Expected Fulfillment", true));
            foreach (KeyValuePair<Resource.RRequest, Resource.RRequest> kvp in inputFullfillments)
            {
                Feature.FactorsLocal[801].Mux(kvp.Value);
                Feature.FactorsGlobal[kvp.Key.Type].Add(kvp.Key);
            }
            foreach (Resource.RRequest r in outputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
                Feature.FactorsGlobal[r.Type].Mux(Feature.FactorsLocal[801]);
            }

        }
        public override void OnEFrame()
        {

            foreach (KeyValuePair<Resource.RRequest, Resource.RRequest> kvp in inputFullfillments)
            {
                kvp.Value.Set(kvp.Key.Fraction());
            }
        }
    }


    public partial class Constant : BaseCondition
    {
        public Resource.RList<Resource.RRequest> outputs;
        public Constant(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            outputs = new Resource.RList<Resource.RRequest>();
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RRequest newResource = new(kvp.Key, kvp.Value, _fulfilled: true);
                outputs.Add(newResource);
            }
        }

        public override void OnAdd()
        {
            foreach (Resource.RRequest r in outputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
            }
        }
    }
}
