using Godot;
using System;

public partial class UIPopover : Control
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
        TopLevel = true;
    }
    public override void _Draw()
    {
        base._Draw();
    }
    public override void _Process(double _delta)
    {
        base._Process(_delta);
        if (moveWithParent)
        {
            Control p = GetParentOrNull<Control>();
            if (p != null)
            {
                Position = (Vector2I)p.GlobalPosition;//+ offset;
            }
        }

    }
}
