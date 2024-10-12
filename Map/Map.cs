using Godot;
using System;
namespace Game;

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
