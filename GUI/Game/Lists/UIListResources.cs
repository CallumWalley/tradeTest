using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class UIListResources : UIList<Resource.IResource>
{
    public bool ShowName { get; set; } = false;
    public bool ShowDetails { get; set; } = false;
    public bool ShowBreakdown { get; set; } = false;
    public void Init(IEnumerable<Resource.IResource> _object)
    {
        Name = "Supply";
        base.Init(_object, GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn"));
    }

    protected override UIResource CreateNewElement(Resource.IResource r)
    {
        UIResource rui = (UIResource)prefab.Instantiate();
        rui.Init(r);
        rui.ShowName = ShowName;
        rui.ShowDetails = ShowDetails;
        rui.ShowBreakdown = ShowBreakdown;
        return rui;
    }
}