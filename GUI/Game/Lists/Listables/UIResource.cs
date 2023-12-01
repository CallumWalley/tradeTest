using Godot;
using System;
// [Tool]
public partial class UIResource : UIDrawEFrame, Lists.IListable<Resource.IResource>
{
    public Resource.IResource resource;
    public Resource.IResource GameElement { get { return resource; } }
    public bool Destroy { get; set; } = false;
    public bool ShowName { get; set; } = false;
    public bool ShowDetails { get; set; } = false;
    public bool ShowBreakdown { get; set; } = false;


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
    public void Init(Resource.IRequestable _resource)
    {
        resource = _resource;
    }

    public override void _Ready()
    {
        // //
        // if (Engine.IsEditorHint())
        // {
        //     Init(new Resource.RStatic(_resource));
        // }
        base._Ready();
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: resource.Type);
        TooltipText = resource.Name;
    }


    public override void _Draw()
    {
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
            value.Text = (resource.Sum).ToString();
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
        ExpandDetails((Resource.IResourceGroup<Resource.IResource>)resource, vbc1);
        return vbc1;
    }
    private void ExpandDetails(Resource.IResourceGroup<Resource.IResource> r1, VBoxContainer vbc1)
    {
        // Don't know why, but this is called before ready.
        // Create element representing this.
        UIResource uir = p_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1);
        uir.ShowName = true;
        vbc1.AddChild(uir);

        // If has children, create element to nest inside.
        if (r1.Count > 0)
        {
            HBoxContainer hbc = new();
            VBoxContainer vbc2 = new();
            hbc.AddChild(new VSeparator());
            hbc.AddChild(vbc2);
            foreach (Resource.IResource r2 in r1)
            {
                ExpandDetails(r2, vbc2);
            }
            vbc1.AddChild(hbc);
        }
    }
    private void ExpandDetails(Resource.IResource r1, VBoxContainer vbc1)
    {
        UIResource uir = p_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1);
        uir.ShowName = true;
        vbc1.AddChild(uir);

    }
}
