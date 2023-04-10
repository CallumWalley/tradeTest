using Godot;
using System;
using System.Collections.Generic;

public class UITransformerTrade : Control
{
	//static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIResource.tscn");

    // Game object this UI element follows.
    public Transformer transformer;
	public static PlayerTradeRoutes playerTradeRoutes;

	TextureButton moveUpButton;
	TextureButton moveDownButton;

	// Element to update on change.
	//Control callback;

	public void Init(Transformer _transformer){
		transformer = _transformer;

		GetNode("Summary").Connect("toggled", this, "ShowDetails");

		// Set button text
		GetNode<Label>("Summary/SummaryContent/Summary").Text = transformer.Name;

		// Set reorder buttons
		moveUpButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveUp");
		moveDownButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveDown");
		moveUpButton.Connect("pressed", this, "ReorderUp");
		moveDownButton.Connect("pressed", this, "ReorderDown");

        GetNode<Label>("DetailContent/VBoxContainer/Description").Text = transformer.Description;

		// Init resource pool display.
		UIResourceList uiCostUpkeep = GetNode<UIResourceList>("DetailContent/VBoxContainer/CostUpkeep");
        UIResourceList uiCostProduction = GetNode<UIResourceList>("DetailContent/VBoxContainer/CostProduction");
		UIResourceList uiCostOperation = GetNode<UIResourceList>("DetailContent/VBoxContainer/CostOperation");

		bool editable = (transformer is TransformerTrade);

        //uiCostUpkeep.Init(transformer.costUpkeep);
		//uiCostOperation.Init(transformer.costOperation);

        uiCostProduction.Init(transformer.costProduction, editable);
	}
	public override void _Ready(){
		base._Ready();
	}
	public override void _Draw()
	{	
		if (transformer == null){return;}
		//int index = tradeRoute.destination.GetTransformerTrade().tradeRoutes.IndexOf(tradeRoute);
		int index = GetIndex();
		moveUpButton.Disabled = false;
		moveDownButton.Disabled = false;
		if (index == 0){
			moveUpButton.Disabled = true;
		}
		if (index == (GetParent().GetChildCount()-1)){
			moveDownButton.Disabled = true;
		}
		// for
		// tradeRoute
	}

	public void ShowDetails(bool toggled){
		GetNode<Control>("DetailContent").Visible = toggled;
	}
	// public void Remove(){
	// 	GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(tradeRoute);
	// 	Control parent = GetParent<Control>();
	// 	parent.RemoveChild(this);
	// 	parent.Update();
	// 	QueueFree();
	// }

	public void ReorderUp(){
		transformer.GetParent<Installation>().MoveChild(transformer, transformer.GetIndex()-1);
		//FIXME
		GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
		GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = true;

	}
	public void ReorderDown(){
		Installation par = transformer.GetParent<Installation>();
		int chindex = transformer.GetIndex()+1;
		par.MoveChild(transformer, chindex);
		// FIXME
		GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
		GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = true;
	}
}
