using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
namespace Game;

/// <summary>
/// Represents a thing that can be larger or smaller.
/// </summary>
public partial class ConditionConstruction : ConditionBase
{
    /// <summary>
    /// How much to add
    /// </summary>
    [Export]
    public double NewScale { get; set; } = 1;
    Resource.RDict<Resource.RStatic> inputFullfillments = new();

    public double Progress;
    public double PercentComplete;
    public double AdjustedCost;
    public double ScaleFactor = 0.1;
    public ConditionConstruction() { }

    //  S = size of change
    //  E = Scaling factor
    //  C = Cost
    //  Fi = factor ideal (e.g. something in inputs)
    //
    // Total == S * (E * S) * F * C
    // 
    // A = amount paid.
    // Effectivness of paying.
    // A * C = Total when A + Fi
    // abs(Fi/A) * A * C 


    //Called when added to feature.
    public override void OnAdd()
    {
        base.OnAdd();
        Feature.UnderConstruction = true;
        // Most efficient amount of resource to allocate per step.
        Feature.FactorsSingle[901].Request = NewScale;
        // If decomissioning, factory must be shut down to that level first.
        if (NewScale < Feature.Scale)
        {
            Feature.CapabilityTarget = (Mathf.Min(Feature.CapabilityTarget * Feature.Scale, NewScale) / Feature.Scale);
        }
        if (Feature.Template.ConstructionInputRequirements == null) { OnCompletion(); return; }
        foreach (KeyValuePair<Variant, Variant> r in Feature.Template.ConstructionInputRequirements)
        {
            inputFullfillments[(int)r.Key] = new Resource.RStatic((int)r.Key, 0, (double)r.Value, "Construction", "Construction Cost");
            // inputFullfillments[input] = new Resource.RStatic(801, 0, 1, $"{Resource.Name((int)r.Key)} fullfillment.", $"{Resource.Name((int)r.Key)} fullfillment.");
            // Feature.FactorsInput[801].Mux(inputFullfillments[input]);
            Feature.FactorsInput[(int)r.Key].Add(inputFullfillments[(int)r.Key]);
        }

        AdjustedCost = GetAdjustedCost(Feature.Scale);
    }

    public double GetAdjustedCost(double size)
    {
        return Feature.Template.ConstructionCost * size - (ScaleFactor * size);
    }
    public override void OnEFrame()
    {
        base.OnEFrame();

        // Caclulate percentage done.
        // foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        // {
        //     kvp.Value.Sum = kvp.Key.Fraction();
        // }

        // The resource with the lowest percentage of fullfillment. 
        double lowestFraction = Feature.Template.ConstructionInputRequirements.Min(x => (inputFullfillments[(int)x.Key].Sum / ((double)x.Value)));

        PercentComplete += (lowestFraction - (lowestFraction * lowestFraction * 0.01)) / AdjustedCost;

        Description = string.Format("Constuction is {0:P0} complete.", PercentComplete);
        if (PercentComplete >= 0.99)
        {
            OnCompletion();
        }
    }

    public override void OnRemove()
    {
        Feature.UnderConstruction = false;
        base.OnRemove();
    }
    void OnCompletion()
    {
        foreach (Resource.RStatic input in inputFullfillments)
        {
            Feature.FactorsInput[input.Type].UnAdd(input);
        }
        if (NewScale == 0)
        {
            Feature.Site.RemoveFeature(Feature);
        }
        else
        {
            Feature.Scale = NewScale;
            // Update actual capability to match new size.
            Feature.CapabilityActual = Mathf.Min(Feature.CapabilityActual * (Feature.Scale / NewScale), 1);

            // Set target to match previous target, or max.
            Feature.CapabilityTarget = (Feature.CapabilityTarget == 1) ? 1 : Mathf.Min(Feature.FactorsSingle[903].Sum * (Feature.Scale / NewScale), 1);

        }

        OnRemove();
    }
}