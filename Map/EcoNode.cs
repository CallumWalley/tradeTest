using Godot;
using System;

public class EcoNode : Node
{
    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("EFrame_Collect",this, "EFrameCollect");
        GetNode<Global>("/root/Global").Connect("EFrame_Move",this, "EFrameMove");
        GetNode<Global>("/root/Global").Connect("EFrame_Produce",this, "EFrameProduce");
    }

    public virtual void EFrameCollect(){
    }
    public virtual void EFrameMove(){
    }
    public virtual void EFrameProduce(){
    }
}
