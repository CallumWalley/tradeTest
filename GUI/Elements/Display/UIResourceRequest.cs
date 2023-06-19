using Godot;
using System;

public partial class UIResourceRequest : UIResource
{
    public Resource.RRequestBase request;
    public new System.Object GameElement { get { return request; } }

    Color colorBad = new(1, 0, 0);

    public override void Init(System.Object _go)
    {
        Init((Resource.RRequestBase)_go);
    }
    public void Init(Resource.RRequestBase _request)
    {
        base.Init(_request);
        request = _request;
    }

    public override void _Draw()
    {
        if (request == null) { return; }
        if (request.State != 0)
        {
            value.Text = $"{request.Response.Sum}/{request.Request.Sum}";
            name.Text = $": {resource.Details}";
            value.AddThemeColorOverride("font_color", colorBad);
        }
        else
        {
            value.RemoveThemeColorOverride("font_color");
            base._Draw();
        }
    }
}