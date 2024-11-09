using Godot;
using System;
namespace Game;

public partial class UIPanelDomainSupply : UIPanel
{
    UIPanelLedger panelLedger;
    public Domain domain;
    public override void _Ready()
    {
        base._Ready();
        panelLedger = new();
        panelLedger.Ledger = domain.Ledger;
        GetNode<HBoxContainer>("MarginContainer/HBoxContainer").AddChild(panelLedger);
    }

    public override void OnEFrameUpdate()
    {
        panelLedger.Update();
    }
}
