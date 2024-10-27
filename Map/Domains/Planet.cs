using Godot;
using System;
namespace Game;

public partial class Planet : Domain, Entities.IOrbital
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
    public float SemiMajorAxis { get; set; }
    [Export]
    public float Eccentricity { get; set; }
    [Export]
    public float Period { get; set; }

    public float drawRadius = 100;


    [ExportGroup("Economic")]
    [Export]
    public bool HasLaunchComplex { get; set; }

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
        // if (HasEconomy)
        // {
        //     player.trade.RegisterTradeRoute(GetParent<SatelliteSystem>(), this);
        // }
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
            float place = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? Aphelion :
            (float)Math.Log(Aphelion, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
            place *= 0.1f; // Modify position of moons to fit on screen
            Position = new Vector2(0, place);
        }
        QueueRedraw();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
}
