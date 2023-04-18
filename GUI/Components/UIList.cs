using Godot;
using System;
using System.Collections.Generic;

public class UIList : Control
{
    public interface IListable
    {
        System.Object GameElement { get; }
        // This must return 'this'
        Control Control { get; }
        void Init(System.Object gameElement);

    }
    public IEnumerable<System.Object> list;
    protected HBoxContainer hbox;
    PackedScene prefab;


    public void Init(IEnumerable<System.Object> _list, PackedScene _prefab)
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
            Update(r, index);
            index++;
        }
        // Any remaining elements greater than index must no longer exist.
        while (hbox.GetChildCount() > index)
        {
            Control uir = hbox.GetChildOrNull<Control>(index + 1);
            hbox.RemoveChild(uir);
            uir.QueueFree();
        }
    }

    public override void _Ready()
    {
        hbox = GetNode<HBoxContainer>("HBoxContainer/AlignLeft");
        GetNode<Global>("/root/Global").Connect("EFrameEarly", this, "DeferredDraw");
    }
    void DeferredDraw()
    {
        Visible = false;
        SetDeferred("visible", true);
    }
    void Update(System.Object r, int index)
    {
        foreach (IListable uir in hbox.GetChildren())
        {
            if (r == uir.GameElement)
            {
                hbox.MoveChild(uir.Control, index);
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        IListable ui = (IListable)prefab.Instance();
        ui.Init(r);
        hbox.AddChild(ui.Control);
        hbox.MoveChild(ui.Control, index);
    }
}
