using Godot;
using System;
using System.Collections;
[Tool]
public partial class UIResourceRequestWithDetails : UIResourceRequest
{
    // Prefabs
    protected static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResourceRequest.tscn");

    public override Control _MakeCustomTooltip(string forText)
    {
        VBoxContainer vbc1 = new();
        ExpandDetails(request, vbc1);
        //Connect("mouse_entered", new Callable(vbc1, "OnMouseEnter"));

        // Label details = new Label();
        // details.Text = resource.Details;
        // vbc1.AddChild(details);
        // if (resource.Count > 0)
        // {
        //     HBoxContainer hbc = new();
        //     VBoxContainer vbc2 = new();
        //     hbc.AddChild(new VSeparator());
        //     hbc.AddChild(vbc2);
        //     foreach (Resource.IResource r2 in ((Resource.RGroup<Resource.IResource>)resource).Adders)
        //     {
        //         ExpandDetails(r2, vbc2);
        //     }
        //     vbc1.AddChild(hbc);
        // }
        return vbc1;
    }

    private void ExpandDetails(Resource.IRequestable r1, VBoxContainer vbc1)
    {
        // Don't know why, but this is called before ready.
        // Create element representing this.
        UIResourceRequest uir = p_resourceIcon.Instantiate<UIResourceRequest>();
        uir.Init(r1, true, true);
        vbc1.AddChild(uir);

        // If has children, create element to nest inside.
        if (r1.Count > 0)
        {
            HBoxContainer hbc = new();
            VBoxContainer vbc2 = new();
            hbc.AddChild(new VSeparator());
            hbc.AddChild(vbc2);
            foreach (Resource.IRequestable r2 in ((Resource.RGroup<Resource.IResource>)r1).Adders)
            {
                ExpandDetails(r2, vbc2);
            }
            vbc1.AddChild(hbc);
        }
    }
}