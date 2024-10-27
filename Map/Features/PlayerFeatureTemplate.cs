using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace Game;

/// <summary>
/// What features can be chosen to build are determined by templates designed by player.
/// </summary>
public partial class PlayerFeatureTemplate : Node
{
    // Temporary measure until feature componenets elaborated.
    [Export]
    public FeatureBase Feature;

    /// <summary>
    /// What is required to build this.
    /// </summary>
    [Export]
    public Godot.Collections.Dictionary ConstructionInputRequirements;

    /// <summary>
    /// How many times this must be paid.
    /// </summary>
    [Export]
    public float ConstructionCost;
    public string GenerateName()
    {
        return $"New {Feature.Name}";
    }

    public FeatureBase Instantiate()
    {
        FeatureBase newFeature = (FeatureBase)Feature.Duplicate();
        newFeature.Template = this;
        return newFeature;
    }
}