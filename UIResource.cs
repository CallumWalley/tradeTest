using Godot;
using System;

public class UIResource : Control
{
    [Export]
    public Resource resource;
    [Export]

    public bool invert = false;
    [Export]

    public bool showDetails = false;
    static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/Components/UIResource.tscn");
    static readonly PackedScene p_infoCard = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/UIInfoCard.tscn");

   	UIInfoCard details;

    public void Init(Resource _resource)
    {
        resource = _resource;
        if (resource != null)
        {
            ((TextureRect)GetNode("Self/Icon")).Texture = Resources.Icon(resourceCode: resource.Type);
        }
        else
        {
            GD.Print("UI made without object");
        }
        Connect("mouse_entered", this, "Focus");
        Connect("mouse_exited", this, "UnFocus");
		details = GetNode<UIInfoCard>("Details");
    }

    public void Focus()
    {	
		if (details != null)details.Focus();
    }

	public void UnFocus()
    {
		if (details != null){details.UnFocus();}
    }

    public override void _Draw()
    {
        // Shoudlnt be needed.
        // if (resource==null){return;}
        // Messy
        if (resource != null)
        {
            GetNode<Label>("Self/Value").Text = (resource.Sum).ToString();
        }
        else
        {
            GD.Print("UI made without object");
        }
        if (showDetails && (resource is ResourceAgr))
        {	
			Clean();
            foreach (Resource r in ((ResourceAgr)resource)._add)
            {
                UIResource ui = resourceIcon.Instance<UIResource>();
                ui.Init(r);
                details.AddChild(ui);
            }
            foreach (Resource r in ((ResourceAgr)resource)._sub)
            {
                UIResource ui = resourceIcon.Instance<UIResource>();
                ui.Init(r);
                details.AddChild(ui);
            }
            foreach (Resource r in ((ResourceAgr)resource)._multi)
            {
                UIResource ui = resourceIcon.Instance<UIResource>();
                ui.Init(r);
                details.AddChild(ui);
            }
        }

    }

    void Clean()
    {
        foreach (UIResource c in details.GetChildren())
        {
            details.RemoveChild(c);
            c.QueueFree();
        }
    }

    public void ShowEdit()
    {

        GetNode<TextureButton>("Self/Change/Incriment").Visible = true;
        GetNode<TextureButton>("Self/Change/Decriment").Visible = true;
        GetNode<TextureButton>("Self/Change/Incriment").Connect("pressed", resource, "Incriment");
        GetNode<TextureButton>("Self/Change/Decriment").Connect("pressed", resource, "Decriment");
    }

    public void HideEdit()
    {
        GetNode<TextureButton>("Self/Change/Incriment").Visible = false;
        GetNode<TextureButton>("Self/Change/Decriment").Visible = false;
    }
}
