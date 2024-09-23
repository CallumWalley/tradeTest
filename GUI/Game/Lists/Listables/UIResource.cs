using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
// [Tool]
public partial class UIResource : Control, Lists.IListable<Resource.IResource>
{
    public Resource.IResource resource;
    public Resource.IResource GameElement { get { return resource; } }

    protected Global global;
    public bool Destroy { get; set; } = false;
    [Export]
    public bool ShowName { get; set; } = false;
    [Export]

    public bool ShowDetails { get; set; } = false;
    [Export]

    public bool ShowBreakdown { get; set; } = false;

    // For interpolation
    public double resourceSumLast = 0;
    public double resourceSumThis = 0;
    public double resourceRequestThis = 0;
    public int resourceStateThis = 0;

    protected double displayValue;
    Color colorBad = new(1, 0, 0);

    // For use in editor only.
    // [Export(PropertyHint.Flags, "Water:1,Energy:2,Minerals:3")]
    // public int _resource;

    // Child components
    public Label value;
    public Label name;
    protected Label details;

    protected static readonly PackedScene prefab_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/UIResource.tscn");
    //protected static readonly PackedScene prefab_resourceTooltip = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Resource.tscn");


    public void Init(Resource.IResource _resource)
    {
        resource = _resource;
    }

    public override void _Ready()
    {
        base._Ready();
        value = GetNode<Label>("Value");
        name = GetNode<Label>("Name");
        // details = GetNode<Label>("Details");
        global = GetNode<Global>("/root/Global");
        Update();
    }

    public override void _Draw()
    {
        if (Destroy)
        {
            Visible = false;
            QueueFree();
            return;
        }

        // Note to self. interpolation is harder than first seems.
        // bool interpolate = (bool)PlayerConfig.config.GetValue("interface", "stepNumericalInterpolation");
        // if (interpolate)
        // {
        //     double t = (global.deltaEFrame / global.timePerEframe);
        //     displayValue = (t * (resourceSumLast - resourceSumThis)) + resourceSumLast;
        // }
        // else
        // {
        //     displayValue = resourceSumThis;
        // }
        displayValue = resourceSumThis;
        // hide if null.
        //Visible = !(resource.Count < 1 && Mathf.Abs(resource.Sum) < 0.1);
        // details.Visible = ShowDetails;
        name.Visible = ShowName;
        TooltipText = resource.Name;
        name.Text = $"{resource.Name}";
        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon((resource != null) ? resource.Type : 0);

        if (resourceStateThis > 0)
        {
            value.Text = string.Format("{0:F1}/{1:F1}", displayValue, resourceRequestThis);

            value.AddThemeColorOverride("font_color", colorBad);
            name.AddThemeColorOverride("font_color", colorBad);
        }
        else
        {
            value.RemoveThemeColorOverride("font_color");
            name.RemoveThemeColorOverride("font_color");
            value.Text = string.Format(resource.ValueFormat, displayValue);
        }
    }

    public void Update()
    {

        resourceSumLast = resourceSumThis;
        resourceSumThis = resource.Sum;
        resourceRequestThis = resource.Request;
        resourceStateThis = resource.State;
        QueueRedraw();
    }


    public override Control _MakeCustomTooltip(string forText)
    {
        if (!ShowBreakdown) { return null; }
        VBoxContainer vbc1 = new();
        vbc1.Alignment = BoxContainer.AlignmentMode.End;
        ExpandDetails(resource, vbc1);
        return vbc1;
    }

    // Recursivly iterate over elements.
    private void ExpandDetails(Resource.IResource r1, VBoxContainer vbc1, int level = 0)
    {
        int maxDescend = 1;

        UIResource uir = prefab_resourceIcon.Instantiate<UIResource>();
        uir.Init(r1);
        uir.ShowName = true;
        uir.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        vbc1.AddChild(uir);
        if (r1 is Resource.IResourceGroup<Resource.IResource> && level < maxDescend)
        {
            HBoxContainer hbc = new();
            VBoxContainer vbc2 = new();
            hbc.AddChild(new VSeparator());
            hbc.AddChild(vbc2);
            foreach (Resource.IResource r2 in (Resource.IResourceGroup<Resource.IResource>)r1)
            {
                ExpandDetails(r2, vbc2, level + 1);
            }
            vbc1.AddChild(hbc);
        }
    }
}
