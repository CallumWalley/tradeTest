using Godot;
using System;
using System.Collections.Generic;

public partial class UIResourceList : UIList
{
    // wrapper for UI list
    public void Init(IEnumerable<System.Object> _object)
    {
        Name = "Supply";
        base.Init(_object, GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn"));
    }
}