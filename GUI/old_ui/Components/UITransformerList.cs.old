using Godot;
using System;
using System.Collections.Generic;

public class UITransformerList : UIList
{
    static readonly PackedScene p_transformer = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UITransformer.tscn");

    public void Init(Installation installation)
    {
        base.Init(new List<System.Object> { installation.GetChildren() }, p_transformer);
    }
    // wrapper
}
