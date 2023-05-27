using Godot;
using System;
using System.Collections.Generic;

public partial class Curves : Node
{

    public partial class NamedCurve : Curve2D
    {
        public string Name { get; set; }
    }
    [Export]
    public string aString;

    [Export]
    public Curve2D cumsum;
    [Export]

    public Godot.Collections.Array cdurves = new();
    // [Export]

    // public Dictionary<string, Curve2D> gddurves = new();
    // [Export]

    // public List<Curve> ldurves;
}
