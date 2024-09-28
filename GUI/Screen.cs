using Godot;
using System;

public partial class Screen : CanvasLayer
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Ready()
    {
        base._Ready();
    }

    

	public void DrawSystem(PlanetarySystem ps){
		// foreach (SatelliteSystem ss in ps){
		// 	ss.Eldest.OverlayElement.Visible = true;
		// 	//ss.Eldest.OverlayElement._Draw();
		// }
		GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D").Center(ps);

	}

}
