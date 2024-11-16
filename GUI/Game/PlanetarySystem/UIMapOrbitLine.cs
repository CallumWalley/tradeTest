using Godot;
using System;
namespace Game;

public partial class UIMapOrbitLine : Line2D
{

    public Entities.IOrbital element;
    public Node2D orbiting;

    Camera camera;

    // is mouse over this element.


    public override void _Ready()
    {
        base._Ready();
        camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
        Width = 10;
        Closed = true;


    }

    public override void _Draw()
    {
        base._Draw();
        //Width = 1 + camera.Zoom.X;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (IsVisibleInTree())
        {
            GlobalPosition = ((Node2D)element).GetCanvasTransform() * ((Node2D)orbiting).GlobalPosition;
            Points = MakeEllipse(((Node2D)orbiting).GlobalPosition, (((Node2D)element).Position * orbiting.GetCanvasTransform()).Length(), 0, 100);
            QueueRedraw();
        }

    }
    public Vector2[] MakeEllipse(Vector2 center, float semiMajorAxis, float eccentricity, int count)
    {
        Vector2[] points = new Vector2[count];
        float step = (2 * Mathf.Pi) / count;

        for (int i = 0; i < count; i++)
        {
            float t = i * step;
            float x = semiMajorAxis * Mathf.Cos(t) + center.X;
            float y = semiMajorAxis * Mathf.Sin(t) + center.Y;
            points[i] = (new Vector2(x, y));
        }

        return points;
    }
}
