using Godot;
using System;

public class UIResource : UIElement, UIList.IListable
{
    public Resource resource;
    public Resource request;

    public Control Control { get { return this; } }
    public System.Object GameElement { get { return resource; } }

    public bool NameVisible
    {
        get
        {
            if (name != null)
            {
                return name.Visible;
            }
            else
            {
                GD.Print("This resrouce is null for some reason");
                return false;
            }
        }
        set
        {
            if (name != null)
            {
                name.Visible = value;
            }
            else
            {
                GD.Print("This resrouce is null for some reason");
            }
        }
    }

    // Prefabs
    static readonly PackedScene p_UIResourceBreakdown = GD.Load<PackedScene>("res://GUI/Components/UIResourceBreakdown.tscn");
    static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");

    // Child components
    protected Control details;
    protected Label value;
    protected Label name;
    bool showDetails = true;

    public void Init(Resource _resource, Resource _request)
    {
        request = _request;
        Init(_resource);
    }

    public void Init(System.Object _go)
    {
        Init((Resource)_go);
    }
    public void Init(Resource _resource, bool _showDetails = true)
    {
        showDetails = _showDetails;
        resource = _resource;
        if (resource != null)
        {
            ((TextureRect)GetNode("Icon")).Texture = Resources.Icon(resourceCode: resource.Type);
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



        // details assigned on first call of ShowDetails
    }

    void CreateDetails()
    {
        details = p_UIResourceBreakdown.Instance<UIResourceBreakdown>();
        ((UIResourceBreakdown)details).Init(resource);
        AddChild(details);
        // Add details panel
        // if (resource is ResourceAgr){
        // 	UIResourceList rl = p_UIResourceList.Instance<UIResourceList>();
        // 	rl.Init(((ResourceAgr)resource).add);
        // 	details.GetNode<Control>("PanelContainer").AddChild(rl);
        // }else{
        // 	Label label = new Label();
        // 	label.Text="Details"; //=resource.Name;
        // 	details.GetNode<Control>("PanelContainer").AddChild(label);
        // }
    }

    protected override void ShowDetails()
    {
        // Create details panel if first time.
        if (details is null)
        {
            CreateDetails();
        }
        base.ShowDetails();
        details.RectGlobalPosition = RectGlobalPosition + new Vector2(2, 0);
        details.Show();
    }

    protected override void HideDetails()
    {
        base.HideDetails();
        details.Hide();
    }

    public override void _Draw()
    {
        if (resource != null)
        {
            if (request != null && request.Sum != resource.Sum)
            {
                value.Text = $"{resource.Sum}/{request.Sum}";
            }
            else
            {
                value.Text = (resource.Sum).ToString();
            }
            name.Text = $": {resource.Name}";
        }
        else
        {
            logger.warning("UI made without object");
        }
    }
}
