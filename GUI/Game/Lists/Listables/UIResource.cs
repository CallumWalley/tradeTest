using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
// [Tool]
public partial class UIResource : Control, Lists.IListable<Resource.IResource>
{
    public Resource.IResource resource;
    public Resource.IResource GameElement { get { return resource; } }
    public bool Destroy { get; set; } = false;
    public bool ShowName { get; set; } = false;
    public bool ShowDetails { get; set; } = false;
    public bool ShowBreakdown { get; set; } = false;

    Color colorBad = new(1, 0, 0);

    // For use in editor only.
    // [Export(PropertyHint.Flags, "Water:1,Energy:2,Minerals:3")]
    // public int _resource;

    // Child components
    public Label value;
    public Label name;
    private Label details;

    protected static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");

    public void Init(Resource.IResource _resource)
    {
        resource = _resource;
    }

    public override void _Ready()
    {
        base._Ready();
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon((resource != null) ? resource.Type : 0);
        TooltipText = resource.Name;
    }


    public override void _Draw()
    {
        Update();

        if (Destroy)
        {
            Visible = false;
            QueueFree();
        }
        else
        {
            // hide if null.
            //Visible = !(resource.Count < 1 && Mathf.Abs(resource.Sum) < 0.1);
            details.Visible = ShowDetails;
            name.Visible = ShowName;
        }
    }

    public void Update()
    {
        // 
        if (resource is Resource.IRequestable && ((Resource.IRequestable)resource).State > 0)
        {
            value.Text = string.Format("{0:G}/{1:G}", resource.Sum, ((Resource.IRequestable)resource).Request);
            name.Text = $"{resource.Name}";
            value.AddThemeColorOverride("font_color", colorBad);
            name.AddThemeColorOverride("font_color", colorBad);
        }
        // else if (resource.Sum == 0)
        // {
        //     value.RemoveThemeColorOverride("font_color");
        //     name.RemoveThemeColorOverride("font_color");
        //     value.Text = "-";
        //     name.Text = $"{resource.Name} : ";
        // }
        else
        {
            value.RemoveThemeColorOverride("font_color");
            name.RemoveThemeColorOverride("font_color");
            value.Text = string.Format("{0:G}", resource.Sum);
            name.Text = $"{resource.Name} : ";
        }
    }


    public override Control _MakeCustomTooltip(string forText)
    {
        if (!ShowBreakdown)
        {
            return null;
        }
        VBoxContainer vbc1 = new();
        ExpandDetails(resource, vbc1);
        return vbc1;
    }
    // private void ExpandDetails(Resource.IResourceGroup<Resource.IRequestable> r1, VBoxContainer vbc1)
    // {
    //     // Don't know why, but this is called before ready.
    //     // Create element representing this.
    //     UIResource uir = new UIResource();
    //     uir.Init(r1);
    //     uir.ShowName = true;
    //     vbc1.AddChild(uir);

    //     // If has children, create element to nest inside.
    //     if (r1.Count > 0)
    //     {
    //         HBoxContainer hbc = new();
    //         VBoxContainer vbc2 = new();
    //         hbc.AddChild(new VSeparator());
    //         hbc.AddChild(vbc2);
    //         foreach (Resource.IRequestable r2 in r1)
    //         {
    //             ExpandDetails(r2, vbc2);
    //         }
    //         vbc1.AddChild(hbc);
    //     }
    // }
    private void ExpandDetails(Resource.IResource r1, VBoxContainer vbc1)
    {
        UIResource uir = p_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1);
        uir.ShowName = true;
        vbc1.AddChild(uir);
        if (r1 is Resource.IResourceGroup<Resource.IResource> && ((Resource.IResourceGroup<Resource.IResource>)r1).Count > 0)
        {
            HBoxContainer hbc = new();
            VBoxContainer vbc2 = new();
            hbc.AddChild(new VSeparator());
            hbc.AddChild(vbc2);
            foreach (Resource.IResource r2 in (Resource.IResourceGroup<Resource.IResource>)r1)
            {
                ExpandDetails(r2, vbc2);
            }
            vbc1.AddChild(hbc);
        }
    }
}
