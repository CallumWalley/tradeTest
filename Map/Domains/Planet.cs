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
    public float Aphelion { get; set; }
    [Export]
    public float Perihelion { get; set; }

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

    public override void _Ready()
    {
        base._Ready();
        RandomNumberGenerator rng = new();
        // if (HasEconomy)
        // {
        //     player.trade.RegisterTradeRoute(GetParent<SatelliteSystem>(), this);
        // }
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
        if (Aphelion > 0)
        {
            float AphelionMod = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? Aphelion :
            (float)Math.Log(Aphelion, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
            // would be better if this was just eccentricity.
            float PerihelionnMod = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? Perihelion :
            (float)Math.Log(Perihelion, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
            Position = CalculatePosition(AphelionMod, PerihelionnMod, rng.RandfRange(0, 2* Mathf.Pi));
        }
        QueueRedraw();
    }
    public Entities.IFeature AddFeature(PlayerFeatureTemplate template, StringName name)
    {
        FeatureBase newFeature = template.Instantiate();
        newFeature.Template = template;
        newFeature.Name = name;

        // If has size. Set size to zero.
        if (newFeature.FactorsSingle.ContainsKey(901))
        {
            newFeature.FactorsSingle[901].Sum = 0;
        }
        AddChild(newFeature);
        return newFeature;
    }
    public static Vector2 CalculatePosition(float semiMajorAxis, float semiMinorAxis, float anomaly)
    {
        float x = semiMajorAxis * Mathf.Cos(anomaly);
        float y = semiMinorAxis * Mathf.Sin(anomaly);
        
        return new Vector2(x, y);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
}
