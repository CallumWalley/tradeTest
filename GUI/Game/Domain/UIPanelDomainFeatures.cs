using Godot;
using System;
using System.Linq;
namespace Game;

/// <summary>
/// Displays the ist of features in a location.
/// </summary>
public partial class UIPanelDomainFeatures : UIPanel, UIInterfaces.IEFrameUpdatable
{

    public Domain domain;
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    ItemList list;
    ScrollContainer display;
    ScrollContainer displayEmpty;

    static readonly PackedScene prefab_UIPanelFeatureFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Feature/UIPanelFeatureFull.tscn");

    FeatureBase selected;
    int selectedIndex = 0;

    UIList<FeatureBase> vbox;

    public override void _Ready()
    {

        base._Ready();
        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");
        displayEmpty = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/DisplayEmpty");


        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));
    }

    public void Init()
    {
        if (domain.GetChildCount() > 0)
        {
            selected = domain[0];
        }
        else
        {
            selected = null;
        }
        UpdateElements();
        DrawDisplay();
    }

    public void OnItemListItemSelected(int i)
    {
        selectedIndex = i;
        selected = domain[selectedIndex];
        DrawDisplay();
    }


    void DrawDisplay()
    {
        if (domain.GetNodeOrNull("Features") == null)
        {
            displayEmpty.Visible = true;
            display.Visible = false;
            return;
        }
        displayEmpty.Visible = false;
        display.Visible = true;
        // {
        // If selected feature changed, or none.
        if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureFull>(0).feature != selected)
        {
            foreach (Control c in display.GetChildren().Cast<Control>())
            {
                c.Visible = false;
                c.QueueFree();
                display.RemoveChild(c);
            }
            selected ??= domain.First();
            UIPanelFeatureFull uipff = prefab_UIPanelFeatureFull.Instantiate<UIPanelFeatureFull>();
            uipff.feature = selected;
            display.AddChild(uipff);
        }
        display.GetChild<UIPanelFeatureFull>(0).OnEFrameUpdate();
        // }
    }

    public void UpdateElements()
    {
        if (domain.GetNodeOrNull("Features") == null) { return; }
        list.Clear();
        foreach (Node f in domain)
        {
            if (f is FeatureBase)
            {
                list.AddItem(((FeatureBase)f).Name, ((FeatureBase)f).iconMedium);
            }
        }
        list.Select(selectedIndex);
    }

    public override void OnEFrameUpdate()
    {
        base.OnEFrameUpdate();
        // If visible, and there are features, update the list to reflect reality.
        // if (IsVisibleInTree() && resourcePool.GetChildCount() > 0)
        // {
        UpdateElements();
        DrawDisplay();
        // }
    }
}
