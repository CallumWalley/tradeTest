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
    /// Represents a generic operating capability. Used to ramp up and down input/output values.
    /// </summary>
    // Resource.RStatic capabilityMain;
    /// <summary>
    /// List of values needing to be proccessed during fulfillment stage.
    /// </summary>
    Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();

    /// <summary>
    /// For non accruable resources, output is scaled to fit demand.
    /// </summary>
    Resource.RDictStatic outputDemand = new();


    public override void OnAdd()
    {
        base.OnAdd();
        Feature.FactorsLocal[801].Name = "Input Fulfillment";
        Feature.FactorsLocal[801].ValueFormat = "{0:P0}";
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
                        Feature.FactorsOutput[(int)r.Key].Mux(Feature.FactorsSingle[903]); // Cabability
                    }
                    // If input
                    else
                    {
                        Resource.RGroup<Resource.RStatic> input = new(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Base input"));
                        inputFullfillments[input] = new Resource.RStatic(801, 1, 1, $"{Resource.Name((int)r.Key)} fullfillment.", $"{Resource.Name((int)r.Key)} fullfillment.");
                        Feature.FactorsLocal[801].Mux(inputFullfillments[input]);
                        Feature.FactorsInput[(int)r.Key].Add(input);
                        Feature.FactorsInput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                        Feature.FactorsInput[(int)r.Key].Mux(Feature.FactorsSingle[903]); // Cabability
                    }
                    break;
                default:
                    throw new ArgumentException($"Invalid factor key {r.Key}");
            }


            // Cabability
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

        // Will approach equilibrium at setpoint
        if (Feature.Scale > 0)
        {
            Feature.CapabilityActual += (Feature.CapabilityTarget - Feature.CapabilityActual) / (Feature.Scale * 10);
        }

        // inputs
        // // Why so complicated?
        // if (inputSecurity is null) { return; }
        // inputSecurity.Sum += ((Feature.FactorsLocal[801].Sum - inputSecurity.Sum + ((GD.Randf() - 0.6))) / 10);
    }
}
