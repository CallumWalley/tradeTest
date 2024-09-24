using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A thing. Subtype of node. Should be used instead of node for game objects.
/// </summary>
public partial class Entities
{
    public interface IEntityable
    {
        string Name { get; set; }
        string Description { get; set; }
    }


}