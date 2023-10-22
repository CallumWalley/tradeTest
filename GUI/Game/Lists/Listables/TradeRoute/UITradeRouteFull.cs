using Godot;
using System;
using System.Collections.Generic;

public partial class UITradeRouteFull : Control, UIList<TradeRoute>.IListable<TradeRoute>
{
    public TradeRoute tradeRoute;

    public bool Destroy { get; set; }

    // Components
    LineEdit labelName;
    UIInstallationTiny installationHead;
    UIInstallationTiny installationTail;
    UIListResources resourcesHead;
    UIListResources resourcesTail;
    UIResource friegherRequirement;
    public TextureButton cancelButton;
    ScrollContainer details;

    public static Player player;
    public Control Control { get { return this; } }
    public TradeRoute GameElement { get { return tradeRoute; } }


    public void Init(TradeRoute _tradeRoute)
    {
        tradeRoute = _tradeRoute;

        //callback = _callback;


        // Set button text.
        // GetNode<Label>("Summary/SummaryContent/Source").Text = $"→ system - {tradeRoute.destination.Name}";

        // Set reorder buttons.

        //vbox.AddChild(BalancePanel());

        // Init resource pool display.
        // UIResource uir = GetNode<UIResource>("DetailContent/Installation");
        // uir.Init(tradeRoute.Balance);
    }

    // HBoxContainer BalancePanel()
    // {
    //     HBoxContainer hbox = new();
    //     UIInstallationSmall head = prefab_UIPanelInstallationSmall.Instantiate<UIInstallationSmall>();
    //     UIInstallationSmall tail = prefab_UIPanelInstallationSmall.Instantiate<UIInstallationSmall>();
    //     VBoxContainer vbox = new();

    //     // just bance for now pending trade rework.

    //     UIResourceList import = new();
    //     //UIResourceList export = new();

    //     vbox.AddChild(import);
    //     //vbox.AddChild(export);


    //     head.Init(tradeRoute.Head);
    //     tail.Init(tradeRoute.Tail);

    //     import.Init(tradeRoute.Balance);

    //     // hbox.AddChild(head);
    //     // hbox.AddChild(vbox);
    //     // hbox.AddChild(tail);

    //     return hbox;
    // }
    public override void _Ready()
    {
        base._Ready();

        player = GetNode<Player>("/root/Global/Player");


        labelName = GetNode<LineEdit>("VBoxContainer/Panel/LineEdit");
        labelName.Text = tradeRoute.Name;

        installationHead = GetNode<UIInstallationTiny>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/Head/InstallationSummary");
        installationTail = GetNode<UIInstallationTiny>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/Tail/InstallationSummary");
        installationHead.Init(tradeRoute.Head);
        installationTail.Init(tradeRoute.Tail);

        UIListResources headDelta = new();
        UIListResources tailDelta = new();

        headDelta.Init(tradeRoute.Head.delta);
        tailDelta.Init(tradeRoute.Tail.delta);

        headDelta.Vertical = false;
        tailDelta.Vertical = false;

        GetNode<HBoxContainer>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/HeadResources").AddChild(headDelta);
        GetNode<HBoxContainer>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/TailResources").AddChild(tailDelta);

        friegherRequirement = GetNode<UIResource>("VBoxContainer/HBoxContainer/HSplitContainer/UIResource");
        friegherRequirement.Init(tradeRoute.TradeWeight, false, false, true);
        details = GetNode<ScrollContainer>("VBoxContainer/HBoxContainer/HSplitContainer/Details");
        cancelButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer/AlignRight/Cancel");
        // Link components
        labelName.Connect("text_submitted", new Callable(this, "ChangeName"));
        cancelButton.Connect("pressed", new Callable(this, "Remove"));
    }

    public void ChangeName(string newName)
    {
        tradeRoute.ChangeName(newName);
    }
    public override void _Draw()
    {
        if (tradeRoute == null) { return; }
        if (Destroy)
        {
            QueueFree();
        }
        details.GetNode<Label>("VBoxContainer/Distance").Text = String.Format("Distance {0:N2}", tradeRoute.distance);
        details.GetNode<Label>("VBoxContainer/Time").Text = String.Format("Distance {0:N2}", tradeRoute.distance);
        labelName.Text = tradeRoute.Name;
        //int index = tradeRoute.destination.GetIndustryTrade().tradeRoutes.IndexOf(tradeRoute);
    }

    public void Remove()
    {
        player.trade.DeregisterTradeRoute(tradeRoute);
        Control parent = GetParent<Control>();
        parent.RemoveChild(this);
        parent.QueueRedraw();
        QueueFree();
    }
}
