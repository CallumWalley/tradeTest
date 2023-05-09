using Godot;
using System;
using System.Collections.Generic;

public partial class UITradeRoute : Control
{
    public TradeRoute tradeRoute;
    static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIResource.tscn");

    public static PlayerTradeRoutes playerTradeRoutes;

    TextureButton moveUpButton;
    TextureButton moveDownButton;

    // Element to update on change.
    Control callback;

    public void Init(TradeRoute _tradeRoute)
    {
        tradeRoute = _tradeRoute;
        //callback = _callback;

        // // Add details panel.
        // foreach (Resource r in tradeRoute.destination.GetStandard()){
        // 	UIResource ui = resourceIcon.Instantiate<UIResource>();
        // 	ui.Init(r);
        // 	GetNode("DetailContent").AddChild(ui);
        // }
        GetNode("Summary").Connect("toggled", new Callable(this, "ShowDetails"));

        // Set button text
        GetNode<Label>("Summary/SummaryContent/Source").Text = $"→ system - {tradeRoute.destination.Name}";
        UIResource freighterIcon = GetNode<UIResource>("Summary/SummaryContent/Freighters");
        freighterIcon.Init(tradeRoute.tradeWeight);

        // Set reorder buttons
        moveUpButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveUp");
        moveDownButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveDown");
        moveUpButton.Connect("pressed", new Callable(this, "ReorderUp"));
        moveDownButton.Connect("pressed", new Callable(this, "ReorderDown"));

        // Set reorder buttons
        GetNode<TextureButton>("Summary/AlignRight/Cancel/").Connect("pressed", new Callable(this, "Remove"));

        // Init resource pool display.
        UIResource uir = GetNode<UIResource>("DetailContent/Installation");
        uir.Init(tradeRoute.Balance);
    }
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Draw()
    {
        if (tradeRoute == null) { return; }
        //int index = tradeRoute.destination.GetTransformerTrade().tradeRoutes.IndexOf(tradeRoute);
        int index = GetIndex();
        moveUpButton.Disabled = false;
        moveDownButton.Disabled = false;
        if (index == 0)
        {
            moveUpButton.Disabled = true;
        }
        if (index == (GetParent().GetChildCount() - 1))
        {
            moveDownButton.Disabled = true;
        }
        // for
        // tradeRoute
    }

    public void ShowDetails(bool toggled)
    {
        GetNode<Control>("DetailContent").Visible = toggled;
    }
    public void Remove()
    {
        GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(tradeRoute);
        Control parent = GetParent<Control>();
        parent.RemoveChild(this);
        parent.QueueRedraw();
        QueueFree();
    }

    public void ReorderUp()
    {
        tradeRoute.destination.MoveChild(tradeRoute, tradeRoute.GetIndex() - 1);
        GetParent<Control>().QueueRedraw();
    }
    public void ReorderDown()
    {
        tradeRoute.destination.MoveChild(tradeRoute, tradeRoute.GetIndex() + 1);
        GetParent<Control>().QueueRedraw();
    }
}
