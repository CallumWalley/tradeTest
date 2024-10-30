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
    public double Addition { get; set; } = 1;

    [Export]
    public Godot.Collections.Dictionary InputRequirements;
    Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();

    public Resource.RStatic completed = new Resource.RStatic(802, 0, 1, "Percent", "Percent Complete");

    // How many times 'InputRequirements' must be paid for completion.
    [Export]
    public double Cost { get; set; } = 100;

    public Resource.RGroup<Resource.RStatic> fulfillment = new Resource.RGroup<Resource.RStatic>(801, "Fullfilment ");

    public ConditionConstruction() { }

    //Called when added to feature.
    public override void OnAdd()
    {
        base.OnAdd();
        Feature.UnderConstruction = true;
        if (Cost == 0) { OnCompletion(); }
        Feature.FactorsSingle[901].Request = Feature.FactorsSingle[901].Sum + Addition;
        if (InputRequirements == null) { return; }
        foreach (KeyValuePair<Variant, Variant> r in InputRequirements)
        {
            Resource.RGroup<Resource.RStatic> input = new(new Resource.RStatic((int)r.Key, 0, (double)r.Value, "Base", "Base input"));
            inputFullfillments[input] = new Resource.RStatic(801, 0, 1, $"{Resource.Name((int)r.Key)} fullfillment.", $"{Resource.Name((int)r.Key)} fullfillment.");
            Feature.FactorsGlobalInput[801].Mux(inputFullfillments[input]);
        }
    }
    public override void OnEFrame()
    {
        base.OnEFrame();

        // Caclulate percentage done.
        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            kvp.Value.Sum = kvp.Key.Fraction();
        }
        double p_of_full = (inputFullfillments.Count() > 0) ? inputFullfillments.Average(x => x.Value.Sum) : 1;
        completed.Sum += (Math.Max(0, p_of_full) / (Cost * Addition));
        if (completed.Sum >= 1)
        {
            OnCompletion();
        }
        Description = string.Format("Constuction is {0:P0} complete.", completed.Sum);
    }

    public override void OnRemove()
    {
        Feature.UnderConstruction = false;
        base.OnRemove();
    }
    void OnCompletion()
    {
        Feature.FactorsSingle[901].Sum += Addition;
        OnRemove();
    }
}