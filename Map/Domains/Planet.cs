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
    public float Aphelion {get;set;}
    [Export]
    public float Perihelion {get;set;}
    [Export]
    public float SemiMajorAxis {get;set;}
    [Export]
    public float Eccentricity {get;set;}
    [Export]
    public float Period {get;set;}
	public override float CameraZoom{
		get{
			GD.Print("Single");

			return GetViewportRect().Size[0] / (4*((float)Math.Log(equatorialRadius, (int)PlayerConfig.config.GetValue("interface", "logBase")) ));}
		}

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
		DrawCircle(this.GlobalPosition, (float)Math.Log(equatorialRadius, (int)PlayerConfig.config.GetValue("interface", "logBase"))/2, albedo);
		GD.Print((float)Math.Log(equatorialRadius, (int)PlayerConfig.config.GetValue("interface", "logBase")));
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Aphelion > 0){
			Position = new Vector2((float)Math.Log(Aphelion * 1000000, (int)PlayerConfig.config.GetValue("interface", "logBase")), 0);
		}
    }

    
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
