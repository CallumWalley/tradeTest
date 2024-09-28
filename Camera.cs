using Godot;
using System;

public partial class Camera : Godot.Camera2D
{
	float previousZoom;
	float nextZoom;

	float zoomSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		previousZoom = Zoom[0];
		nextZoom = previousZoom;
		zoomSpeed = (float)PlayerConfig.config.GetValue("interface", "cameraZoomSpeed");
		PositionSmoothingSpeed = (float)PlayerConfig.config.GetValue("interface", "cameraPanSpeed");
	}	

	public void SetZoom(float zoom){
		previousZoom = nextZoom;
		nextZoom = zoom;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		float zoomterpolate =  Zoom[0] + ((nextZoom - Zoom[0]) * (float)delta * zoomSpeed * 2);
		// GD.Print($"Previous Zoom: {previousZoom} Current Zoom {Zoom[0]}, Next Zoom: {nextZoom} Interpolate:{zoomterpolate}");
		
		Zoom = new Vector2(zoomterpolate, zoomterpolate);
    }
}