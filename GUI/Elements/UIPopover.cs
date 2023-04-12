using Godot;
using System;

public class UIPopover : UIElement
{   
    public bool moveWithParent = true;
    Vector2 offset; // initial offset to parent.

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("ui_cancel"))
        {
            SetDeferred("visible", false);
        }
    }

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Draw()
    {
        base._Draw();
    }
    public override void _Process(float _delta)
    {   
        base._Process(_delta);
        if (moveWithParent){
            Control p = GetParentOrNull<Control>();
            if (p != null){
                RectPosition = p.RectGlobalPosition;//+ offset;
            }
        }

    }
}