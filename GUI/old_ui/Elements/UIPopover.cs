using Godot;
using System;

public partial class UIPopover : Control
{
    // Will call 'CloseCallback' if focus is off for too long.
    public bool MoveWithParent { get; set; } = true;

    // Make 999 to disable hiding
    public double HidePeriod { get; set; } = 0.6f;
    double count = 0f;

    public bool Focus = true;

    public Action CloseCallback;

    public Vector2 offset; // initial offset to parent.

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("ui_cancel"))
        {
            CloseCallback?.Invoke();
        }
    }
    public override void _Ready()
    {
        base._Ready();
        TopLevel = true;
        count = HidePeriod;


        // // defaultVisibile = Visible;
        Connect("mouse_entered", new Callable(this, "MouseEnter"));
        Connect("mouse_exited", new Callable(this, "MouseExit"));
    }

    public override void _Draw()
    {
        base._Draw();
    }

    void MouseEnter()
    {
        count = HidePeriod;
        Focus = true;
    }
    void MouseExit()
    {
        Focus = false;
    }

    public override void _Process(double _delta)
    {
        base._Process(_delta);
        if (MoveWithParent)
        {
            Control p = GetParentOrNull<Control>();
            if (p != null)
            {
                Position = (Vector2I)p.GlobalPosition + offset;
            }
        }
        if (!Focus)
        {
            count -= _delta;
        }

        if (count <= 0)
        {
            CloseCallback?.Invoke();
        }
    }
}
