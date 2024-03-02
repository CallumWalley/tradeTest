using Godot;
using System;

public partial class UIPanelPoolFeatures : UIPanel
{

    public ResourcePool resourcePool;
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    ItemList list;
    ScrollContainer display;
    static readonly PackedScene prefab_UIFeatureSmall = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/Feature/UIFeatureSmall.tscn");

    Feature selected;
    UIList<Feature> vbox;

    public override void _Ready()
    {
        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");
        // vbox = new UIList<Feature>();
        // vbox.Vertical = true;
        // vbox.Init(resourcePool, prefab_UIFeatureSmall);
        // GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/ScrollContainer").AddChild(vbox);
    }

    public override void _Draw()
    {

    }

    public override void Update()
    {
        list.Clear();
        foreach (Feature f in resourcePool)
        {
            list.AddItem(f.Name, f.iconMedium);
        }
        display.GetChild(0);


        base.Update();
    }

}
