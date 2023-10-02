using Godot;
using System;

public partial class UIResource : Control, UIContainers.IListable<Resource.IResource>
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

    public void Init(Resource.IResource _resource)
    {
        Init(_resource, false, false);
    }
    public void Init(Resource.IResource _resource, bool _showName = false, bool _showDetails = false)
    {
        resource = _resource;
        showName = _showName;
        showDetails = _showDetails;
    }

    public override void _Ready()
    {
        // Assign children
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        details.Visible = showDetails;
        name.Visible = showName;

        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: resource.Type);
    }


    public virtual void Update()
    {
        value.Text = (resource.Sum).ToString();
        name.Text = $"{resource.Name} : ";
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
            Update();
        }
    }

}
