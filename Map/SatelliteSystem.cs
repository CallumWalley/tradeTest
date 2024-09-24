using Godot;
using System;

public partial class SatelliteSystem : Node2D, Entities.IEntityable
{
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    public string Description { get; set; }

    [Export]
    public float Zoom;
}
