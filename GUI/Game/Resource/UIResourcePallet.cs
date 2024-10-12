using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class UIResourcePallet : HBoxContainer
{

    TextureButton addButton;
    Popup popup;
    UIResourceSelect uIResourceSelect;

    UIList<Resource.IResource> uIList;
    // UIList<Resource.RStatic> uIList;
    public Resource.RDictStatic rdict = new Resource.RDictStatic();
    static readonly PackedScene prefab_resourceEdit = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Resource/UIResourceEdit.tscn");

    // static readonly PackedScene prefab_resourceEdit = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Lists//UIResourceEdit.tscn");

    public override void _Ready()
    {
        base._Ready();

        addButton = GetNode<TextureButton>("Add");
        popup = GetNode<Popup>("Popup");
        uIResourceSelect = popup.GetNode<UIResourceSelect>("UiResourceSelect");

        addButton.Connect("button_up", new Callable(this, "OnAddButtonUp"));
        uIResourceSelect.Connect("OnClick", new Callable(this, "OnResourceSelectItemClicked"));
        uIResourceSelect.Connect("OnToggler", new Callable(this, "OnResourceSelectItemToggled"));


        uIList = new();
        uIList.Init(rdict, prefab_resourceEdit);
        GetNode("HBoxContainer").AddChild(uIList);
    }

    void OnAddButtonUp()
    {
        popup.Popup(new Rect2I(GetWindow().Position + (Vector2I)GlobalPosition, new Vector2I((int)Size.X, 0)));
        popup.Popup();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        QueueRedraw();
    }
    public override void _Draw()
    {
        base._Draw();
        uIResourceSelect.isDisabled = rdict.Keys().ToList();
        uIList.Update();
    }

    public void OnResourceSelectItemClicked(int selected)
    {
        GD.Print(selected);
        rdict[selected] = new Resource.RStatic(selected, 0, 1);
        popup.Hide();
        QueueRedraw();
    }

    public void OnResourceSelectItemToggled(int selected, bool toggled)
    {

        if (toggled)
        {
            rdict[selected] = new Resource.RStatic(selected, 0, 1);
        }
        else
        {
            rdict.Remove(selected);
        }

        popup.Hide();
        QueueRedraw();
    }
}
