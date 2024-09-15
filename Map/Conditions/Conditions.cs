using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class Condition
{
    // Utility function
    public static object TryGetDefault(Dictionary<string, object> dict, string key, object defaultValue = null)
    {
        dict.TryGetValue(key, out defaultValue);
        return defaultValue;
    }
    // Condition is some logic affecting a feature, evaluated every EFrame
    protected JsonSerializer serializer = new();
    public interface IConditionable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FeatureBase Feature { get; set; } // parent reference.

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnRemove() { } //Called when added to feature.
        public virtual void OnEFrame() { } //Called every eframe.

    }
    public partial class BaseCondition : IConditionable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FeatureBase Feature { get; set; } // parent reference.

        public BaseCondition() { }

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnRemove()
        {
            Feature.Conditions.Remove(this);
        } //Called when removed

        public virtual void OnEFrame() { } //Called every eframe.

    }
    public partial class ScaleCondition : BaseCondition
    {
        // For things that have their primary properties modified by their size.
        public double InitialScale { get; set; }

        public ScaleCondition(string str) : this(JsonConvert.DeserializeObject<Dictionary<string, string>>(str)) { }
        public ScaleCondition(Dictionary<string, string> kvp)
        {
            InitialScale = Convert.ToDouble(kvp["initialScale"]);
        }
        public ScaleCondition(double _initialScale = 1)
        {
            InitialScale = _initialScale;
        }

        public ScaleCondition() { }

        //Called when added to feature.
        public override void OnAdd()
        {
            base.OnAdd();
            Feature.FactorsSingle.Add(new Resource.RStatic(901, InitialScale, 0, "Size", "Size"));
        }
    }
    /// <summary>
    /// Represents a thing that can be larger or smaller.
    /// </summary>
    public partial class SimpleIndustry : ScaleCondition
    {


        public Resource.RDict<Resource.RStatic> outputs = new Resource.RDict<Resource.RStatic>();
        Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();

        // Sets 'output' in proportion to fulfillment.

        // public SimpleIndustry(object obj) : this() { }
        public SimpleIndustry(Dictionary<string, object> kvp) : this(
            Convert.ToDouble(TryGetDefault(kvp, "initialScale", "1")),
            (Dictionary<int, double>)TryGetDefault(kvp, "inputs", null),
            (Dictionary<int, double>)TryGetDefault(kvp, "outputs", null)
        )
        { }
        public SimpleIndustry(double _initialScale, Dictionary<int, double> _inputs, Dictionary<int, double> _outputs) : base(_initialScale)
        {
            // if (_inputs == null)
            // {
            //     foreach (KeyValuePair<int, double> kvp in _inputs)
            //     {
            //         Resource.RGroup<Resource.RStatic> newr = new(new Resource.RStatic(kvp.Key, 0, kvp.Value, "Base", "Base input"));
            //         // newr.Mux(Feature.FactorsSingle[800]);
            //         Resource.RStatic newf = new Resource.RStatic(801, 1, 0, $"{Resource.Name(kvp.Key)} Fullfilment.", $"How much of the requested resource was delivered");
            //         inputFullfillments[newr] = newf;
            //     }
            // }
            // if (_outputs == null)
            // {
            //     foreach (KeyValuePair<int, double> kvp in _outputs)
            //     {
            //         outputs.Add(new Resource.RStatic(kvp.Key, kvp.Value, kvp.Value, "Base", "Expected Yield"));
            //     }
            // }
        }
        public SimpleIndustry() { }
        public override void OnAdd()
        {
            base.OnAdd();

            foreach (Resource.RStatic r in outputs)
            {
                Feature.FactorsGlobalOutput[r.Type].Add(r);
                Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsLocal[801]); // Input fulfillment
                Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsSingle[901]); // Scale
                Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsLocal[802]); // Cabability
            }
            Feature.FactorsLocal[801].Name = "Input Fulfillment";
            Feature.FactorsLocal[801].Add(new Resource.RStatic(801, 1, 0, "Base", "Expected Fulfillment"));
            Feature.FactorsLocal[802].Name = "Capability";
            Feature.FactorsLocal[802].Add(new Resource.RStatic(802, 1, 0, "Base", "Cabability"));
            foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
            {
                /// fulfilment is equal to this
                Feature.FactorsLocal[801].Mux(kvp.Value);

                Feature.FactorsGlobalInput[kvp.Key.Type].Add(kvp.Key);
                Feature.FactorsGlobalInput[kvp.Key.Type].Mux(Feature.FactorsSingle[901]); // Scale
                Feature.FactorsGlobalInput[kvp.Key.Type].Mux(Feature.FactorsLocal[802]); // Cabability
            }
        }


        public override void OnEFrame()
        {
            base.OnEFrame();
            foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
            {
                kvp.Value.Set(kvp.Key.Fraction());
            }
        }
    }

    public partial class SlowStart : BaseCondition
    {

        // Starts efficiency out at zero and slowly increases.
        public SlowStart(Dictionary<string, object> kvp) : this(Convert.ToDouble(TryGetDefault(kvp, "initialModifier", "0"))) { }

        //public SlowStart(Dictionary<string, object> kvp) : this(Convert.ToDouble(TryGetDefault(kvp, "initialModifier", "0"))) { }
        public SlowStart(double initialModifier) { }
        public SlowStart() { }
        public Resource.RStatic slowStart = new Resource.RStatic(802, 0.0, 0, "Slow Start", "Slow Start Size");

        public override void OnAdd()
        {
            base.OnAdd();
            Feature.FactorsLocal[802].Mux(slowStart);
        }
        public override void OnRemove()
        {
            Feature.FactorsLocal[802].UnMux(slowStart);
            base.OnRemove();
        }
        public override void OnEFrame()
        {
            base.OnEFrame();
            slowStart.Set((0.1 * Feature.FactorsLocal[801].Sum) + slowStart.Sum);
            if (slowStart.Sum > 0.9)
            {
                OnRemove();
            }
        }
    }


}
