using Godot;
using System;
using System.Collections.Generic;

public partial class Body : Node2D
{
    [Export]
    public int nPoints = 32;
    [Export]
    public float radius = 10;
    [Export]
    public Color color = new Color(1, 0, 0);

    public UIWindowBody uiBody;

    public IEnumerable<Installation> Installations { get { return GetInstallations(); } }
    static readonly PackedScene p_uiBody = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Windows/UIWindowBody.tscn");

    bool focus = false;

    public partial class Designations
    {
        string name;
        List<String> altNames;
        string adjective;
    }

    public partial class Orbital
    {
        double aphelion;
        double perihelion;
        double semiMajorAxis;
        double eccentricity;
        double period;
        double inclination;
    }
    // Satellites:
    public partial class Physical
    {
        double circumference;
        Dictionary<string, double> surfaceArea;
        double mass;
        double meanDensity;
        double escapeVelocity;
        double rotationPeriod;
        double axialTilt;
        double albedo;
        double[] surfaceTemp;
    }

    public partial class Atmosphere
    {
        double surfacePressure;
        Dictionary<string, double> composition;
    }
    public override void _Ready()
    {
        /// UI
        // Main UI element.
        // uiBody = p_uiBody.Instantiate<UIBody>();
        // uiBody.Init(this);
        // AddChild(uiBody);

        // Add interactive 
        GetNode("Area2D").Connect("mouse_entered", new Callable(this, "Focus"));
        GetNode("Area2D").Connect("mouse_exited", new Callable(this, "UnFocus"));
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
        DrawCircleArcPoly(nPoints, radius, color);
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
