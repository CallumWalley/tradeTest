using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// List of all buildable structures.
/// Used in template menu and build menu.
/// </summary>
public partial class UIPanelFeatureFactoryList : UIPanel, UIInterfaces.IEFrameUpdatable
{
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    ItemList list;
    ScrollContainer display;
	List<Features.BasicFactory> featureList = new();
    static readonly PackedScene prefab_UIPanelFeatureFactoryFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelFeatureFactoryFull.tscn");

    public Features.BasicFactory selected;
    int selectedIndex = 0;
    UIList<Features.Basic> vbox;
    
    public override void _Ready()
    {
        base._Ready();
        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));
    }

    public void OnItemListItemSelected(int i)
    {   
        selectedIndex = i;
        selected = (Features.BasicFactory)featureList[selectedIndex];
        DrawDisplay();
    }

    void DrawDisplay()
    {
        if (featureList.Count > 0)
        {
            // If selected feature changed, or none.
            if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureFactoryFull>(0).feature != selected)
            {
                foreach (Control c in display.GetChildren().Cast<Control>())
                {
                    c.Visible = false;
                    c.QueueFree();
                    display.RemoveChild(c);
                }
                selected ??= (Features.BasicFactory)featureList[0];
                UIPanelFeatureFactoryFull uipff = prefab_UIPanelFeatureFactoryFull.Instantiate<UIPanelFeatureFactoryFull>();
                uipff.feature = selected;
                display.AddChild(uipff);
            }
            display.GetChild<UIPanelFeatureFactoryFull>(0).OnEFrameUpdate();
        }
    }
    public override void OnEFrameUpdate()
    {
        // If visible, and there are features, update the list to reflect reality.
        base.OnEFrameUpdate();
        featureList = GetNode<Features>("/root/Features").ToList(); //.Where(x => x.IsBuildable()).ToList();
        if (IsVisibleInTree() && featureList.Count > 0){
            list.Clear();
            foreach (Node f in featureList)
            {
                list.AddItem(((Features.BasicFactory)f).Name, ((Features.BasicFactory)f).iconMedium);
            }
            list.Select(selectedIndex);
            DrawDisplay();
        }
    } 
}
