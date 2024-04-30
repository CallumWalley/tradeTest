using Godot;
using System;
using System.Linq;

// List of all buildable structures.
public partial class UIPanelBuildTemplate : UIPanel, UIInterfaces.IEFrameUpdatable
{
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    ItemList list;
    ScrollContainer display;
	Features featureList;
    static readonly PackedScene prefab_UIFeatureSmall = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists/Listables/Feature/UIFeatureSmall.tscn");
    static readonly PackedScene prefab_UIPanelFeatureFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelFeatureFull.tscn");

    Features.FeatureBase selected;
    int selectedIndex = 0;
    UIList<Features.FeatureBase> vbox;

    public override void _Ready()
    {
        base._Ready();
		featureList = GetNode<Features>("/root/Features");
        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));

        // vbox = new UIList<Feature>();
        // vbox.Vertical = true;
        // vbox.Init(resourcePool, prefab_UIFeatureSmall);
        // GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/ScrollContainer").AddChild(vbox);
    }

    public void OnItemListItemSelected(int i)
    {   
        selectedIndex = i;
        selected = featureList.GetChild<Features.FeatureBase>(selectedIndex);
        DrawDisplay();
    }

    void DrawDisplay()
    {
        if (featureList.GetChildCount() > 0)
        {
            // If selected feature changed, or none.
            if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureFull>(0).feature != selected)
            {
                foreach (Control c in display.GetChildren().Cast<Control>())
                {
                    c.Visible = false;
                    c.QueueFree();
                    display.RemoveChild(c);
                }
                selected ??= featureList.GetChild<Features.FeatureBase>(0);
                UIPanelFeatureFull uipff = prefab_UIPanelFeatureFull.Instantiate<UIPanelFeatureFull>();
                uipff.feature = selected;
                display.AddChild(uipff);
            }
            display.GetChild<UIPanelFeatureFull>(0).OnEFrameUpdate();
        }
    }
    public override void OnEFrameUpdate()
    {
        // If visible, and there are features, update the list to reflect reality.
        base.OnEFrameUpdate();
        if (Visible && featureList.GetChildCount() > 0){
            list.Clear();
            foreach (Node f in featureList.GetChildren())
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
