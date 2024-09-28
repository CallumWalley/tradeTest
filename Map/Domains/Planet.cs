using Godot;
using System;

public partial class Planet : Domain
{
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

	[ExportGroup("Physical")]
	[Export]
	double mass;
	[Export]
	public Color albedo { get; set; } = new();
	[Export]
	float equatorialRadius; // in Mm

	public override float ZoomLevel {get{return (float)Math.Log(equatorialRadius, (int)PlayerConfig.config.GetValue("interface", "logBase"));}}

    public override void _Ready()
    {
        base._Ready();
		// // UI Stuff
        // CircleShape2D circleShape2D = new CircleShape2D();
        // circleShape2D.Radius = UIRadius;
        // collisionShape2D = new();
        // collisionShape2D.Shape = circleShape2D;
        // // Add interactive 
        // Area2D area2D = new();
        // area2D.AddChild(collisionShape2D);

        // area2D.Connect("mouse_entered", new Callable(this, "Focus"));
        // area2D.Connect("mouse_exited", new Callable(this, "UnFocus"));
        // AddChild(area2D);
    }

	public override void _Draw(){
		base._Draw();
		DrawCircle(this.Position, ZoomLevel, albedo);
	}
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
