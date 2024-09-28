using Godot;
using System;

public partial class Map : Node
{
	[Export]
	public PlanetarySystem startView;

    public override void _Ready()
    {
        base._Ready();
		GetNode<Screen>("../Screen").DrawSystem(startView);
    }
}
