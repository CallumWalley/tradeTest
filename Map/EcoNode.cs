using Godot;
using System;

public class EcoNode : Node
{
    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("EFrameEarly",this, "EFrameEarly");
        GetNode<Global>("/root/Global").Connect("EFrameLate",this, "EFrameLate");

    }

    public virtual void EFrameEarly(){
    }
    public virtual void EFrameLate(){
    }
}
