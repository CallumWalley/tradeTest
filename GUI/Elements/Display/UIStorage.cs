using Godot;
using System;

public partial class UIStorage : UIElement, UIContainers.IListable
{

    public Resource.RStorage storage;

    public Control Control { get { return this; } }
    public System.Object GameElement { get { return storage; } }


    static readonly PackedScene p_UIEditValue = GD.Load<PackedScene>("res://GUI/Elements/Input/UIEditValue.tscn");

    UIPopover detailPopover;
    Label detailLabel;
    Label value;
    public void Init(System.Object _go)
    {
        Init((Resource.RStorage)_go);
    }
    public void Init(Resource.RStorage _storage)
    {
        storage = _storage;

        if (storage != null)
        {
            ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(resourceCode: storage.Type());
        }
    }

    public override void _Ready()
    {
        base._Ready();
        value = GetNode<Label>("Value");
        detailPopover = GetNode<UIPopover>("Details");
        detailLabel = detailPopover.GetNode<Label>("Tooltip");
    }

    protected override void ShowDetails()
    {
        base.ShowDetails();
        detailPopover.Show();
    }

    protected override void HideDetails()
    {
        base.HideDetails();
        detailPopover.Hide();
    }

    public override void _Draw()
    {
        if (storage == null) { return; }

        base._Draw();
        // Set text color
        value.RemoveThemeColorOverride("font_color");
        value.Text = string.Format("{0:N1}/{1:N1}", storage.Stock(), storage.Sum());

        // Storage is in deficit.
        // if (0 >= storage.Stock())
        // {
        //     value.AddColorOverride("font_color", new Color(1, 0, 0));
        //     detailLabel.Text = string.Format("{0} reserves empty!", storage.Name().ToUpper());
        //     // Storage is overcapacity.
        // }
        // else if (remainder > storage.Sum())
        // {
        //     value.AddColorOverride("font_color", new Color(1, 0, 0));
        //     value.Text = "+";
        //     detailLabel.Text = string.Format("{0} reserves at capacity", storage.Name().ToUpper());
        // }
        // // No storage
        // else if (storage.Sum() == 0)
        // {
        //     value.Text = "*";
        //     detailLabel.Text = string.Format("No capibility to store {0}", storage.Name().ToUpper());
        // }
        // // Storage is static.
        // else if (delta.Sum() == 0)
        // {
        //     value.Text = String.Format("-");
        //     detailLabel.Text = string.Format("{0} reserves are being maintained", storage.Name().ToUpper());

        // }
        // // Storage is filling.
        // else if (delta.Sum() > 0)
        // {
        //     double t = (storage.Sum() - stockpile.Sum()) / delta.Sum();
        //     value.Text = String.Format("+{0}", t);
        //     detailLabel.Text = string.Format("{0} reserves will reach capacity in {1}", storage.Name().ToUpper(), UnitTypes.TimeSol(t));

        // }
        // // Storage is emptying.
        // else
        // {
        //     double t = remainder / (delta.Sum());
        //     value.Text = String.Format("-{0}", t);
        //     string.Format("{0} reserves will be exhausted in {1}", storage.Name().ToUpper(), UnitTypes.TimeSol(t));
        // }

    }
}
