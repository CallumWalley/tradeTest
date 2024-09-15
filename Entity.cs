using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A thing. Subtype of node. Should be used instead of node for game objects.
/// </summary>
public partial class Entity : Node
{
    [Export]
    public string Description { get; set; }

}