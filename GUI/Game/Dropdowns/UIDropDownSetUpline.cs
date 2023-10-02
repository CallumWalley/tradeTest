using Godot;
using System;
using System.Collections.Generic;

public partial class UIDropDownSetUpline : UIDropDown
{ }
//     PlayerTradeReciever globalTrade;
//     PlayerTradeRoutes playerTradeRoutes;
//     static readonly Texture2D freighterIcon = GD.Load<Texture2D>("res://assets/icons/freighter.png");


//     // Index of trade route destingation if exists already;
//     public bool receiver = false;

//     public Installation installation;


//     // Children members
//     TextureButton textureButton;
//     VBoxContainer dropdownList;
//     UIPopover uIPopover;
//     Label noTradeLabel;
//     UITradeSource currentTradeSourcePanel;
//     VBoxContainer vBox;
//     static readonly PackedScene p_tradeSource = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITradeSource.tscn");

//     List<Installation> validTradeSources;

//     // To avoid failure if no trade route.
//     Installation CurrectSourceInstallation
//     {
//         get
//         {
//             if (installation.UplineTraderoute != null)
//             {
//                 return installation.UplineTraderoute.Head;
//             }
//             else
//             {
//                 return null;
//             };
//         }
//         set { CurrectSourceInstallation = value; }
//     }


//     public void Init(Installation _installation)
//     {
//         installation = _installation;
//     }

//     public override void _Ready()
//     {

//         // Global elements
//         playerTradeRoutes = GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes");

//         textureButton = GetNode<TextureButton>("AlignRight/VBoxContainer/Settings");
//         uIPopover = GetNode<UIPopover>("Popover");
//         uIPopover.HidePeriod = 9999;
//         dropdownList = new();
//         uIPopover.AddChild(dropdownList);
//         uIPopover.CloseCallback = DropDownClose;

//         //currentTradeSourcePanel = GetNode<UITradeSource>("AlignLeft/TradeSource");
//         //label = GetNode<Label>("AlignLeft/CurrentDestination/Text");

//         // currentTradeSourcePanel = (UITradeSource)p_tradeSource.Instantiate();
//         // currentTradeSourcePanel.Init(installation, CurrectSourceInstallation, this);
//         GetNode("AlignLeft").AddChild(currentTradeSourcePanel);
//         currentTradeSourcePanel.GetNode<Button>("Button").Flat = true;
//         currentTradeSourcePanel.GetNode<Button>("Button").Disabled = true;
//         textureButton.Connect("pressed", new Callable(this, "DropDownOpen"));

//         PopulateList();
//     }

//     public override void _Process(double delta)
//     {
//         // Draw is too slow to update, makes panel jump around.
//         base._Process(delta);
//     }

//     public override void _Draw()
//     {
//         base._Draw();

//         if (!(validTradeSources is null) && validTradeSources.Count < 2)
//         {
//             textureButton.TooltipText = "No stations are able to send freighters.";
//             textureButton.Disabled = true;
//         }
//         else
//         {
//             textureButton.TooltipText = "Request freighters from another station.";
//             textureButton.Disabled = false;
//         }
//     }

//     void PopulateList()
//     {
//         foreach (UITradeSource uits in dropdownList.GetChildren())
//         {
//             uits.HideTradeRoute();
//             uits.QueueFree();
//         }
//         // Get list of valid trade sources;
//         // This could be done async;
//         validTradeSources = GetNode<PlayerTradeReciever>("/root/Global/Player/Trade/Receivers").list.FindAll(x => ((x != installation)));
//         validTradeSources.Insert(0, null);

//         foreach (Installation i in validTradeSources)
//         {
//             UITradeSource uits = p_tradeSource.Instantiate<UITradeSource>();
//             uits.Init(installation, i, this);
//             dropdownList.AddChild(uits);
//         }
//     }

//     void DropDownOpen()
//     {
//         uIPopover.Show();
//         PopulateList();
//     }

//     void DropDownClose()
//     {
//         uIPopover.Hide();
//     }

//     public void SetTradeSource(Installation newSourceInstallation)
//     {
//         // Called from child buttons.

//         DropDownClose();

//         GD.Print($"Existing trade route index {CurrectSourceInstallation}, selected {newSourceInstallation}");
//         // If this is true, no change has been made.
//         if (CurrectSourceInstallation != newSourceInstallation)
//         {
//             // If existing non-null, remove it.
//             if (CurrectSourceInstallation != null)
//             {
//                 GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(installation.UplineTraderoute);
//                 installation.UplineTraderoute = null;
//             }
//             // If selection non-null, create new trade route.
//             if (newSourceInstallation != null)
//             {
//                 GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").RegisterTradeRoute(installation, newSourceInstallation);
//             }
//             currentTradeSourcePanel.QueueFree();
//             UITradeSource newTradeSource = p_tradeSource.Instantiate<UITradeSource>();
//             newTradeSource.Init(installation, newSourceInstallation, this);
//             GetNode<HBoxContainer>("AlignLeft").AddChild(newTradeSource);
//             // Hide right hand side. (Undecided)
//             newTradeSource.GetNode<HBoxContainer>("Button/AlignRight").Visible = false;
//             newTradeSource.GetNode<Button>("Button").Flat = true;
//             newTradeSource.GetNode<Button>("Button").Disabled = true;

//             currentTradeSourcePanel = newTradeSource;
//         }
//         //tradeDestSelectorButton.Selected = indexOfExisting;
//         // Replace UI element in panel.
//         // Force redraw.
//         // Replace with 'panel' method'
//         QueueRedraw();
//         //GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
//         //GetParent<Control>().GetParent<Control>().SetDeferred("visible", true);
//     }
// }
