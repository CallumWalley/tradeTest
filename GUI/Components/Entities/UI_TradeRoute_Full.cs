using Godot;
using System;
using System.Collections.Generic;

public partial class UI_TradeRoute_Full : Control, UIContainers.IListable
{
    public TradeRoute tradeRoute;
    static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");
    static readonly PackedScene p_uiInstallation_Small = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Installation_Small.tscn");

    public static PlayerTradeRoutes playerTradeRoutes;
    public Control Control { get { return this; } }
    public System.Object GameElement { get { return tradeRoute; } }

    Label labelDistance;

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

        VBoxContainer vbox = GetNode<VBoxContainer>("VBoxContainer");

        labelDistance = GetNode<Label>("VBoxContainer/Info/Distance");
        // Set button text.
        // GetNode<Label>("Summary/SummaryContent/Source").Text = $"→ system - {tradeRoute.destination.Name}";
        UIResource freighterIcon = GetNode<UIResource>("VBoxContainer/Name/SummaryContent/Freighters");
        freighterIcon.Init(tradeRoute.tradeWeight);

        // Set reorder buttons.
        GetNode<TextureButton>("VBoxContainer/Name/AlignRight/Cancel/").Connect("pressed", new Callable(this, "Remove"));

        vbox.AddChild(BalancePanel());

        // Init resource pool display.
        // UIResource uir = GetNode<UIResource>("DetailContent/Installation");
        // uir.Init(tradeRoute.Balance);
    }

    HBoxContainer BalancePanel()
    {
        HBoxContainer hbox = new();
        UI_Installation_Small head = p_uiInstallation_Small.Instantiate<UI_Installation_Small>();
        UI_Installation_Small tail = p_uiInstallation_Small.Instantiate<UI_Installation_Small>();
        VBoxContainer vbox = new();

        // just bance for now pending trade rework.

        UIResourceList import = new();
        //UIResourceList export = new();

        vbox.AddChild(import);
        //vbox.AddChild(export);


        head.Init(tradeRoute.Head);
        tail.Init(tradeRoute.Tail);

        import.Init(tradeRoute.Balance);

        hbox.AddChild(head);
        hbox.AddChild(vbox);
        hbox.AddChild(tail);

        return hbox;
    }
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Draw()
    {
        if (tradeRoute == null) { return; }
        labelDistance.Text = String.Format("Distance {0:N2}", tradeRoute.distance);
        //int index = tradeRoute.destination.GetIndustryTrade().tradeRoutes.IndexOf(tradeRoute);
    }

    public void Remove()
    {
        GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(tradeRoute);
        Control parent = GetParent<Control>();
        parent.RemoveChild(this);
        parent.QueueRedraw();
        QueueFree();
    }
}
