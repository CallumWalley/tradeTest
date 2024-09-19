using Godot;
using System;

public partial class UIPanelPoolSupply : UIPanel
{
    UIPanelLedger panelLedger;
    public Domain resourcePool;
    public override void _Ready()
    {
        base._Ready();
        panelLedger = new();
        panelLedger.Ledger = resourcePool.Ledger;
        GetNode<HBoxContainer>("MarginContainer/HBoxContainer").AddChild(panelLedger);
    }

    public override void OnEFrameUpdate()
    {
        panelLedger.Update();
    }
}
