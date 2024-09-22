using Godot;
using System;
using System.Collections.Generic;

public partial class UITradeRouteFull : Control, Lists.IListable<TradeRoute>
{
    public TradeRoute tradeRoute;

    public bool Destroy { get; set; }

    // Components
    LineEdit labelName;
    UIDomainTiny DomainHead;
    UIDomainTiny DomainTail;
    UIResource friegherRequirement;
    public TextureButton cancelButton;
    ScrollContainer details;
    UIListResources toHead;
    UIListResources toTail;


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
        // UIResource uir = GetNode<UIResource>("DetailContent/Domain");
        // uir.Init(tradeRoute.Balance);
    }

    // HBoxContainer BalancePanel()
    // {
    //     HBoxContainer hbox = new();
    //     UIDomainSmall head = prefab_UIPanelDomainSmall.Instantiate<UIDomainSmall>();
    //     UIDomainSmall tail = prefab_UIPanelDomainSmall.Instantiate<UIDomainSmall>();
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

        DomainHead = GetNode<UIDomainTiny>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/Head/DomainSummary");
        DomainTail = GetNode<UIDomainTiny>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/Tail/DomainSummary");
        friegherRequirement = GetNode<UIResource>("VBoxContainer/HBoxContainer/HSplitContainer/UIResource");
        toHead = GetNode<UIListResources>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/toHead");
        toTail = GetNode<UIListResources>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/toTail");


        DomainHead.Init(tradeRoute.Head);
        DomainTail.Init(tradeRoute.Tail);

        toHead.Init(tradeRoute.ListHead);
        toTail.Init(tradeRoute.ListTail);

        friegherRequirement.Init(tradeRoute.ShipDemand);
        friegherRequirement.ShowBreakdown = true;
        // toHead.ShowDetails = true;
        // toTail.ShowDetails = true;

        // demand.Vertical = false;
        // surplus.Vertical = false;
        // demand.ShowBreakdown = true;
        // surplus.ShowBreakdown = true;

        // GetNode<HBoxContainer>("VBoxContainer/HBoxContainer/HSplitContainer/GridContainer/TailResources").AddChild(surplus);

        details = GetNode<ScrollContainer>("VBoxContainer/HBoxContainer/HSplitContainer/Details");
        cancelButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer/AlignRight/Cancel");
        // Link components
        labelName.Connect("text_submitted", new Callable(this, "ChangeName"));
        cancelButton.Connect("pressed", new Callable(this, "Remove"));

        Update();
    }

    public void ChangeName(string newName)
    {
        tradeRoute.ChangeName(newName);
    }
    public override void _Draw()
    {
        if (Destroy || tradeRoute == null)
        {
            Visible = false;
            GD.Print($"{this} QueueFree");
            QueueFree();
            return;
        }
        // details.GetNode<Label>("VBoxContainer/Distance").Text = String.Format("Distance {0:N2}", tradeRoute.distance);
        // details.GetNode<Label>("VBoxContainer/Time").Text = String.Format("Distance {0:N2}", tradeRoute.distance);
        labelName.Text = tradeRoute.Name;
    }

    public void Update()
    {
        if (!Destroy || tradeRoute != null)
        {
            toHead.Update();
            toTail.Update();
            _Draw();
        }
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
