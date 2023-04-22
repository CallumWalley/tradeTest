using Godot;
using System;
using System.Collections.Generic;

public class Body : Node2D
{
    [Export]
    public int nPoints = 32;
    [Export]
    public float radius = 10;
    [Export]
    public Color color = new Color(1, 0, 0);

    public UIBodyCard uiBody;

    public IEnumerable<Installation> Installations { get { return GetInstallations(); } }
    static readonly PackedScene p_uiBody = (PackedScene)GD.Load<PackedScene>("res://GUI/Cards/UIBodyCard.tscn");

    bool focus = false;

    public class Designations
    {
        string name;
        List<String> altNames;
        string adjective;
    }

    public class Orbital
    {
        float aphelion;
        float perihelion;
        float semiMajorAxis;
        float eccentricity;
        float period;
        float inclination;
    }
    // Satellites:
    public class Physical
    {
        float circumference;
        Dictionary<string, float> surfaceArea;
        float mass;
        float meanDensity;
        float escapeVelocity;
        float rotationPeriod;
        float axialTilt;
        float albedo;
        float[] surfaceTemp;
    }

    public class Atmosphere
    {
        float surfacePressure;
        Dictionary<string, float> composition;
    }
    public override void _Ready()
    {
        /// UI
        // Main UI element.
        // uiBody = p_uiBody.Instance<UIBody>();
        // uiBody.Init(this);
        // AddChild(uiBody);

        // Add interactive 
        GetNode("Area2D").Connect("mouse_entered", this, "Focus");
        GetNode("Area2D").Connect("mouse_exited", this, "UnFocus");
    }

    public override void _Process(float _delta)
    {
        if (Input.IsActionPressed("ui_select"))
        {
            if (focus)
            {
                if (uiBody == null)
                {
                    uiBody = p_uiBody.Instance<UIBodyCard>();
                    uiBody.Init(this);
                    AddChild(uiBody);
                }
                uiBody.Visible = true;
                uiBody.Raise();
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

    public void DrawEllipseLineArc(int nPoints, float radius, Color color)
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
