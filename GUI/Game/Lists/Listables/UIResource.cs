using Godot;
using System;
public partial class UIResource : Control, UIList<Resource.IResource>.IListable<Resource.IResource>
{
    public Resource.IResource resource;
    public Resource.IResource GameElement { get { return resource; } }
    public bool Destroy { get; set; } = false;

    // Child components
    public Label value;
    public Label name;
    private Label details;

    bool showName;
    bool showDetails;
    bool showBreakdown;
    protected static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");

    public void Init(Resource.IResource _resource)
    {
        Init(_resource, false, false);
    }
    public void Init(Resource.IResource _resource, bool _showName = false, bool _showDetails = false, bool _showBreakdown = true)
    {
        (resource, showName, showDetails, showBreakdown) = (_resource, _showName, _showDetails, _showBreakdown);
    }

    public override void _Ready()
    {

    }


    public virtual void Update()
    {
        value.Text = (resource.Sum).ToString();
        name.Text = $"{resource.Name} : ";
    }

    public override void _Draw()
    {
        // Assign children
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        details.Visible = showDetails;
        name.Visible = showName;

        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: resource.Type);

        if (Destroy)
        {
            Visible = false;
            QueueFree();
        }
        else
        {
            Update();
        }
    }
    public override Control _MakeCustomTooltip(string forText)
    {
        if (!showBreakdown)
        {
            return null;
        }
        VBoxContainer vbc1 = new();
        ExpandDetails(resource, vbc1);
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

    private void ExpandDetails(Resource.IResource r1, VBoxContainer vbc1)
    {
        // Don't know why, but this is called before ready.
        // Create element representing this.
        UIResource uir = p_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1, true, false, false);
        vbc1.AddChild(uir);

        // If has children, create element to nest inside.
        if (r1.Count > 0)
        {
            HBoxContainer hbc = new();
            VBoxContainer vbc2 = new();
            hbc.AddChild(new VSeparator());
            hbc.AddChild(vbc2);
            foreach (Resource.IResource r2 in ((Resource.RGroup<Resource.IResource>)r1).Adders)
            {
                ExpandDetails(r2, vbc2);
            }
            vbc1.AddChild(hbc);
        }
    }
}
