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
    public interface IConditionable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Features.Basic Feature { get; set; } // parent reference.

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnEFrame() { } //Called every eframe.

    }
    public partial class BaseCondition : IConditionable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Features.Basic Feature { get; set; } // parent reference.

        public BaseCondition() { }

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnEFrame() { } //Called every eframe.

    }
    public partial class FulfilmentOutput : BaseCondition
    {
        public Resource.RList<Resource.RRequest> outputs = new Resource.RList<Resource.RRequest>();
        public FulfilmentOutput(string str) : this(JsonConvert.DeserializeObject<Dictionary<int, double>>(str)) { }

        public FulfilmentOutput(Dictionary<int, double> _outputs)
        {
            foreach (KeyValuePair<int, double> kvp in _outputs)
            {
                outputs.Add(new Resource.RRequest(kvp.Key, kvp.Value, "Base", "Expected Yield", true));
            }
        }

        public override void OnAdd()
        {
            base.OnAdd();
            foreach (Resource.RRequest r in outputs)
            {
                Feature.FactorsGlobal[r.Type].Add(r);
                Feature.FactorsGlobal[r.Type].Mux(Feature.FactorsLocal[801]);
            }
        }

    }

    public partial class InputFulfilment : BaseCondition
    {

        // All Resources in outputs will have their quantity scaled based on percentage of input fullfilled.
        Dictionary<Resource.RRequest, Resource.RRequest> inputFullfillments = new();

        public InputFulfilment(string str) : this(JsonConvert.DeserializeObject<Dictionary<int, double>>(str)) { }

        public InputFulfilment(Dictionary<int, double> inputs)
        {
            foreach (KeyValuePair<int, double> kvp in inputs)
            {
                Resource.RRequest newr = new(kvp.Key, kvp.Value, "Input", "Required input");
                Resource.RRequest newf = new Resource.RRequest(801, 1, $"{Resource.Name(kvp.Key)} Fullfilment.", $"How much of the requested resource was delivered");
                inputFullfillments[newr] = newf;
            }
        }
        public override void OnAdd()
        {
            base.OnAdd();
            Feature.FactorsLocal[801].Name = "Input Fulfillment";
            Feature.FactorsLocal[801].Add(new Resource.RRequest(801, 1, "Base", "Expected Fulfillment", true));
            foreach (KeyValuePair<Resource.RRequest, Resource.RRequest> kvp in inputFullfillments)
            {
                Feature.FactorsLocal[801].Mux(kvp.Value);
                Feature.FactorsGlobal[kvp.Key.Type].Add(kvp.Key);
            }


        }
        public override void OnEFrame()
        {
            base.OnEFrame();
            foreach (KeyValuePair<Resource.RRequest, Resource.RRequest> kvp in inputFullfillments)
            {
                kvp.Value.Set(kvp.Key.Fraction());
            }
        }
    }


    public partial class OutputConstant : BaseCondition
    {
        public Resource.RList<Resource.RRequest> outputs;

        public OutputConstant(string str) : this(JsonConvert.DeserializeObject<Dictionary<int, double>>(str)) { }

        public OutputConstant(Dictionary<int, double> _outputs)
        {
            outputs = new Resource.RList<Resource.RRequest>();
            foreach (KeyValuePair<int, double> kvp in _outputs)
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
