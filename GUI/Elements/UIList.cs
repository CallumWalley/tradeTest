using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class UIList<T> : BoxContainer
// where T : Lists.IListable<T>
{
    public IEnumerable<T> list;

    public String EmptyText { get; set; }
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

    /// <summary>
    ///  Call to recalculate all child elements.
    /// </summary>
    public virtual void Update()
    {
        /// Queue all children for deletion
        foreach (Node uichild in GetChildren())
        {
            if (typeof(Lists.IListable<T>).IsAssignableFrom(uichild.GetType()))
            {
                ((Lists.IListable<T>)uichild).Destroy = true;
            }
            else
            {
                uichild.QueueFree();
            }
        }
        if (list == null) { GD.Print("List instantiated will null list."); return; }

        foreach (T r in list)
        {
            UpdateElement(r);
        }

        if (!GetChildren().Any(x => ((Control)x).Visible))
        {
            Label label = new Label();
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Text = EmptyText;
            AddChild(label);
        }
        /// For each in list, check if element exists. 

        QueueRedraw();
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
