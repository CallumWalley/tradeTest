using Godot;
using System;

public partial class UIPanelPoolGeneral : UIPanel
{

    public Domain resourcePool;
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;

    public override void _Ready()
    {
        nameLabel = GetNode<Label>("TabContainer/Designation/Name");
        adjLabel = GetNode<Label>("TabContainer/Designation/Adj");
        altNameLabel = GetNode<Label>("TabContainer/Designation/AltNames");
    }

    public override void _Draw()
    {
        base._Draw();

        nameLabel.Text = $"Name: {resourcePool.Name}";


        // nameLabel.Text = $"Name: {resourcePool.}";

    }
}
