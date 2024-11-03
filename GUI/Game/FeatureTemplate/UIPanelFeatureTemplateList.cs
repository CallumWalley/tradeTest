using Godot;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game;

/// <summary>
/// List of all buildable structures.
/// Used in template menu and build menu.
/// </summary>
public partial class UIPanelFeatureTemplateList : UIPanel
{
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    public ItemList list;
    ScrollContainer display;

    /// <summary>
    /// If this is set, list will be filtered to valid options. TODO make this nicer.
    /// </summary>
    public Entities.IPosition baseDomain;
    public List<PlayerFeatureTemplate> featureList = new List<PlayerFeatureTemplate>();
    static readonly PackedScene prefab_UIPanelFeatureFactoryFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/FeatureTemplate/UIPanelFeatureTemplateFull.tscn");
    Player player;

    public PlayerFeatureTemplate selected;
    int selectedIndex = 0;
    UIList<FeatureBase> vbox;

    public override void _Ready()
    {
        base._Ready();
        player = GetNode<Player>("/root/Global/Player");

        list = GetNode<ItemList>("VBoxContainer/HSplitContainer/ScrollContainer/VBoxContainer/ItemList");
        display = GetNode<ScrollContainer>("VBoxContainer/HSplitContainer/Display");

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));
        list.Connect("visibility_changed", new Callable(this, "OnItemListVisibilityChanged"));

        UpdateElements();
        OnItemListItemSelected(0);
    }

    public void OnItemListVisibilityChanged()
    {
        UpdateElements();
        DrawDisplay();
    }

    public void OnItemListItemSelected(int i)
    {
        selectedIndex = i;
        if (selectedIndex >= featureList.Count) { return; }
        selected = (PlayerFeatureTemplate)featureList[selectedIndex];
        DrawDisplay();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

    }
    void DrawDisplay()
    {
        // Draws the UI. Does not refresh elements.
        if (featureList.Count < 1) { return; }

        // If selected feature changed, or none.
        if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureTemplateFull>(0).template != selected)
        {
            foreach (Control c in display.GetChildren().Cast<Control>())
            {
                c.Visible = false;
                c.QueueFree();
                display.RemoveChild(c);
            }
            selected ??= (PlayerFeatureTemplate)featureList[0];
            UIPanelFeatureTemplateFull uipff = prefab_UIPanelFeatureFactoryFull.Instantiate<UIPanelFeatureTemplateFull>();
            uipff.template = selected;
            display.AddChild(uipff);
        }
        display.GetChild<UIPanelFeatureTemplateFull>(0).OnEFrameUpdate();
    }
    public void UpdateElements()
    {
        // Updates list elements. Does not draw them.
        featureList = GetFeatureList().ToList();

        if (featureList.Count < 1) { return; }

        list.Clear();
        foreach (Node f in featureList)
        {
            list.AddItem(((PlayerFeatureTemplate)f).Name, ((PlayerFeatureTemplate)f).Feature.IconMedium);
        }
        list.Select(selectedIndex);
    }
    public override void OnEFrameUpdate()
    {
        // If visible, and there are features, update the list to reflect reality.
        base.OnEFrameUpdate();

        if (IsVisibleInTree() && featureList.Count > 0)
        {
            UpdateElements();
            DrawDisplay();
        }
    }
    /// <summary>
    /// Unless overwritten, this will return all templates.
    /// </summary>
    /// <returns></returns>

    public virtual IEnumerable<PlayerFeatureTemplate> GetFeatureList()
    {
        if (baseDomain != null)
        {
            return player.featureTemplates.GetValid(baseDomain);
        }
        else
        {
            return player.featureTemplates;
        }
    }
}
