using Godot;
using System;
namespace Game;

public partial class UIPanelPositionGeneral : UIPanel
{

    public Entities.IPosition position;
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

        nameLabel.Text = $"Name: {position.Name}";
        // nameLabel.Text = $"Name: {resourcePool.}";

    }
}
