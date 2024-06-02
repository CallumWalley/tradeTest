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
        protected JsonSerializer serializer = new();

        public virtual BaseCondition Instantiate() { return new BaseCondition(); }
    }
    public partial class ConstantFactory : BaseConditionFactory
    {
        public Resource.RList<Resource.RRequest> outputs;
        public ConstantFactory(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            outputs = new Resource.RList<Resource.RRequest>();
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RRequest newResource = new(kvp.Key, kvp.Value, _fulfilled: true);
                outputs.Add(newResource);
            }

        }
        public override BaseCondition Instantiate()
        {
            Constant condition = new();
            condition.Name = Name;
            condition.Description = Description;
            condition.outputs = outputs.Clone();

            return condition;
        }
    }


    public partial class FulfilmentFactory : BaseConditionFactory
    {
        // // For features representing a transformation of resources.
        // public new string Name { get; set; }
        // public new string Description { get; set; }
        public Resource.RList<Resource.RRequest> inputs;
        public Resource.RList<Resource.RRequest> outputs;
        public Resource.RStatic fulfilment;
        public FulfilmentFactory(string str)
        {
            Dictionary<string, Dictionary<int, double>> x = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, double>>>(str);
            inputs = new Resource.RList<Resource.RRequest>();
            foreach (KeyValuePair<int, double> kvp in x["input"])
            {
                Resource.RRequest newResource = new(kvp.Key, kvp.Value);
                inputs.Add(newResource);
            }
            outputs = new Resource.RList<Resource.RRequest>();
            foreach (KeyValuePair<int, double> kvp in x["output"])
            {
                Resource.RRequest newResource = new(kvp.Key, kvp.Value);
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

    public partial class BaseCondition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Features.Basic Feature { get; set; } // parent reference.

        public virtual void OnAdd() { } //Called when added to feature.
    }
    public partial class Fulfilment : BaseCondition
    {
        public Resource.RList<Resource.RRequest> inputs;
        public Resource.RList<Resource.RRequest> outputs;
        public Resource.RStatic fulfilment;

        public override void OnAdd()
        {
            foreach (Resource.RRequest r in inputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
            }
            foreach (Resource.RRequest r in outputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
            }
            Feature.FactorsLocal[801].Add(fulfilment);
        }
    }


    public partial class Constant : BaseCondition
    {
        public Resource.RList<Resource.RRequest> outputs;

        public override void OnAdd()
        {
            foreach (Resource.RRequest r in outputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
            }
        }
    }
}
