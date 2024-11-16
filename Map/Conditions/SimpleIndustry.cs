using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
namespace Game;

public partial class SimpleIndustry : ConditionBase
{

    [Export]
    public Godot.Collections.Dictionary Factors;

    /// <summary>
    /// Capability value this starts at.
    // /// </summary>
    [Export(PropertyHint.Range, "0,1,0.01")]
    public double StartingCapability = 0.1;
    /// <summary>
    /// Represents a generic operating capability. Used to ramp up and down input/output values.
    /// </summary>
    // Resource.RStatic capabilityMain;
    /// <summary>
    /// List of values needing to be proccessed during fulfillment stage.
    /// </summary>
    Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();
    //public Resource.RStatic inputSecurity = new Resource.RStatic(802, 1, 1, "Input Fulfilment", "Input Fulfilment");
    public Resource.RStatic demand = new Resource.RStatic(802, 1, 1, "Demand", "Demand");

    /// <summary>
    /// For non accruable resources, output is scaled to fit demand.
    /// </summary>
    Resource.RDictStatic outputDemand = new();


    public override void OnAdd()
    {
        base.OnAdd();
        Feature.FactorsLocal[801].Name = "Input Fulfillment";
        // Feature.FactorsLocal[801].Add(new Resource.RStatic(801, 1, 0, "Base", "Expected Fulfillment"));
        Feature.FactorsLocal[802].groupMode = Resource.GroupMode.Min;
        Feature.FactorsLocal[802].Name = "Capability";
        Feature.FactorsLocal[802].groupMode = Resource.GroupMode.Min;
        //inputSecurity = new Resource.RStatic(802, StartingCapability, 1, "Input Security", "Local Resource insecurity is affecting output.");
        //Feature.FactorsLocal[802].Add(inputSecurity);
        //
        //Feature.FactorsLocal[802].Mux(Feature.FactorsLocal[802]);
        //Resource.RStatic demand = new Resource.RStatic((int)802, 1, 1, "Demand", "Demand");
        Feature.FactorsLocal[802].Mux(demand);
        // Feature.FactorsLocal[802].Mux(inputSecurity);
        //Feature.FactorsLocal[802].Mux(Feature.FactorsLocal[801]);

        foreach (KeyValuePair<Variant, Variant> r in Factors)
        {
            switch ((int)r.Key)
            {
                // Accruable
                case < 999:
                    // If output
                    if ((double)r.Value > 0)
                    {
                        Feature.FactorsOutput[(int)r.Key].Add(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Expected Yield"));
                        Feature.FactorsOutput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                        Feature.FactorsOutput[(int)r.Key].Mux(Feature.FactorsLocal[801]); // Fulfilment
                        Feature.FactorsOutput[(int)r.Key].Mux(Feature.FactorsLocal[802]); // Cabability
                    }
                    // If input
                    else
                    {
                        Resource.RGroup<Resource.RStatic> input = new(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Base input"));
                        inputFullfillments[input] = new Resource.RStatic(801, 1, 1, $"{Resource.Name((int)r.Key)} fullfillment.", $"{Resource.Name((int)r.Key)} fullfillment.");
                        Feature.FactorsLocal[801].Mux(inputFullfillments[input]);
                        Feature.FactorsInput[(int)r.Key].Add(input);
                        Feature.FactorsInput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                        Feature.FactorsInput[(int)r.Key].Mux(Feature.FactorsLocal[802]); // Cabability
                    }
                    break;
                default:
                    throw new ArgumentException($"Invalid factor key {r.Key}");
            }


            // Cabability
        }

        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            /// fulfilment is equal to this

        }
    }


    public override void OnEFrame()
    {
        base.OnEFrame();
        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            //double rolling = ((kvp.Key.Fraction() + kvp.Value.Sum) / 2);
            kvp.Value.Sum = kvp.Key.Fraction();

        }
        if ((Feature.FactorsLocal[801].Sum) < 1)
        {
            Visible = true;
            Name = "Resource shortage";
            Description = string.Format("A shortage of {0} is causing reduced output", string.Join(", ", inputFullfillments.Where(x => x.Key.State != 0).Select(x => Resource.Name(x.Key.Type))));
        }
        else
        {
            Visible = false;
        }
        // inputs
        // // Why so complicated?
        // if (inputSecurity is null) { return; }
        // inputSecurity.Sum += ((Feature.FactorsLocal[801].Sum - inputSecurity.Sum + ((GD.Randf() - 0.6))) / 10);
    }
}
