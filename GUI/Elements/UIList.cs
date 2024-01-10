using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class UIList<T> : BoxContainer
{
    public IEnumerable<T> list;
    protected PackedScene prefab;

    Control unset;


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

    /// <summary>
    ///  Call to recalculate all child elements.
    /// </summary>
    public void Update()
    {
        /// Queue all children for deletion
        foreach (Lists.IListable<T> uichild in GetChildren())
        {
            uichild.Destroy = true;
        }

        /// For each in list, check if element exists. 
        foreach (T r in list)
        {
            UpdateElement(r);
        }
    }

    public override void _Draw()
    {
        if (list == null)
        {
            GD.Print($"UI list tring to draw null");
            return;
        }
    }

    protected void UpdateElement(T r)
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

        AddChild((Control)CreateNewElement(r));
    }

    protected virtual Lists.IListable<T> CreateNewElement(T r)
    {
        Lists.IListable<T> newui = (Lists.IListable<T>)prefab.Instantiate();
        newui.Init(r);
        return newui;
    }
}
