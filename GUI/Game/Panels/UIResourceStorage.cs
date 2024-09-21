using Godot;
using System;
using System.Collections.Generic;

public partial class UIResourceStorage : Control
{
    public Resource.RStatic resource;
    public Resource.Ledger.EntryAccrul entry;

    Global global;
    public bool Destroy { get; set; } = false;

    double resourceSumLast = 0;
    double resourceStoreThis;
    Color colorBad = new(1, 0, 0);
    protected Control details;
    public Label value;
    public Label name;
    public Resource.RStatic GameElement { get { return resource; } }
    static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");

    public void Init(Resource.Ledger.EntryAccrul _entry)
    {
        entry = _entry;
        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon(entry.Type);
        TooltipText = Resource.Name(entry.Type);
    }

    public override void _Ready()
    {
        // Assign children
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        details = GetNode<Label>("Details");
        global = GetNode<Global>("/root/Global");
    }

    public override void _Process(double _delta)
    {

        //value.Text = string.Format("{0:P0}", entry.Stored.Sum / entry.Capacity.Sum);
        //double displayValue = ((global.deltaEFrame / global.timePerEframe) * (resourceSumLast - resourceStoreThis)) + resourceSumLast;
        double displayValue = resourceStoreThis;
        value.Text = string.Format("{0:F0}", displayValue);
        name.Text = $": Storage";
        // Storage is in deficit.
        if (0 <= resourceStoreThis)
        {
            value.AddThemeColorOverride("font_color", new Color(1, 0, 0));
            // detailLabel.Text = string.Format("{0} reserves empty!", storage.Name().ToUpper());
            // Storage is overcapacity.
        }
        QueueRedraw();
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

    public void Update()
    {
        resourceSumLast = resourceStoreThis;
        resourceStoreThis = entry.Stored.Sum;
    }

    public override void _Draw()
    {
        if (Destroy)
        {
            Visible = false;
            QueueFree();
        }
    }

    public override Control _MakeCustomTooltip(string forText)
    {
        HBoxContainer hbc1 = new();

        TextureRect icon = new();
        Label text = new();

        icon.Texture = Resource.Icon(entry.Type);
        text.Text = $"{entry.Stored.Sum}/{entry.Stored.Request}";

        hbc1.AddChild(icon);
        hbc1.AddChild(text);

        // hbc1.AddChild(uir1);
        // hbc1.AddChild(new VSeparator());
        // hbc1.AddChild(uir2);

        return hbc1;
    }
    //value.Text = string.Format("{0:F0}/{1:F0}", storage.Value.Capacity, storage.Value.Level);

    //value.AddThemeColorOverride("font_color", colorBad);
}
// public override void _Draw()
// {
//     if (Destroy)
//     {
//         Visible = false;
//         QueueFree();
//     }
//     else
//     {
//         Update();
//     }
// }
// protected override void ShowDetails()
// {
//     // Create details panel if first time.
//     if (storeDetailsPopover is null)
//     {
//         storeDetailsPopover = p_uipopover.Instantiate<UIPopover>();
//         storeDetailsPopover.Focus = true;
//         storeDetailsPopover.offset = Position;
//         HBoxContainer vbc = new();

//         storeDetailsPopover.AddChild(vbc);

//         GetParent<Control>().AddChild(storeDetailsPopover);
//         storeDetailsPopover.GlobalPosition = GlobalPosition;
//         storeDetailsPopover.CloseCallback = ClosePopover;
//     }
// }
// protected override void ShowDetails()
// {
//     // Create details panel if first time.
//     if (detailsPopover is null)
//     {
//         detailsPopover = p_uipopover.Instantiate<UIPopover>();
//         detailsPopover.Focus = true;
//         detailsPopover.offset = Position;

//         Label nl = new();
//         // PLACEHOLDER
//         nl.Text = string.Format("FULL IN {0}", UnitTypes.TimeSol(10));
//         detailsPopover.AddChild(nl);
//         GetParent<Control>().AddChild(detailsPopover);
//         detailsPopover.GlobalPosition = GlobalPosition;
//         detailsPopover.CloseCallback = ClosePopover;
//     }
// }

