using Godot;
using System;
using System.Collections;

public partial class UIResourceRequest : Control, UIList<Resource.IRequestable>.IListable<Resource.IRequestable>
{
    // Prefabs
    public Resource.IRequestable GameElement { get { return request; } }
    public Resource.IRequestable request;
    public bool Destroy { get; set; } = false;

    // Child components
    public Label value;
    public Label name;
    private Label details;

    bool showName;
    bool showDetails;
    Color colorBad = new(1, 0, 0);

    public void Init(Resource.IRequestable _request)
    {
        Init(_request, false, false);
    }
    public void Init(Resource.IRequestable _request, bool _showName = false, bool _showDetails = false)
    {
        request = _request;
        showName = _showName;
    }
    public override void _Ready()
    {
        // Assign children
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        details.Visible = showDetails;
        name.Visible = showName;

        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: request.Type);
    }

    public void Update()
    {
        if (request.State != 0)
        {
            value.Text = $"{request.Sum}/{request.Request.Sum}";
            name.Text = $": {request.Details}";
            value.AddThemeColorOverride("font_color", colorBad);
            name.AddThemeColorOverride("font_color", colorBad);

        }
        else
        {
            value.RemoveThemeColorOverride("font_color");
            base._Draw();
        }
    }

}