using Godot;
using System;
using System.Linq;
namespace Game;

public partial class ScreenTradeRoutes : Control
{
	// Called when the node enters the scene tree for the first time.
	PlayerTrade playerTrade;
	Camera camera;

	float count;

	Godot.Color lineColor = new Godot.Color(1, 1, 1, 1);
	public override void _Ready()
	{
		playerTrade = GetNode<PlayerTrade>("/root/Global/Player/PlayerTrade");
		camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print(playerTrade.First<TradeRoute>().Head.GetCanvasTransform());
		//	GD.Print(GetLocalMousePosition());
		//	GD.Print(camera.GetViewportTransform() );
		QueueRedraw();
	}
	public override void _Draw()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}
		foreach (TradeRoute tr in playerTrade)
		{
			//DrawLine(tr.Head.GlobalPosition * gt, tr.Tail.GlobalPosition * gt, new Color(1,1,1,1), 100, true);
			//DrawLine(((Node2D)tr.Head).GetViewportTransform() * ((Node2D)tr.Head).GlobalPosition, ((Node2D)tr.Tail).GetViewportTransform() * ((Node2D)tr.Tail).GlobalPosition, lineColor, 1, true);
			Vector2 apparentHead = ((Node2D)tr.Head).GetCanvasTransform() * ((Node2D)tr.Head).Position;
			Vector2 apparentTail = ((Node2D)tr.Tail).GetCanvasTransform() * ((Node2D)tr.Tail).Position;

			Godot.Color[] colors = new Godot.Color[100];

			for (int i = 0; i < 100; i++)
			{
				colors[i] = lineColor;

			}
			Line2D l2d = new Line2D();
			float seperation = Mathf.Abs(apparentHead.X - apparentTail.X);
			l2d.Points = MakeEllipse((apparentHead + apparentTail) / 2, (seperation * 0.6f), seperation * 0.3f, 100);
			//((Node2D)tr.Head).GetViewportTransform() * ((Node2D)tr.Head).GlobalPosition
			l2d.Width = 1;
			l2d.Antialiased = true;
			l2d.Visible = true;
			AddChild(l2d);
			//DrawLine(((Node2D)tr.Head).GetViewportTransform() * ((Node2D)tr.Head).GlobalPosition, ((Node2D)tr.Tail).GetViewportTransform() * ((Node2D)tr.Tail).GlobalPosition, lineColor, 1, true);
		}
	}


	public Vector2[] MakeEllipse(Vector2 center, float semiMajorAxis, float semiMinorAxis, int count)
	{
		Vector2[] points = new Vector2[count];
		float step = 2 * Mathf.Pi / count;

		for (int i = 0; i < count; i++)
		{
			float t = i * step;
			float x = semiMajorAxis * Mathf.Cos(t) + center.X;
			float y = semiMinorAxis * Mathf.Sin(t) + center.Y;
			points[i] = (new Vector2(x, y));
		}

		return points;
	}
}
