using Godot;
using System;
using System.Linq;
namespace Game;

public partial class ScreenOrbitLines : Control
{
    // Called when the node enters the scene tree for the first time.
    PlanetarySystem system;
    Camera camera;

    float count;

    Godot.Color lineColor = new Godot.Color(1, 1, 1, 1);
    public override void _Ready()
    {
        system = GetNode<PlanetarySystem>("/root/Global/Map/Galaxy/Sol System");
        camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
        foreach (Entities.IOrbital s in system)
        {
            UIMapOrbitLine nmol = new UIMapOrbitLine();
            nmol.element = s;
            nmol.orbiting = system;
            AddChild(nmol);
        }
    }
}