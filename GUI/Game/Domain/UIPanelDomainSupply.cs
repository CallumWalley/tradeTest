using Godot;
using System;
namespace Game;

public partial class UIPanelDomainSupply : UIPanel
{
    UIPanelLedger panelLedger;

    [Export]
    public Label label;
    public Domain domain;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void OnEFrameUpdate()
    {
        label.Text = domain.Name;
        panelLedger = GetNodeOrNull<UIPanelLedger>("MarginContainer/VBoxContainer/HBoxContainer/Ledger");
        if (panelLedger == null)
        {
            panelLedger = new UIPanelLedger();
            panelLedger.Ledger = domain.Ledger;
            panelLedger.Name = "Ledger";
            GetNode<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer").AddChild(panelLedger);
        }
        panelLedger.Update();
    }
}
