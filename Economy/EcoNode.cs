using Godot;
using System;

public partial class EcoNode : Node
{
    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("EFrame", new Callable(this, "EFrame"));
    }

    public virtual void EFrame()
    {
    }
}
