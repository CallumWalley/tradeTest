using Godot;
using System;

public partial class UIResource : UIElement, UIContainers.IListable
{
    public Resource.IResource resource;

    public Control Control { get { return this; } }
    public System.Object GameElement { get { return resource; } }

    // public bool NameVisible
    // {
    // 	get
    // 	{
    // 		if (name != null)
    // 		{
    // 			return name.Visible;
    // 		}
    // 		else
    // 		{
    // 			GD.Print("This resrouce is null for some reason");
    // 			return false;
    // 		}
    // 	}
    // 	set
    // 	{
    // 		if (name != null)
    // 		{
    // 			name.Visible = value;
    // 		}
    // 		else
    // 		{
    // 			GD.Print("This resrouce is null for some reason");
    // 		}
    // 	}
    // }

    public bool NameVisible { get; set; } = false;
    //     { get { return name.Visible; }
    // set { name.Visible = value; } }

    // Prefabs
    protected static readonly PackedScene p_uipopover = GD.Load<PackedScene>("res://GUI/Elements/UIPopover.tscn");
    protected static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");

    // Child components
    protected Control details;
    protected Label value;
    protected Label name;
    protected UIPopover detailsPopover;
    public bool showDetails = true;

    public virtual void Init(System.Object _go)
    {
        Init((Resource.IResource)_go);
    }
    // public void Init(Resource.IResource _resource)
    // {
    //     Init(_resource);
    // }
    public void Init(Resource.IResource _resource, bool _showDetails = true)
    {
        showDetails = _showDetails;
        if (showDetails)
        {
            ShowDetailsCallback = ShowDetails;
        }

        resource = _resource;
        if (resource != null)
        {
            ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: resource.Type);
        }
        else
        {
            logger.warning("UI made without object");
        }
    }

    public override void _Ready()
    {
        if (showDetails)
        {
            base._Ready();
        }

        // Assign children
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        name.Visible = NameVisible;
        // details assigned on first call of ShowDetails
    }

    private void ExpandDetails(Resource.IResource r1, VBoxContainer vbc1)
    {
        // Create element representing this.
        UIResource uir = p_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1, false);
        uir.NameVisible = true;
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

    protected virtual void ShowDetails()
    {
        // Create details panel if first time.
        if (detailsPopover is null)
        {
            detailsPopover = p_uipopover.Instantiate<UIPopover>();
            detailsPopover.Focus = true;
            detailsPopover.offset = Position;
            VBoxContainer vbc = new();
            ExpandDetails(resource, vbc);

            detailsPopover.AddChild(vbc);

            GetParent<Control>().AddChild(detailsPopover);
            detailsPopover.GlobalPosition = GlobalPosition;
            detailsPopover.CloseCallback = ClosePopover;
        }
    }

    protected void ClosePopover()
    {
        detailsPopover.QueueFree();
        detailsPopover = null;
    }

    public override void _Draw()
    {
        if (resource != null)
        {
            value.Text = (resource.Sum).ToString();
            name.Text = $": {resource.Details}";
        }
        else
        {
            logger.warning("UI made without object");
        }
    }
}
