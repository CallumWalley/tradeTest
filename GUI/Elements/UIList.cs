using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIList<T> : BoxContainer
{
    public IEnumerable<T> list;
    protected PackedScene prefab;

    Control unset;
    List<UIContainers.IListable<T>> uichildren = new(); // Because godot arrays are a pain.

    // public virtual void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
    // {
    //     list = _list;
    //     prefab = _prefab;
    // }
    // if prefab exists, try that. Else use class.
    public virtual void Init(IEnumerable<T> _list, PackedScene _prefab)
    {
        list = _list;
        prefab = _prefab;
    }
    public virtual void Init(IEnumerable<T> _list, Type type)
    {
        list = _list;
    }
    public virtual Control DrawUnset()
    {
        Label label = new Label();
        label.Text = "Empty";
        return label;
    }
    public override void _Draw()
    {

        if (list == null)
        {
            GD.Print($"UI list tring to draw null");
            return;
        }

        // // Check if contains 'default' value
        // if (unset != null)
        // {

        // }

        foreach (UIContainers.IListable<T> uichild in uichildren)
        {
            uichild.Destroy = true;
        }

        // if (list.Count() < 1)
        // {
        //     unset = DrawUnset();
        //     AddChild(DrawUnset());
        // }

        foreach (T r in list)
        {
            // Dont know why these elements are null somethimes.
            // if (r == null)
            // {
            //     // list.Remove()
            // }
            Update(r);
            //index++;
        }
        //GD.Print($"{GetChildCount()} tracking {index} objects.");

        // Any remaining elements greater than index must no longer exist.
        // Cannot use  GetChildCount() directly as element removal is queued.
        // while (uichildren.Count > index)
        // {
        //     Control uir = GetChildOrNull<Control>(index);
        //     if (uir == null)
        //     {
        //         GD.Print("UI element out of sync.");
        //     }
        //     else
        //     {
        //         RemoveChild(uir);
        //         uir.QueueFree();
        //     }
        // }
    }
    // public override void _Ready()
    // {
    //     GetNode<Global>("/root/Global").Connect("EFrame", new Callable(this, "DeferredDraw"));
    // }
    // void DeferredDraw()
    // {
    //     Visible = false;
    //     SetDeferred("visible", true);
    // }
    protected void Update(T r)
    {
        foreach (UIContainers.IListable<T> uir in uichildren)
        {
            if (ReferenceEquals(uir.GameElement, r))
            {
                uir.Destroy = false;
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        UIContainers.IListable<T> newui = (UIContainers.IListable<T>)prefab.Instantiate();
        newui.Init(r);
        AddChild((Control)newui);
        uichildren.Add(newui);
        //MoveChild(ui.Control, index);
    }
}