using Godot;
using System;

public partial class UIResourceRequest : UIResource
{
    public Resource.IRequestable request;
    public new System.Object GameElement { get { return request; } }

    Color colorBad = new(1, 0, 0);

    public override void Init(System.Object _go)
    {
        Init((Resource.IRequestable)_go);
    }
    public void Init(Resource.IRequestable _request)
    {
        base.Init(_request);
        request = _request;
    }
    // public void Init(Resource.IRequestable _request)
    // {
    //     base.Init(_request);
    //     request = _request;
    // }
    public override void _Draw()
    {
        if (request == null) { return; }
        if (request.State != 0)
        {
            value.Text = $"{request.Sum}/{request.Request.Sum}";
            name.Text = $": {resource.Details}";
            value.AddThemeColorOverride("font_color", colorBad);
        }
        else
        {
            value.RemoveThemeColorOverride("font_color");
            base._Draw();
        }
    }

    // This is a complete implimentation of 'ExpandDetails' in base class. Couldn't figure out proper way.
    private void ExpandDetails(Resource.IRequestable r1, VBoxContainer vbc1)
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
            foreach (Resource.IRequestable r2 in ((Resource.RGroup<Resource.IRequestable>)r1).Adders)
            {
                ExpandDetails(r2, vbc2);
            }
            vbc1.AddChild(hbc);
        }
    }
}