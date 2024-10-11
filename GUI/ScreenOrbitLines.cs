using Godot;
using System;
using System.Linq;

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
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // GD.Print(playerTrade.First<TradeRoute>().Head.GetCanvasTransform());
        //	GD.Print(GetLocalMousePosition());
        //	GD.Print(camera.GetViewportTransform() );
        QueueRedraw();
    }
    public override void _Draw()
    {
        foreach (Node2D s in system)
        {
            DrawArc(system.GetCanvasTransform() * system.Position, (s.Position * s.GetCanvasTransform()).X, 0, 6, 2, lineColor, 1, true);
        }
    }


    public Vector2[] MakeEllipse(Vector2 center, float semiMajorAxis, float semiMinorAxis, int count)
    {
        Vector2[] points = new Vector2[count];
        float step = 2 * Mathf.Pi / count;

        for (int i = 0; i < count; i++)
        {
            float t = i * step;
            float x = semiMajorAxis * Mathf.Cos(t) + center.X;
            float y = semiMinorAxis * Mathf.Sin(t) + center.Y;
            points[i] = (new Vector2(x, y));
        }

        return points;
    }
}