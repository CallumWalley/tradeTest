using Godot;
using System;

/// <summary>
/// Displays the ist of features in a location.
/// </summary>
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
    int selectedIndex = 0;

    UIList<Features.FeatureBase> vbox;

    public override void _Ready()
    {

        base._Ready();
        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");

        GetNode<UIWindowPoolFeaturePlan>("WindowPoolFeaturePlan").resourcePool = resourcePool;

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));

        if (resourcePool.GetChildCount() > 0)
        {
            selected = resourcePool.GetChild<Features.FeatureBase>(0);
        }
        else
        {
            selected = null;
        }
    }

        public void OnItemListItemSelected(int i)
    {   
        selectedIndex = i;
        selected = resourcePool.GetChild<Features.FeatureBase>(selectedIndex);
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
                    display.RemoveChild(c);
                }
                selected ??= resourcePool.GetChild<Features.FeatureBase>(0);
                UIPanelFeatureFull uipff = prefab_UIPanelFeatureFull.Instantiate<UIPanelFeatureFull>();
                uipff.feature = selected;
                display.AddChild(uipff);
            }
            display.GetChild<UIPanelFeatureFull>(0).OnEFrameUpdate();
        }
    }
    public override void OnEFrameUpdate()
    {
        base.OnEFrameUpdate();
        // If visible, and there are features, update the list to reflect reality.
        if (Visible && resourcePool.GetChildCount() > 0){
            list.Clear();
            foreach (Node f in resourcePool)
            {
                if (f is Features.FeatureBase){
                    list.AddItem(((Features.FeatureBase)f).Name, ((Features.FeatureBase)f).iconMedium);
                }
            }
            list.Select(selectedIndex);
            DrawDisplay();
        }
    }
}
