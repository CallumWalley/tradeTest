using Godot;
using System;
using System.Collections.Generic;

public partial class UIListResources : UIList<Resource.IResource>
{
    // wrapper for UI list
    public void Init(IEnumerable<Resource.IResource> _object)
    {
        Name = "Supply";
        base.Init(_object, GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn"));
    }
}