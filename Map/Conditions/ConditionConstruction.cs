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
    // For things that have their primary properties modified by their size.
    [Export]
    public double Weight { get; set; } = 1;

    public double OldSize { get; set; }
    public double NewSize { get; set; }

    public Resource.RStatic completed;
    public ConditionConstruction() { }

    //Called when added to feature.
    public override void OnAdd()
    {
        base.OnAdd();
        completed = new Resource.RStatic(802, OldSize / NewSize, 1, "Under Construction", "Under Construction");
        Feature.FactorsLocal[802].Add(completed);
    }
    public override void OnEFrame()
    {
        base.OnEFrame();
        completed.Sum += 0.1;

        Feature.FactorsSingle[901].Sum += 0.1;
        // Why so complicated?
    }
}