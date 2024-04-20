using Godot;
using System;

public partial class FeatureBase : Features.FeatureConstructor
{
    public override void _Ready()
    {
        GetParent().AddChild(Make());
        QueueFree();
    }
}