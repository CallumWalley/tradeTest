using Godot;
using System;

public partial class UIResourceStorage : UIResource
{

    public Resource.RStorage storage;
    Color colorBad = new(1, 0, 0);

    static readonly PackedScene p_UIEditValue = GD.Load<PackedScene>("res://GUI/Elements/Input/UIEditValue.tscn");
    public override void Init(System.Object _go)
    {
        Init((Resource.RStorage)_go);
    }
    public void Init(Resource.RStorage _storage)
    {
        base.Init(_storage);
        storage = _storage;
    }

    public override void _Draw()
    {
        if (storage == null) { return; }

        value.Text = string.Format("{0:N1}/{1:N1}", storage.Stock(), storage.Sum);
        name.Text = $": {resource.Details}";
        value.AddThemeColorOverride("font_color", colorBad);
    }
    protected override void ShowDetails()
    {
        // Create details panel if first time.
        if (detailsPopover is null)
        {
            detailsPopover = p_uipopover.Instantiate<UIPopover>();
            detailsPopover.Focus = true;
            detailsPopover.offset = Position;

            Label nl = new();
            // PLACEHOLDER
            nl.Text = string.Format("FULL IN {0}", UnitTypes.TimeSol(10));
            detailsPopover.AddChild(nl);
            GetParent<Control>().AddChild(detailsPopover);
            detailsPopover.GlobalPosition = GlobalPosition;
            detailsPopover.CloseCallback = ClosePopover;
        }
    }
    // Storage is in deficit.
    // if (0 >= storage.Stock())
    // {
    //     value.AddColorOverride("font_color", new Color(1, 0, 0));
    //     detailLabel.Text = string.Format("{0} reserves empty!", storage.Name().ToUpper());
    //     // Storage is overcapacity.
    // }
    // else if (remainder > storage.Sum)
    // {
    //     value.AddColorOverride("font_color", new Color(1, 0, 0));
    //     value.Text = "+";
    //     detailLabel.Text = string.Format("{0} reserves at capacity", storage.Name().ToUpper());
    // }
    // // No storage
    // else if (storage.Sum == 0)
    // {
    //     value.Text = "*";
    //     detailLabel.Text = string.Format("No capibility to store {0}", storage.Name().ToUpper());
    // }
    // // Storage is static.
    // else if (delta.Sum == 0)
    // {
    //     value.Text = String.Format("-");
    //     detailLabel.Text = string.Format("{0} reserves are being maintained", storage.Name().ToUpper());

    // }
    // // Storage is filling.
    // else if (delta.Sum > 0)
    // {
    //     double t = (storage.Sum - stockpile.Sum) / delta.Sum;
    //     value.Text = String.Format("+{0}", t);
    //     detailLabel.Text = string.Format("{0} reserves will reach capacity in {1}", storage.Name().ToUpper(), UnitTypes.TimeSol(t));

    // }
    // // Storage is emptying.
    // else
    // {
    //     double t = remainder / (delta.Sum);
    //     value.Text = String.Format("-{0}", t);
    //     string.Format("{0} reserves will be exhausted in {1}", storage.Name().ToUpper(), UnitTypes.TimeSol(t));
    // }

}

