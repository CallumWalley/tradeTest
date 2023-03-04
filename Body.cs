using Godot;
using System;

public class Body : Node2D
{
	[Export]
	public int nPoints = 32;
	[Export]
	public float radius = 10;
	[Export]
	public Color color = new Color(1, 0, 0);

	[Export]
	public bool hasTradeSource = false;
	
	[Export]
	public bool hasTradeReceiver = false;
	ResourcePool rp;
    static readonly PackedScene tradeSource = (PackedScene)GD.Load<PackedScene>("res://templates/TradeSource.tscn");
	static readonly PackedScene tradeReceiver = (PackedScene)GD.Load<PackedScene>("res://templates/TradeReceiver.tscn");
	static readonly PackedScene resourcePool = (PackedScene)GD.Load<PackedScene>("res://templates/ResourcePool.tscn");


	public override void _Ready(){
		rp = GetNodeOrNull<ResourcePool>("ResourcePool");
		if (rp == null && (hasTradeSource || hasTradeReceiver)){
			rp = resourcePool.Instance<ResourcePool>();
			AddChild(rp);
		}
		if (hasTradeSource){
			TradeSource ts = tradeSource.Instance<TradeSource>();
			AddChild(ts);
			ts.Init(rp);
		}
		if (hasTradeReceiver){
			TradeReceiver tr = tradeReceiver.Instance<TradeReceiver>();
			AddChild(tr);
			tr.Init(rp);
		}
	}
	
	
	public override void _Draw()
	{
		DrawCircleArcPoly(nPoints, radius, color);
	}

	public void DrawCircleArcPoly(int nPoints, float radius, Color color)
	{
		var pointsArc = new Vector2[nPoints + 1];
		var colors = new Color[] { color };

		for (int i = 0; i < nPoints; ++i)
		{
			float anglePoint = i * ( (float)Math.PI * 2f / nPoints );
			pointsArc[i] = new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
			//GD.Print(pointsArc[i]);
		}
		pointsArc[nPoints] =  pointsArc[0];
		DrawPolygon(pointsArc, colors);
	}

	public void DrawEllipseLineArc(int nPoints, float radius, Color color){

	}

}
