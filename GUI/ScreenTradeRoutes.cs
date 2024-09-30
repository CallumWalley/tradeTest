using Godot;
using System;
using System.Linq;

public partial class ScreenTradeRoutes : Control
{
	// Called when the node enters the scene tree for the first time.
	PlayerTrade playerTrade;
	Camera camera;

	Planet Earth;
	float count;

	public override void _Ready()
	{
		playerTrade = GetNode<PlayerTrade>("/root/Global/Player/PlayerTrade");
		camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
		Earth = GetNode<Planet>("/root/Global/Map/Galaxy/Sol/Earth/Earth");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print(playerTrade.First<TradeRoute>().Head.GetCanvasTransform());
	//	GD.Print(GetLocalMousePosition());
	//	GD.Print(camera.GetViewportTransform() );
		count += 1f;
		QueueRedraw();
	}
	public override void _Draw(){
		// foreach (TradeRoute tr in playerTrade)
		// {	
		// 	//DrawLine(tr.Head.GlobalPosition * gt, tr.Tail.GlobalPosition * gt, new Color(1,1,1,1), 100, true);
		//DrawCircle((Earth.GetViewportTransform() * Earth.GlobalPosition), 10, new Color(1,1,1,1));

		// }
	}
}
