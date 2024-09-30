using Godot;
using System;

public partial class Screen : CanvasLayer
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	Control tradeRoutes;
	Control overlay;

    public override void _Ready()
    {
        base._Ready();
		overlay = GetNode<Control>("PlanetOverlay");
    }

    

	public void DrawSystem(PlanetarySystem ps){
		foreach (Entities.IOrbital ss in ps){
			UIMapOverlayElement nuimoe = new UIMapOverlayElement();
			nuimoe.element = ss;
			overlay.AddChild(nuimoe);
			nuimoe.Visible = true;
			//ss.Eldest.OverlayElement._Draw();
		}
		GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D").Center(ps);
	}

}
