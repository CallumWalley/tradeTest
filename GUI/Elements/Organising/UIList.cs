using Godot;
using System;
using System.Collections.Generic;

public partial class UIList : BoxContainer
{
    public IEnumerable<System.Object> list;
    protected PackedScene prefab;

    // public virtual void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
    // {
    //     list = _list;
    //     prefab = _prefab;
    // }
    public virtual void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
    {
        list = _list;
        prefab = _prefab;
    }
    // public virtual void Init(Node _parent, PackedScene _prefab)
    // {
    //     list = _parent;
    //     prefab = _prefab;
    // }
    public override void _Draw()
    {

        if (list == null)
        {
            GD.Print($"UI list tring to draw null");
            return;
        }
        // Go over all trade routes in pool, and either update or create. 
        int index = 0;
        foreach (System.Object r in list)
        {
            // Dont know why these elements are null somethimes.
            if (r == null)
            {
                // list.Remove()
            }
            Update(r, index);
            index++;
        }
        //GD.Print($"{GetChildCount()} tracking {index} objects.");

        // Any remaining elements greater than index must no longer exist.
        // Cannot use  GetChildCount() directly as element removal is queued.
        var cdebug = GetChildren();
        while (GetChildCount() > index)
        {
            Control uir = GetChildOrNull<Control>(index);
            if (uir == null)
            {
                GD.Print("UI element out of sync.");
            }
            else
            {
                RemoveChild(uir);
                uir.QueueFree();
            }
        }
    }
    // public override void _Ready()
    // {
    //     GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "DeferredDraw"));
    // }
    // void DeferredDraw()
    // {
    //     Visible = false;
    //     SetDeferred("visible", true);
    // }
    protected void Update(System.Object r, int index)
    {
        var uichildren = GetChildren();
        foreach (UIContainers.IListable uir in uichildren)
        {
            if (r == uir.GameElement)
            {
                MoveChild(uir.Control, index);
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        UIContainers.IListable ui = (UIContainers.IListable)prefab.Instantiate();
        ui.Init(r);
        AddChild(ui.Control);
        MoveChild(ui.Control, index);
    }
}