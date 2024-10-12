using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// What features can be chosen to build are determined by templates designed by player.
/// </summary>
public partial class PlayerFeatureTemplate : Node
{
    // Temporary measure until feature componenets elaborated.
    [Export]
    public FeatureBase Feature;

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