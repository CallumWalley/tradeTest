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
public partial class ConditionScale : ConditionBase
{
    // For things that have their primary properties modified by their size.
    [Export]
    public double InitialScale { get; set; } = 1;
    // public ConditionScale(double _initialScale = 1)
    // {
    //     InitialScale = _initialScale;
    // }

    //Called when added to feature.
    public override void OnAdd()
    {
        base.OnAdd();
        Feature.FactorsSingle.Add(new Resource.RStatic(901, InitialScale, InitialScale, "Size", "Size"));
    }
}