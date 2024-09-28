using Godot;
using System;

public partial class Screen : CanvasLayer
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Ready()
    {
        base._Ready();
    }

    

	public void DrawSystem(PlanetarySystem ps){
		foreach (SatelliteSystem ss in ps){
			ss.Eldest.OverlayElement.Visible = true;
			ss.Eldest.OverlayElement._Draw();
		}
	}
}
