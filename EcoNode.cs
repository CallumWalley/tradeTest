using Godot;
using System;

public class EcoNode : Node
{
    public override void _Ready()
    {
		    GetNode<Global>("/root/Global").Connect("EFrame",this, "EconomyFrame");
    }

    public virtual void EconomyFrame(){
    }
}
