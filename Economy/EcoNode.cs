using Godot;
using System;

public partial class EcoNode : Node
{
    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "EFrameEarly"));
        GetNode<Global>("/root/Global").Connect("EFrameLate", new Callable(this, "EFrameLate"));
    }

    public virtual void EFrame()
    {
    }
    public virtual void EFrameLate()
    {
    }
}
