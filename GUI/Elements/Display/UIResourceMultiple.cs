using Godot;
using System;

public partial class UIResourceMultiple : UIResource
{

    protected override void ShowDetails()
    {

    }

    protected override void HideDetails()
    {

    }

    public override void _Draw()
    {
        if (resource != null)
        {

            value.Text = string.Format("%{0:N2}", resource.Sum);
            name.Text = $": {resource.Details}";
        }
        else
        {
            logger.warning("UI made without object");
        }
    }
}
