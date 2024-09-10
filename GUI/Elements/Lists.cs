using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Classes for creating dynamic lists.
/// </summary>
public partial class Lists
{
    /// <summary>
    /// A listable element is one that may or may not exist in a parent list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListable<T>
    {
        public T GameElement { get; }
        public void Init(T @object);
        public void Update();
        public void _Draw();

        public bool Visible { get; set; }
        public bool Destroy { set; }// If flag true, must destroy self.

    }

    // Sub-class of listable element used in UIListSelector
    public interface IOrderable<T> : IListable<T>
    {

    }




    // public partial class UIListRequestable : UIList<Resource.IResource>
    // {
    //     public bool ShowName { get; set; } = false;
    //     public bool ShowDetails { get; set; } = false;
    //     public bool ShowBreakdown { get; set; } = false;
    //     public void Init(IEnumerable<Resource.IResource> _object)
    //     {
    //         Name = "Supply";
    //         base.Init(_object, GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn"));
    //     }

    //     protected override UIResource CreateNewElement(Resource.IResource r)
    //     {
    //         UIResource rui = (UIResource)prefab.Instantiate();
    //         rui.Init(r);
    //         rui.ShowName = ShowName;
    //         rui.ShowDetails = ShowDetails;
    //         rui.ShowBreakdown = ShowBreakdown;
    //         return rui;
    //     }
    // }
}