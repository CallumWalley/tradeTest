using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class Zone : Node2D
{
    CollisionShape2D collisionShape2D;
    int clickRadius;

    public IEnumerable<Feature> Features { get { return GetFeatures(); } }
    static readonly PackedScene p_uiZone = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Windows/UIWindowZone.tscn");

    UIWindowZone uiZone;
    bool focus = false;

    [ExportGroup("Designations")]
    [Export]
    Godot.Collections.Array altNames;

    [Export]
    string adjective;

    // Dictionary<string, double> composition;
    public override void _Ready()
    {
        /// UI
        // Main UI element.
        // uiBody = p_uiBody.Instantiate<UIBody>();
        // uiBody.Init(this);
        // AddChild(uiBody);
        CircleShape2D circleShape2D = new CircleShape2D();
        circleShape2D.Radius = clickRadius;
        collisionShape2D = new();
        collisionShape2D.Shape = circleShape2D;
        // Add interactive 
        Area2D area2D = new();
        area2D.AddChild(collisionShape2D);

        area2D.Connect("mouse_entered", new Callable(this, "Focus"));
        area2D.Connect("mouse_exited", new Callable(this, "UnFocus"));
        AddChild(area2D);
    }

    public override void _Process(double _delta)
    {
        if (Input.IsActionPressed("ui_select"))
        {
            if (focus)
            {
                if (uiZone == null)
                {
                    uiZone = p_uiZone.Instantiate<UIWindowZone>();
                    uiZone.Init(this);
                    AddChild(uiZone);
                }
                uiZone.Visible = true;
                uiZone.MoveToForeground();
            }
        }
        //  && focus)
        // {
        // 	uiBody.Visible = true;
        // }
    }

    public void Focus()
    {
        focus = true;
    }

    public void UnFocus()
    {
        focus = false;
    }

    public override void _Draw()
    {
        DrawCircle(this.Position, clickRadius, new Color(1, 1, 1));
        // DrawCircleArcPoly(spriteNPoints, clickRadius, new Color(1, 1, 1));
    }

    public void DrawCircleArcPoly(int nPoints, float radius, Color color)
    {
        // var pointsArc = new Vector2[nPoints + 1];
        // var colors = new Color[] { color };

        // for (int i = 0; i < nPoints; ++i)
        // {
        //     float anglePoint = i * ((float)Math.PI * 2f / nPoints);
        //     pointsArc[i] = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
        //     //GD.Print(pointsArc[i]);
        // }
        // pointsArc[nPoints] = pointsArc[0];
    }
    // public void DrawCircleArcLine(int nPoints, double radius, Color color)
    // {
    //     var pointsArc = new Vector2[nPoints + 1];
    //     var colors = new Color[] { color };

    //     for (int i = 0; i < nPoints; ++i)
    //     {
    //         double anglePoint = i * ((double)Math.PI * 2f / nPoints);
    //         pointsArc[i] = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
    //         //GD.Print(pointsArc[i]);
    //     }
    //     pointsArc[nPoints] = pointsArc[0];
    //     DrawArc(pointsArc, colors);
    // }
    public void DrawEllipseLineArc(int nPoints, double radius, Color color)
    {

    }

    public IEnumerable<Feature> GetFeatures()
    {
        foreach (Node c in GetChildren())
        {
            if (c is Feature)
            {
                yield return (Feature)c;
            }
        }
    }
}
