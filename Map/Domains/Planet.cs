using Godot;
using System;

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
    public override float CameraZoom
    {
        get
        {
            GD.Print("Single");
            if ((int)PlayerConfig.config.GetValue("interface", "logBase") <= 1)
            {
                return GetViewportRect().Size[0] / (4 * (equatorialRadius));
            }
            else
            {
                return GetViewportRect().Size[0] / (4 * ((float)Math.Log(equatorialRadius, (int)PlayerConfig.config.GetValue("interface", "logBase"))));
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Draw()
    {
        if ((int)PlayerConfig.config.GetValue("interface", "logBase") <= 1)
        {
            DrawCircle(Vector2.Zero, equatorialRadius / (float)PlayerConfig.config.GetValue("interface", "radialScale"), albedo);
        }
        else
        {
            DrawCircle(Vector2.Zero, (float)Math.Log(equatorialRadius, (float)PlayerConfig.config.GetValue("interface", "logBase")) / (float)PlayerConfig.config.GetValue("interface", "radialScale"), albedo);
        }
        GD.Print((float)Math.Log(equatorialRadius, (float)PlayerConfig.config.GetValue("interface", "logBase")));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Aphelion > 0)
        {
            if ((int)PlayerConfig.config.GetValue("interface", "logBase") <= 1)
            {
                Position = new Vector2(0, Aphelion * 1000000);
            }
            else
            {
                Position = new Vector2(0, (float)Math.Log(Aphelion * 1000000, (float)PlayerConfig.config.GetValue("interface", "logBase")));
            }
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
}
