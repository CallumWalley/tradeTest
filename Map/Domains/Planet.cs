using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
namespace Game;

public partial class Planet : Domain, Entities.IPosition
{
    [ExportGroup("Physical")]
    [Export]
    public float mass;
    [Export]
    public Color albedo { get; set; } = new();
    [Export]
    public float equatorialRadius; // in Mm

    [ExportGroup("Orbital")]
    [Export]
    public float SemiMajorAxis { get; set; }
    [Export]
    public float Anomaly { get; set; }

    [Export]
    public float Eccentricity { get; set; }
    [Export]
    public float Period { get; set; }

    public float drawRadius = 100;


    [ExportGroup("Economic")]
    [Export]
    public bool HasLaunchComplex { get; set; }

    RandomNumberGenerator rng;
    public Entities.IDomain Domain
    {
        get { return this; }
    }

    public Entities.IFeature this[int index]
    {
        get
        {
            return (Entities.IFeature)GetChild(index);
        }
    }
    public override float CameraZoom
    {
        get
        {
            return GetViewportRect().Size[0] / (4 * drawRadius);
        }
    }
    new public Vector2 CameraPosition
    {
        get
        {
            return GlobalPosition;
            // size = this.Max<Entities.IPosition>(x => { return ((Node2D)x).Position.DistanceTo(Position); });
            // return this.Average<Entities.IPosition>(x => { return ((Node2D)x).Position; }); ;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        rng = new();
        if (HasEconomy)
        {
            UplineTraderoute = player.trade.RegisterTradeRoute(GetParent<SatelliteSystem>(), this);
        }
    }

    public override IEnumerable<Entities.IFeature> Features
    {
        get
        {
            foreach (Entities.IFeature f in GetChildren())
            {
                yield return f;
            }
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerator<Entities.IFeature> GetEnumerator()
    {
        foreach (Entities.IFeature f in Features)
        {
            yield return f;
        }
    }
    public override void _Draw()
    {

        drawRadius = (((float)PlayerConfig.config.GetValue("interface", "radialLogBase")) < 2) ? equatorialRadius :
        (float)Math.Log(equatorialRadius, (float)PlayerConfig.config.GetValue("interface", "radialLogBase"));
        drawRadius *= (float)PlayerConfig.config.GetValue("interface", "radialScale") / 1000;
        DrawCircle(Vector2.Zero, drawRadius, albedo);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (SemiMajorAxis > 0)
        {
            float AphelionMod = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? SemiMajorAxis :
            (float)Math.Log(SemiMajorAxis, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
            // would be better if this was just eccentricity.
            Position = CalculatePosition(AphelionMod, AphelionMod, Anomaly);
        }
        QueueRedraw();
    }
    public void AddFeature(Entities.IFeature f)
    {
        AddChild((Node)f);
    }
    public void RemoveFeature(Entities.IFeature f)
    {
        RemoveChild((Node)f);
    }
    public static Vector2 CalculatePosition(float semiMajorAxis, float semiMinorAxis, float anomaly)
    {
        float x = semiMajorAxis * Mathf.Cos(anomaly);
        float y = semiMinorAxis * Mathf.Sin(anomaly);

        return new Vector2(x, y);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
}
