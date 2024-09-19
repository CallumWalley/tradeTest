using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class UIList<T> : BoxContainer
// where T : Lists.IListable<T>
{
    public IEnumerable<T> list;
    protected PackedScene prefab;

    Control unset;

    public virtual void Init(IEnumerable<T> _list, PackedScene _prefab)
    {
        list = _list;
        prefab = _prefab;
        Update();
    }
    // public virtual void Init(IEnumerable<T> _list, Type type)
    // {
    //     list = _list;
    //     Update();
    // }
    public virtual Control DrawUnset()
    {
        Label label = new Label();
        label.Text = "Empty";
        return label;
    }

    /// <summary>
    ///  Call to recalculate all child elements.
    /// </summary>
    public virtual void Update()
    {
        /// Queue all children for deletion
        foreach (Lists.IListable<T> uichild in GetChildren())
        {
            uichild.Destroy = true;
        }
        if (list == null) { GD.Print("List instantiated will null list."); return; }
        /// For each in list, check if element exists. 
        foreach (T r in list)
        {
            UpdateElement(r);
        }
        QueueRedraw();
        // eww
        // force child redraw.
        // Visible = !Visible;
        // Visible = !Visible;
    }

    public override void _Draw()
    {
        if (list == null)
        {
            GD.Print($"UI list tring to draw null");
            return;
        }
    }

    protected virtual void UpdateElement(T r)
    {
        foreach (Lists.IListable<T> uir in GetChildren())
        {
            if (ReferenceEquals(uir.GameElement, r))
            {
                uir.Destroy = false;
                uir.Update();
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        InsertNewElement(CreateNewElement(r));
    }

    /// <summary>
    ///  Overwrite this, for elements needing a more complicated init.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    protected virtual Lists.IListable<T> CreateNewElement(T r)
    {
        var newui = (Lists.IListable<T>)prefab.Instantiate<Lists.IListable<T>>();
        newui.Init(r);
        return newui;
    }
    protected virtual void InsertNewElement(Lists.IListable<T> r)
    {
        AddChild((Control)r);
    }
}
