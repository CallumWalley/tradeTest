using Godot;
using System;

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
	}

	public override void _Draw(){
		base._Draw();
		DrawArc(new Vector2(0,0), UIRadius, UIRotate, UIRotate + 4f, 64, new Color(1, 1, 1), 10, true);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
        // if (IsVisibleInTree())
        // {
        //     UIRotate += focus ? 0.05f : 0.005f;

        //     if (Input.IsActionPressed("ui_select"))
        //     {
        //         if (focus)
        //         {
        //             camera.Center((Entities.IOrbital)element);
        //         }
        //     }
        // }
        GlobalPosition=((Node2D)element).GetViewportTransform() * ((Node2D)element).GlobalPosition;
        QueueRedraw();
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
