using Godot;
using System;

public partial class UIPanelPoolSupply : UIPanel
{
    UIPanelLedger panelLedger;
    public ResourcePool resourcePool;
    public override void _Ready()
    {
        panelLedger = new();
        panelLedger.Ledger = resourcePool.Ledger;
        GetNode<HBoxContainer>("HBoxContainer").AddChild(panelLedger);
    }

    public override void OnEFrameUpdate()
    {
        panelLedger.Update();
    }
}
