using Godot;
using System;
namespace Game;

public partial class UINetworkInfo : HBoxContainer
{
    public Domain domain;

    Label label;


    public override void _Ready()
    {
        base._Ready();
        label = GetNode<Label>("MarginContainer/Label");
    }

    public override void _Draw()
    {
        base._Draw();
        if (domain.Network == null)
        {
            label.Text = "Not part of an external market.";
        }
        else
        {
            label.Text = $"Member of the '{domain.Network.NetworkName}'";
        }
    }
}
