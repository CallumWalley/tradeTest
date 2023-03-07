using Godot;
using System;

public class TradeSource : Resource
{	
	static readonly PackedScene tradeSource = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/UITradeSource.tscn");
	static readonly PackedScene p_resourcePool = (PackedScene)GD.Load<PackedScene>("res://templates/ResourcePool.tscn");
	public float shipWeight = 0;
	public TradeRoute tradeRoute;
	public ResourcePool resourcePool;

	// public override void _Ready(){
	// 	Init();
	// }
	// Pass forward 'position'
	public Vector2 Position {
		get { return GetParent<Body>().Position; }
	}
	public override void _Ready(){
		base._Ready();
		// If parent doesn't have a resrouce pool. add one.
		resourcePool = GetNodeOrNull<ResourcePool>("../ResourcePool");
		if (resourcePool==null){
			resourcePool = p_resourcePool.Instance<ResourcePool>();
			GetParent().AddChild(resourcePool);
		}
		Control uiParent = (Control)GetNode("../InfoCard");
		UITradeSource ui = tradeSource.Instance<UITradeSource>();
		ui.Init(this);
		uiParent.AddChild(ui);
	}
	
	// public override void _Draw(){
	// 	if (tl.GetChildCount()<1){tl.Visible=false;}
	// }

	public override void EconomyFrame(){
		
		shipWeight=GetShipWeight();
	}

	public float GetShipWeight(){
		float shipWeightImport = 0;
		float shipWeightExport = 0;
		foreach (Resource child in resourcePool.GetChildren()){
			if (child.Sum > 0){
				shipWeightExport += child.Sum * Resources.ShipWeight(child.Type);
			} else {
				shipWeightImport += child.Sum * Resources.ShipWeight(child.Type);
			}
		}
		return Math.Max(shipWeightExport, shipWeightImport);
	}
}
