using Godot;
using System;
using System.Collections.Generic;


[Tool]
public partial class Body : Node2D
{
    CollisionShape2D collisionShape2D;
    int spriteNPoints; // For drawing sprite.
    int spriteRadius;

    public IEnumerable<Installation> Installations { get { return GetInstallations(); } }
    static readonly PackedScene p_uiBody = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Windows/UIWindowBody.tscn");

    UIWindowBody uiBody;
    bool focus = false;

    [ExportGroup("Designations")]
    [Export]
    Godot.Collections.Array altNames;

    [Export]
    string adjective;

    [ExportGroup("Orbital")]
    [Export]
    double aphelion;
    [Export]
    double perihelion;
    [Export]
    double semiMajorAxis;
    [Export]
    double eccentricity;
    [Export]
    double period;
    [Export]
    double inclination;
    [ExportGroup("Physical")]
    [Export]
    double equatorialRadius = 6.378; //MM


    // Dictionary<string, double> surfaceArea;
    // double mass;
    // double meanDensity;
    // double escapeVelocity;
    // double rotationPeriod;
    // double axialTilt;
    // double albedo;
    // double[] surfaceTemp;
    // double circumference
    // {
    //     get { return equatorialRadius * 2 * Math.PI; }
    // }

    [ExportGroup("Atmosphere")]
    [Export]
    double surfacePressure;
    [Export]
    Color color;
    // [Export]
    // Dictionary<string, double> composition;
    public override void _Ready()
    {
        /// UI
        // Main UI element.
        // uiBody = p_uiBody.Instantiate<UIBody>();
        // uiBody.Init(this);
        // AddChild(uiBody);
        spriteNPoints = 12 + (int)(equatorialRadius / 5);
        spriteRadius = (int)equatorialRadius;
        CircleShape2D circleShape2D = new CircleShape2D();
        circleShape2D.Radius = (float)equatorialRadius + 10;
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
                if (uiBody == null)
                {
                    uiBody = p_uiBody.Instantiate<UIWindowBody>();
                    uiBody.Init(this);
                    AddChild(uiBody);
                }
                uiBody.Visible = true;
                uiBody.MoveToForeground();
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
        DrawCircleArcPoly(spriteNPoints, spriteRadius, color);
    }

    public void DrawCircleArcPoly(int nPoints, float radius, Color color)
    {
        var pointsArc = new Vector2[nPoints + 1];
        var colors = new Color[] { color };

        for (int i = 0; i < nPoints; ++i)
        {
            float anglePoint = i * ((float)Math.PI * 2f / nPoints);
            pointsArc[i] = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
            //GD.Print(pointsArc[i]);
        }
        pointsArc[nPoints] = pointsArc[0];
        DrawPolygon(pointsArc, colors);
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

    public IEnumerable<Installation> GetInstallations()
    {
        foreach (Node c in GetChildren())
        {
            if (c is Installation)
            {
                yield return (Installation)c;
            }
        }
    }

}
