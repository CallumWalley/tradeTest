using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
namespace Game;

public partial class ConditionMaintainance : ConditionBase
{

    [Export]
    public Godot.Collections.Dictionary Factors;

    /// <summary>
    /// List of values needing to be proccessed during fulfillment stage.
    /// </summary>
    Dictionary<int, Resource.RGroup<Resource.RStatic>> maintainanceFullfillments = new();
    //public Resource.RStatic inputSecurity = new Resource.RStatic(802, 1, 1, "Input Fulfilment", "Input Fulfilment");

    /// <summary>
    /// For non accruable resources, output is scaled to fit demand.
    /// </summary>
    Resource.RDictStatic outputDemand = new();


    public override void OnAdd()
    {
        base.OnAdd();
        Name = "Condition";

        foreach (KeyValuePair<Variant, Variant> r in Factors)
        {
            switch ((int)r.Key)
            {
                // Accruable
                case < 999:
                    maintainanceFullfillments[(int)r.Key] = new Resource.RGroup<Resource.RStatic>(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Base Maintainance"), "maintainance", "Required for maintainance");
                    maintainanceFullfillments[(int)r.Key].Mux(Feature.FactorsSingle[901]);
                    Feature.FactorsOutput[(int)r.Key].Add(maintainanceFullfillments[(int)r.Key]);
                    // Scale
                    break;
                default:
                    throw new ArgumentException($"Invalid factor key {r.Key}");
            }
        }
    }


    public override void OnEFrame()
    {
        base.OnEFrame();

        double percentFulfilled = maintainanceFullfillments.Average(x => x.Value.Fraction());
        double change = (Mathf.Sqrt(percentFulfilled) - Feature.Condition) / (10 * Feature.Scale);

        Feature.Condition += change;

        switch (Feature.Condition)
        {
            case < 0.2:
                Description = $"{Feature.Name} is in terrible condition";
                break;
            case < 0.4:
                Description = $"{Feature.Name} is in poor condition";
                break;
            case < 0.6:
                Description = $"{Feature.Name} is in acceptable condition";
                break;
            case < 0.8:
                Description = $"{Feature.Name} is in good condition";
                break;
            default:
                Description = $"{Feature.Name} is in excellent condition";
                break;
        }
    }
}
