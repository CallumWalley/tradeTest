using Godot;
using System;

public partial class UIPanelPoolFeatures : UIPanel, UIInterfaces.IEFrameUpdatable
{

    public ResourcePool resourcePool;
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    ItemList list;
    ScrollContainer display;
    static readonly PackedScene prefab_UIFeatureSmall = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/Feature/UIFeatureSmall.tscn");
    static readonly PackedScene prefab_UIPanelFeatureFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelFeatureFull.tscn");

    Features.FeatureBase selected;
    UIList<Features.FeatureBase> vbox;

    public override void _Ready()
    {

        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));

        if (resourcePool.GetChildCount() > 0)
        {
            selected = resourcePool.GetChild<Features.FeatureBase>(0);
        }
        else
        {
            selected = null;
        }

        // vbox = new UIList<Feature>();
        // vbox.Vertical = true;
        // vbox.Init(resourcePool, prefab_UIFeatureSmall);
        // GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/ScrollContainer").AddChild(vbox);
    }

    public void OnItemListItemSelected(int i)
    {
        selected = resourcePool.GetChild<Features.FeatureBase>(i);
        DrawDisplay();
    }



    void DrawDisplay()
    {
        if (resourcePool.GetChildCount() > 0)
        {
            // If selected feature changed, or none.
            if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureFull>(0).feature != selected)
            {
                foreach (Control c in display.GetChildren())
                {
                    c.Visible = false;
                    c.QueueFree();
                }
                UIPanelFeatureFull uipff = prefab_UIPanelFeatureFull.Instantiate<UIPanelFeatureFull>();
                uipff.feature = selected;
                display.AddChild(uipff);
            }
            display.GetChild<UIPanelFeatureFull>(0).OnEFrameUpdate();
        }
    }
    public override void OnEFrameUpdate()
    {
        list.Clear();
        foreach (Features.FeatureBase f in resourcePool)
        {
            list.AddItem(f.Name, f.iconMedium);
        }
        DrawDisplay();

    }
}
