using Godot;
using System;

public partial class UIResourceBreakdown : UIPopover
{
    Resource.IResource resource;
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
    public void Init(Resource.IResource _resource)
    {
        resource = _resource;
    }

    void Render(Resource.IResource r, Control parent, bool multiple = false)
    {
        UIResource newResource = p_resource.Instance<UIResource>();
        newResource.Init(resource, false);

        if (r is Resource.RGroup)
        {
            UIAccordian newAccordian = p_accordian.Instance<UIAccordian>();
            parent.AddChild(newAccordian);

            newAccordian.GetNode<Button>("Button").AddChild(newResource);

            foreach (Resource.IResource r2 in ((Resource.RGroup)r).GetAdd)
            {
                Render(r2, newAccordian.GetNode<Container>("Container"));
            }
            foreach (Resource.IResource r2 in ((Resource.RGroup)r).GetMulti)
            {
                newAccordian.AddChild(new HSeparator());
                break;
            }
            foreach (Resource.IResource r2 in ((Resource.RGroup)r).GetMulti)
            {
                Render(r2, newAccordian.GetNode<Container>("Container"), true);
            }
            newAccordian.Expanded = true;
            newAccordian.OffsetLeft = 8;
        }
        else
        {
            parent.AddChild(newResource);
        }
        newResource.NameVisible = true;
        newResource.OffsetLeft = 8;


    }
}
