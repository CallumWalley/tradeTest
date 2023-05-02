using Godot;
using System;
using System.Collections.Generic;

public partial class UIContainers
{
    public interface IListable
    {
        System.Object GameElement { get; }
        // This must return 'this'
        Control Control { get; }
        void Init(System.Object gameElement);
    }
    public abstract class UIHList : HBoxContainer
    {

        public IEnumerable<System.Object> list;
        protected PackedScene prefab;

        public virtual void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
        {
            list = _list;
            prefab = _prefab;
        }
        public override void _Draw()
        {
            if (list == null) { return; }
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
            // Any remaining elements greater than index must no longer exist.
            while (GetChildCount() > index)
            {
                Control uir = GetChildOrNull<Control>(index + 1);
                RemoveChild(uir);
                uir.QueueFree();
            }
        }
        public override void _Ready()
        {
            GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "DeferredDraw"));
        }
        void DeferredDraw()
        {
            Visible = false;
            SetDeferred("visible", true);
        }
        protected void Update(System.Object r, int index)
        {
            foreach (IListable uir in GetChildren())
            {
                if (r == uir.GameElement)
                {
                    MoveChild(uir.Control, index);
                    return;
                }
            }
            // If doesn't exist, add it and insert at postition.
            IListable ui = (IListable)prefab.Instance();
            ui.Init(r);
            AddChild(ui.Control);
            MoveChild(ui.Control, index);
        }
    }

    public partial class UIHListChildren : UIHList
    {
        // does the same thing as UI list, but with node children
        // because ARRAYS ARNT CASTABLE AND ITS FUNCKC ANNOYING
        Node node;
        public void Init(Node _node, PackedScene _prefab)
        {
            node = _node;
            prefab = _prefab;
        }

        public override void _Draw()
        {
            if (node == null) { return; }
            // Go over all trade routes in pool, and either update or create. 
            int index = 0;
            foreach (Node r in node.GetChildren())
            {
                // Dont know why these elements are null somethimes.
                if (r == null)
                {
                    // list.Remove()

                }
                Update(r, index);
                index++;
            }
            // Any remaining elements greater than index must no longer exist.
            while (GetChildCount() > index)
            {
                Control uir = GetChildOrNull<Control>(index + 1);
                RemoveChild(uir);
                uir.QueueFree();
            }
        }
    }

    public partial class UIHResourceList : UIHList
    {
        //Just a wrapper
        static readonly PackedScene p_resource = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");

        // wrapper
        public void Init(IEnumerable<System.Object> _object)
        {
            base.Init(_object, p_resource);
        }
    }

    public abstract class UIVList : VBoxContainer
    {

        public IEnumerable<System.Object> list;
        protected PackedScene prefab;

        public virtual void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
        {
            list = _list;
            prefab = _prefab;
        }

        public override void _Draw()
        {
            if (list == null) { return; }
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
            // Any remaining elements greater than index must no longer exist.
            while (GetChildCount() > index)
            {
                Control uir = GetChildOrNull<Control>(index + 1);
                RemoveChild(uir);
                uir.QueueFree();
            }
        }
        public override void _Ready()
        {
            GetNode<Global>("/root/Global").Connect("EFrameEarly", new Callable(this, "DeferredDraw"));
        }
        void DeferredDraw()
        {
            Visible = false;
            SetDeferred("visible", true);
        }
        protected void Update(System.Object r, int index)
        {
            foreach (IListable uir in GetChildren())
            {
                if (r == uir.GameElement)
                {
                    MoveChild(uir.Control, index);
                    return;
                }
            }
            // If doesn't exist, add it and insert at postition.
            IListable ui = (IListable)prefab.Instance();
            ui.Init(r);
            AddChild(ui.Control);
            MoveChild(ui.Control, index);
        }
    }

    public partial class UIVListChildren : UIVList
    {
        // does the same thing as UI list, but with node children
        // because ARRAYS ARNT CASTABLE AND ITS FUNCKC ANNOYING
        Node node;
        public void Init(Node _node, PackedScene _prefab)
        {
            node = _node;
            prefab = _prefab;
        }

        public override void _Draw()
        {
            if (node == null) { return; }
            // Go over all trade routes in pool, and either update or create. 
            int index = 0;
            foreach (Node r in node.GetChildren())
            {
                // Dont know why these elements are null somethimes.
                if (r == null)
                {
                    // list.Remove()

                }
                Update(r, index);
                index++;
            }
            // Any remaining elements greater than index must no longer exist.
            while (GetChildCount() > index)
            {
                Control uir = GetChildOrNull<Control>(index + 1);
                RemoveChild(uir);
                uir.QueueFree();
            }
        }
    }

    public partial class UIVResourceList : UIVList
    {
        //Just a wrapper
        static readonly PackedScene p_resource = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");

        // wrapper
        public void Init(IEnumerable<System.Object> _object)
        {
            base.Init(_object, p_resource);
        }
    }
}
