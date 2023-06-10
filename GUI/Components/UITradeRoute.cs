using Godot;
using System;
using System.Collections.Generic;

public partial class UITradeRoute : Control, UIContainers.IListable
{
    public TradeRoute tradeRoute;
    static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIResource.tscn");

    public static PlayerTradeRoutes playerTradeRoutes;
    public Control Control { get { return this; } }
    public System.Object GameElement { get { return tradeRoute; } }
    TextureButton moveUpButton;
    TextureButton moveDownButton;

    // Element to update on change.
    Control callback;
    public virtual void Init(System.Object gameObject)
    {
        Init((TradeRoute)gameObject);
    }
    public void Init(TradeRoute _tradeRoute)
    {
        tradeRoute = _tradeRoute;
        //callback = _callback;

        // Add details panel.

        GetNode("Summary").Connect("toggled", new Callable(this, "ShowDetails"));

        // Set button text.
        GetNode<Label>("Summary/SummaryContent/Source").Text = $"→ system - {tradeRoute.destination.Name}";
        UIResource freighterIcon = GetNode<UIResource>("Summary/SummaryContent/Freighters");
        freighterIcon.Init(tradeRoute.tradeWeight);

        // Set reorder buttons.
        GetNode<TextureButton>("Summary/AlignRight/Cancel/").Connect("pressed", new Callable(this, "Remove"));

        // Init resource pool display.
        // UIResource uir = GetNode<UIResource>("DetailContent/Installation");
        // uir.Init(tradeRoute.Balance);
    }
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Draw()
    {
        if (tradeRoute == null) { return; }
        //int index = tradeRoute.destination.GetIndustryTrade().tradeRoutes.IndexOf(tradeRoute);
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
