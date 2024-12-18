using Godot;
using System;
using System.Linq;
namespace Game;

/// <summary>
/// Displays the ist of features in a location.
/// </summary>
public partial class UIPanelPositionFeatures : UIPanel, UIInterfaces.IEFrameUpdatable
{

    public Entities.IPosition position;
    Label nameLabel;
    Label adjLabel;
    Label altNameLabel;
    [Export]
    ItemList list;
    [Export]
    ScrollContainer display;
    [Export]
    ScrollContainer displayEmpty;

    [Export]
    UIButton buttonAddFeature;

    static readonly PackedScene prefab_UIPanelFeatureFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Feature/UIPanelFeatureFull.tscn");
    static readonly PackedScene prefab_UIPanelDomainFeatureTemplate = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/FeatureTemplate/UIWindowDomainFeatureTemplate.tscn");


    Entities.IFeature selected;
    int selectedIndex = 0;

    UIList<FeatureBase> vbox;
    public override void _Ready()
    {

        base._Ready();

        list.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));
        buttonAddFeature.Connect("pressed", new Callable(this, "OnButtonAddFeaturePressed"));
        if (position.Any())
        {
            selected = position[0];
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
        selected = position[selectedIndex];
        DrawDisplay();
    }

    public void OnButtonAddFeaturePressed()
    {
        UIWindow wdfpw = GetNodeOrNull<UIWindow>($"{position}_build_dialouge");


        if (wdfpw == null)
        {
            wdfpw = prefab_UIPanelDomainFeatureTemplate.Instantiate<UIWindow>();
            UIPanelDomainFeatureTemplate pdft = wdfpw.GetNode<UIPanelDomainFeatureTemplate>("UIPanelDomainFeatureTemplate");
            pdft.Name = $"{position}_build_dialouge";
            pdft.Site = position;
            AddChild(wdfpw);
        }
        else
        {
            wdfpw.Visible = !wdfpw.Visible;
        }
    }

    void DrawDisplay()
    {
        if (position.Count() < 1)
        {
            displayEmpty.Visible = true;
            display.Visible = false;
        }
        else
        {
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
                selected ??= (FeatureBase)position.First();
                UIPanelFeatureFull uipff = prefab_UIPanelFeatureFull.Instantiate<UIPanelFeatureFull>();
                uipff.feature = selected;
                display.AddChild(uipff);
            }
            display.GetChild<UIPanelFeatureFull>(0).OnEFrameUpdate();
        }
    }

    public void UpdateElements()
    {
        list.Clear();
        foreach (Node f in position)
        {
            if (f is FeatureBase)
            {
                list.AddItem(((FeatureBase)f).Name, ((FeatureBase)f).IconMedium);
            }
        }
        if (list.GetChildCount() > selectedIndex)
        {
            list.Select(0);
        }
        else
        {
            list.Select(selectedIndex);
        }

    }

    public override void OnEFrameUpdate()
    {
        base.OnEFrameUpdate();
        // If visible, and there are features, update the list to reflect reality.
        if (IsVisibleInTree() && position.Count() > 0)
        {
            UpdateElements();
            DrawDisplay();
        }
    }
}
