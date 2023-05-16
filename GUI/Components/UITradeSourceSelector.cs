using Godot;
using System;
using System.Collections.Generic;

public partial class UITradeSourceSelector : Control
{
    PlayerTradeReciever globalTrade;
    PlayerTradeRoutes playerTradeRoutes;
    static readonly Texture2D freighterIcon = GD.Load<Texture2D>("res://assets/icons/freighter.png");


    // Index of trade route destingation if exists already;
    public bool receiver = false;

    public Installation installation;


    // Children members
    TextureButton textureButton;
    UIPopover popoverList;
    Label noTradeLabel;
    UITradeSource currentTradeSourcePanel;
    VBoxContainer vBox;
    static readonly PackedScene p_tradeSource = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeSource.tscn");

    List<Installation> validTradeSources;

    // To avoid failure if no trade route.
    Installation CurrectSourceInstallation
    {
        get
        {
            if (installation.uplineTraderoute != null)
            {
                return installation.uplineTraderoute.source;
            }
            else
            {
                return null;
            };
        }
        set { CurrectSourceInstallation = value; }
    }


    public void Init(Installation _installation)
    {
        installation = _installation;
    }

    public override void _Ready()
    {
        // Global elements
        playerTradeRoutes = GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes");

        textureButton = (TextureButton)GetNode("AlignRight/VBoxContainer/Settings");
        popoverList = (UIPopover)GetNode("PopoverList");
        vBox = popoverList.GetNode<VBoxContainer>("VBoxContainer");
        //currentTradeSourcePanel = GetNode<UITradeSource>("AlignLeft/TradeSource");
        //label = GetNode<Label>("AlignLeft/CurrentDestination/Text");

        currentTradeSourcePanel = (UITradeSource)p_tradeSource.Instantiate();
        currentTradeSourcePanel.Init(installation, CurrectSourceInstallation, this);
        GetNode("AlignLeft").AddChild(currentTradeSourcePanel);
        currentTradeSourcePanel.GetNode<Button>("Button").Flat = true;
        currentTradeSourcePanel.GetNode<Button>("Button").Disabled = true;
        textureButton.Connect("pressed", new Callable(this, "DropDown"));
    }

    public override void _Process(double delta)
    {
        // Draw is too slow to update, makes panel jump around.
        base._Process(delta);
    }

    public override void _Draw()
    {
        base._Draw();
        validTradeSources = GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").list.FindAll(x => ((x != installation)));
        validTradeSources.Insert(0, null);

        if (!(validTradeSources is null) && validTradeSources.Count < 2)
        {
            textureButton.TooltipText = "No stations are able to send freighters.";
            textureButton.Disabled = true;
        }
        else
        {
            textureButton.TooltipText = "Request freighters from another station.";
            textureButton.Disabled = false;
        }
    }

    void DropDown()
    {
        // //tradeDestSelectorButton.Clear();
        // // Todo better structure to avoid this check.
        // //tradeDestSelectorButton.AddIconItem(freighterIcon, " 0 - No Route Assigned", 0);
        // int indexId = 1; //Start at 1 to allow for 'none' in []
        // indexOfExisting = 0;

        //Filter to only valid options
        //validDestinations.Sort

        //Add 'none' destination
        int index = 0;

        foreach (Installation i in validTradeSources)
        {
            UpdateTradeDestination(i, index);
            index++;
        }

        // Any remaining elements greater than index must no longer exist.
        while (vBox.GetChildCount() > index)
        {
            UIResource uir = vBox.GetChildOrNull<UIResource>(index);
            vBox.RemoveChild(uir);
            uir.QueueFree();
        }
        popoverList.Show();
        popoverList.GlobalPosition = GlobalPosition;

        // show current dest somehow.
    }

    void UpdateTradeDestination(Installation i, int index)
    {
        foreach (UITradeSource uir in vBox.GetChildren())
        {
            if (i == uir.sourceInstallation)
            {
                vBox.MoveChild(uir, index);
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        UITradeSource uitd = (UITradeSource)p_tradeSource.Instantiate();
        uitd.Init(installation, i, this);
        vBox.AddChild(uitd);
        vBox.MoveChild(uitd, index);
    }
    public void SetTradeSource(Installation newSourceInstallation)
    {
        // Called from child buttons.

        // Hide List
        popoverList.Visible = false;

        GD.Print($"Existing trade route index {CurrectSourceInstallation}, selected {newSourceInstallation}");
        // If this is true, no change has been made.
        if (CurrectSourceInstallation != newSourceInstallation)
        {
            // If existing non-null, remove it.
            if (CurrectSourceInstallation != null)
            {
                GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(installation.uplineTraderoute);
                installation.uplineTraderoute = null;
            }
            // If selection non-null, create new trade route.
            if (newSourceInstallation != null)
            {
                GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").RegisterTradeRoute(installation, newSourceInstallation);
            }
            currentTradeSourcePanel.QueueFree();
            UITradeSource newTradeSource = p_tradeSource.Instantiate<UITradeSource>();
            newTradeSource.Init(installation, newSourceInstallation, this);
            GetNode<HBoxContainer>("AlignLeft").AddChild(newTradeSource);
            // Hide right hand side. (Undecided)
            newTradeSource.GetNode<HBoxContainer>("Button/AlignRight").Visible = false;
            newTradeSource.GetNode<Button>("Button").Flat = true;
            newTradeSource.GetNode<Button>("Button").Disabled = true;

            currentTradeSourcePanel = newTradeSource;
        }
        //tradeDestSelectorButton.Selected = indexOfExisting;
        // Replace UI element in panel.
        // Force redraw.
        // Replace with 'panel' method'
        QueueRedraw();
        //GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
        //GetParent<Control>().GetParent<Control>().SetDeferred("visible", true);
    }
}
