using Godot;
using System;
namespace Game;

public partial class UIMapOverlayElement : Control
{
    CollisionShape2D collisionShape2D;

    float UIRotate;
    float UIRadius = 30;
    public Entities.IEntityable element;

    // is mouse over this element.
    bool focus;
    Vector2 positionOnMap;
    Camera camera;
    public bool visible = false;
    public override void _Ready()
    {
        Visible = visible;
        camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
        // // UI Stuff
        CircleShape2D circleShape2D = new CircleShape2D();
        circleShape2D.Radius = UIRadius;
        collisionShape2D = new();
        collisionShape2D.Shape = circleShape2D;
        // Add interactive 
        Area2D area2D = new();
        area2D.AddChild(collisionShape2D);

        area2D.Connect("mouse_entered", new Callable(this, "Focus"));
        area2D.Connect("mouse_exited", new Callable(this, "UnFocus"));
        AddChild(area2D);
    }

    public override void _Draw()
    {
        base._Draw();
        DrawArc(new Vector2(0, 0), UIRadius, UIRotate, endAngle: UIRotate + 4f, 64, new Color(1, 1, 1), 2, true);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (IsVisibleInTree())
        {
            UIRotate += focus ? 0.05f : 0.005f;

            if (Input.IsActionPressed("ui_select"))
            {
                if (focus)
                {
                    camera.Center((Entities.IOrbital)element);
                }
            }

            GlobalPosition = ((Node2D)element).GetCanvasTransform() * ((Node2D)element).GlobalPosition;
            QueueRedraw();
        }
    }

    public void Focus()
    {
        focus = true;
    }
    public void UnFocus()
    {
        focus = false;
    }
}
