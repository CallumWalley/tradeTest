using Godot;
using System;

public partial class UIInputType : UIResource
{
    // modification for inputType class (e.g, show requested and delivered)
    public Resource.BaseRequest inputType;

    public void Init(Resource.BaseRequest _inputType)
    {
        inputType = _inputType;
        base.Init(inputType.Response);

    }

    public override void _Draw()
    {
        if (inputType != null)
        {
            value.Text = String.Format("{0:N1}/{1:N1}", inputType.Response.Sum, inputType.Request.Sum);
        }
        else
        {
            logger.warning("UI made without object");
        }
    }
}
