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
	public bool hasResourcePool = false;
	
	[Export]
	public bool hasTradeReceiver = false;
	
	[Export]

	public ResourcePool resourcePool;

	public UIBody uiBody;

	static readonly PackedScene p_tradePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UITradePanel.tscn");
	static readonly PackedScene p_resourcePool = (PackedScene)GD.Load<PackedScene>("res://Map/ResourcePool.tscn");

	static readonly PackedScene p_uiBody = (PackedScene)GD.Load<PackedScene>("res://GUI/UIBody.tscn");

	bool focus = false;
	public override void _Ready(){
		uiBody = p_uiBody.Instance<UIBody>();
		AddChild(uiBody);
		if (hasResourcePool || hasTradeReceiver){
			AddResourcePool();
			UITradePanel tp = p_tradePanel.Instance<UITradePanel>();

			tp.Init(this);
			uiBody.AddChild(tp);

		}
		GetNode("Area2D").Connect("mouse_entered", this, "Focus");
		GetNode("Area2D").Connect("mouse_exited", this, "UnFocus");

	}

	public override void _Process(float _delta)
	{
		if (Input.IsActionPressed("ui_select")){
			if (focus){
				uiBody.Visible = true;
				uiBody.Raise();
			}
		}
		//  && focus)
		// {
		// 	uiBody.Visible = true;
		// }
	}
	
	
	public void Focus(){
		focus = true;
	}

	public void UnFocus(){
		focus = false;
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

	public ResourcePool AddResourcePool(){
		resourcePool = GetNodeOrNull<ResourcePool>("ResourcePool");
		if (resourcePool==null){
			resourcePool = p_resourcePool.Instance<ResourcePool>();
			if (hasTradeReceiver){
				resourcePool.isValidTradeReceiver=true;
			}
			AddChild(resourcePool);
		}
		return resourcePool;
	}

	// public TradeReceiver AddTradeReceiver(){
	// 	tradeReceiver = GetNodeOrNull<TradeReceiver>("ResourcePool");
	// 	if (tradeReceiver==null){
	// 		tradeReceiver = p_tradeReceiver.Instance<TradeReceiver>();
	// 		tradeReceiver.Init(resourcePool);
	// 		AddChild(tradeReceiver);
	// 	}
	// 	return tradeReceiver;
	// }

}
