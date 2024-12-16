using Game;
using Godot;
using System;

public partial class UIOrbitalDescription : VBoxContainer
{
    [Export]
    Label richTextLabel;
    public Entities.IOrbital orbital;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Draw()
    {
        base._Draw();
        richTextLabel.Text = string.Format("Semi-Major Axis: {0} Mm\nOrbital Anomaly: {1}\nOrbital Eccentricity {2}\nPeriod: {3} Gs", orbital.SemiMajorAxis, orbital.Anomaly, orbital.Eccentricity, orbital.Period);
    }
}
