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

        public bool Destroy { set; }// If flag true, must destroy self.

    }
    public interface IOrderable<T> : IListable<T>
    {

    }
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
            foreach (IListable<T> uichild in GetChildren())
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
            foreach (IListable<T> uir in GetChildren())
            {
                if (ReferenceEquals(uir.GameElement, r))
                {
                    uir.Destroy = false;
                    //uir.Update();
                    return;
                }
            }
            // If doesn't exist, add it and insert at postition.

            AddChild((Control)CreateNewElement(r));
        }

        protected virtual IListable<T> CreateNewElement(T r)
        {
            IListable<T> newui = (IListable<T>)prefab.Instantiate();
            newui.Init(r);
            return newui;
        }
    }


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

    // public partial class UIListRequestable : UIList<Resource.IRequestable>
    // {
    //     public bool ShowName { get; set; } = false;
    //     public bool ShowDetails { get; set; } = false;
    //     public bool ShowBreakdown { get; set; } = false;
    //     public void Init(IEnumerable<Resource.IRequestable> _object)
    //     {
    //         Name = "Supply";
    //         base.Init(_object, GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn"));
    //     }

    //     protected override UIResource CreateNewElement(Resource.IRequestable r)
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