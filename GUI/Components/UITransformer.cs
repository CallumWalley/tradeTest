using Godot;
using System;
using System.Collections.Generic;

public partial class UIIndustry : Control, UIContainers.IListable
{
    // Game object this UI element follows.
    public Control Control { get { return this; } }
    public System.Object GameElement { get { return Industry; } }
    public Industry Industry;
    public static PlayerTradeRoutes playerTradeRoutes;

    static readonly PackedScene p_situation = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UISituation.tscn");

    TextureButton moveUpButton;
    TextureButton moveDownButton;

    Control details;
    Button button;
    Control situations;

    // Element to update on change.
    //Control callback;
    public virtual void Init(System.Object gameObject)
    {
        Init((Industry)gameObject);
    }
    public virtual void Init(Industry _Industry)
    {
        Industry = _Industry;
    }
    public override void _Ready()
    {
        base._Ready();
        button = GetNode<Button>("Accordian/Button");
        details = GetNode<Container>("Accordian/Container");

        VBoxContainer leftSide = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/Left");


        // button.Connect("toggled", new Callable(this, "ShowDetails"));

        // Set button text
        button.GetNode<Label>("SummaryContent/Summary").Text = Industry.Name;

        // Set reorder buttons
        moveUpButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveUp");
        moveDownButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveDown");
        moveUpButton.Connect("pressed", new Callable(this, "ReorderUp"));
        moveDownButton.Connect("pressed", new Callable(this, "ReorderDown"));

        situations = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/TabContainer/Situation");
        details.GetNode<Label>("VBoxContainer/Description").Text = Industry.Description;

        // Init resource pool display. // new UIResourceList();
        UIResourceList uiDelta = new UIResourceList(); //leftSide.GetNode<UIResourceList>("/Production");
        //UIResourceList uiConsumption = new UIResourceList();//details.GetNode<UIResourceList>("VBoxContainer/HSplitContainer/Left/Consumption");
        UIResourceList uiStorage = new UIResourceList(); //details.GetNode<UIResourceList>("VBoxContainer/HSplitContainer/Left/Storage");
        leftSide.AddChild(uiDelta);
        // leftSide.AddChild(uiConsumption);
        leftSide.AddChild(uiStorage);


        // uiConsumption.Init(Flatten(Industry.Consumption));
        uiDelta.Init(Industry.Production);
        uiStorage.Init(Industry.Storage);

    }
    public override void _Draw()
    {
        if (Industry == null) { return; }
        //int index = tradeRoute.destination.GetIndustryTrade().tradeRoutes.IndexOf(tradeRoute);
        int index = GetIndex();
        moveUpButton.Disabled = false;
        moveDownButton.Disabled = false;
        if (index == 0)
        {
            moveUpButton.Disabled = true;
        }
        if (index == (GetParent().GetChildCount() - 1))
        {
            moveDownButton.Disabled = true;
        }
        if (index == (GetParent().GetChildCount() - 1))
        {
            moveDownButton.Disabled = true;
        }
        if (details.Visible)
        {
            if (Industry.Situations.Count > 0)
            {
                situations.GetNode<Label>("NoSituation").Visible = false;
            }
            else
            {
                situations.GetNode<Label>("NoSituation").Visible = true;
            }
        }
    }

    public void ShowDetails(bool toggled)
    {
        details.Visible = toggled;
    }


    public void ReorderUp()
    {
        Industry.GetParent<Installation>().MoveChild(Industry, Industry.GetIndex() - 1);
        //FIXME
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = true;

    }
    public void ReorderDown()
    {
        Installation par = Industry.GetParent<Installation>();
        int chindex = Industry.GetIndex() + 1;
        par.MoveChild(Industry, chindex);
        // FIXME
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = true;
    }
    void UpdateSituations(Situations.Base s, int index)
    {
        foreach (UISituation uis in situations.GetNode<VBoxContainer>("VBoxContainer").GetChildren())
        {
            if (uis.situation == s)
            {
                situations.MoveChild(uis, index);
                return;
            }
        }
        // If doesn't exist, add it and insert at postition.
        UISituation ui = (UISituation)p_situation.Instantiate();
        ui.Init(s);
        situations.AddChild(ui);
        situations.MoveChild(ui, index);
    }

    IEnumerable<Resource.IResource> Flatten(IEnumerable<IndustryInputType.Base> inputList)
    {
        foreach (IndustryInputType.Base i in inputList)
        {
            yield return i.Response;
        }
    }
}
