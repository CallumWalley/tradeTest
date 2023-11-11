using Godot;
using System;
using System.Collections;

public partial class UIResourceRequest : UIDrawEFrame, Lists.IListable<Resource.IRequestable>
{
    public Resource.IRequestable request;
    public Resource.IRequestable GameElement { get { return request; } }
    public bool Destroy { get; set; } = false;
    public bool ShowName { get; set; } = false;
    public bool ShowDetails { get; set; } = false;
    public bool ShowBreakdown { get; set; } = false;

    // Child components
    public Label value;
    public Label name;
    private Label details;

    protected static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResourceRequest.tscn");

    Color colorBad = new(1, 0, 0);

    public void Init(Resource.IRequestable _request)
    {
        request = _request;
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: request.Type);

        TooltipText = request.Name;
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
            details.Visible = ShowDetails;
            name.Visible = ShowName;
            name.Text = $"{request.Name} : ";

            if (request.State != 0)
            {
                value.Text = $"{request.Sum}/{request.Request}";
                name.Text = $"{request.Name}";
                value.AddThemeColorOverride("font_color", colorBad);
                name.AddThemeColorOverride("font_color", colorBad);
            }
            else
            {
                value.Text = (request.Sum).ToString();
                name.Text = $"{request.Name} : ";
                value.RemoveThemeColorOverride("font_color");
                name.RemoveThemeColorOverride("font_color");
            }
        }
    }

    public override Control _MakeCustomTooltip(string forText)
    {
        if (!ShowBreakdown)
        {
            return null;
        }
        VBoxContainer vbc1 = new();
        ExpandDetails((Resource.RGroupRequests<Resource.IRequestable>)request, vbc1);
        return vbc1;
    }
    private void ExpandDetails(Resource.IRequestable r1, VBoxContainer vbc1)
    {
        UIResourceRequest uir = p_resourceIcon.Instantiate<UIResourceRequest>();
        uir.Init(r1);
        uir.ShowName = true;
        vbc1.AddChild(uir);
    }
    private void ExpandDetails(Resource.RGroupRequests<Resource.IRequestable> r1, VBoxContainer vbc1)
    {
        UIResourceRequest uir = p_resourceIcon.Instantiate<UIResourceRequest>();
        uir.Init(r1);
        uir.ShowName = true;
        vbc1.AddChild(uir);


        HBoxContainer hbc = new();
        VBoxContainer vbc2 = new();
        hbc.AddChild(new VSeparator());
        hbc.AddChild(vbc2);
        foreach (Resource.IRequestable r2 in ((Resource.RGroup<Resource.IRequestable>)r1))
        {
            ExpandDetails(r2, vbc2);
        }
        vbc1.AddChild(hbc);
    }


}