using Godot;
using System;

public class UIResourceBreakdown : UIPopover
{
    Resource resource;
    readonly PackedScene p_accordian = GD.Load<PackedScene>("res://GUI/Elements/UIAccordian.tscn");
    readonly PackedScene p_resource = GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Render(resource, this);
        this.Raise();
    }
    public override void _Draw()
    {
        base._Draw();
        Raise();
    }
    public void Init(Resource _resource)
    {
        resource = _resource;
    }

    void Render(Resource r, Control parent)
    {
        UIResource newResource = p_resource.Instance<UIResource>();
        newResource.Init(resource, false);

        if (r is ResourceAgr)
        {
            UIAccordian newAccordian = p_accordian.Instance<UIAccordian>();
            parent.AddChild(newAccordian);

            newAccordian.GetNode<Button>("Button").AddChild(newResource);

            foreach (Resource r2 in ((ResourceAgr)r).GetAdd)
            {
                Render(r2, newAccordian.GetNode<Container>("Container"));
            }
            newAccordian.AddChild(new HSeparator());
            foreach (Resource r2 in ((ResourceAgr)r).GetMulti)
            {
                Render(r2, newAccordian.GetNode<Container>("Container"));
            }
            newAccordian.Expanded = false;
        }
        else
        {
            parent.AddChild(newResource);
        }
        newResource.NameVisible = true;

    }
}
