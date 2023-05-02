using Godot;
using System;
using System.Collections.Generic;

public partial class UITransformer : Control, UIContainers.IListable
{
    // Game object this UI element follows.
    public Control Control { get { return this; } }
    public System.Object GameElement { get { return transformer; } }
    public Transformer transformer;
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
        Init((Transformer)gameObject);
    }
    public virtual void Init(Transformer _transformer)
    {
        transformer = _transformer;
    }
    public override void _Ready()
    {
        base._Ready();
        button = GetNode<Button>("Accordian/Button");
        details = GetNode<Container>("Accordian/Container");

        VBoxContainer leftSide = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/Left");


        button.Connect("toggled", new Callable(this, "ShowDetails"));

        // Set button text
        button.GetNode<Label>("SummaryContent/Summary").Text = transformer.Name;

        // Set reorder buttons
        moveUpButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveUp");
        moveDownButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveDown");
        moveUpButton.Connect("pressed", new Callable(this, "ReorderUp"));
        moveDownButton.Connect("pressed", new Callable(this, "ReorderDown"));

        situations = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/TabContainer/Situation");
        details.GetNode<Label>("VBoxContainer/Description").Text = transformer.Description;

        // Init resource pool display. // new UIContainers.UIResourceList();
        UIContainers.UIResourceList uiDelta = leftSide.GetNode<UIContainers.UIResourceList>("/Production");
        UIContainers.UIResourceList uiConsumption = new UIContainers.UIResourceList();//details.GetNode<UIContainers.UIResourceList>("VBoxContainer/HSplitContainer/Left/Consumption");
        UIContainers.UIResourceList uiStorage = new UIContainers.UIResourceList(); //details.GetNode<UIContainers.UIResourceList>("VBoxContainer/HSplitContainer/Left/Storage");
        leftSide.AddChild(uiDelta);
        // leftSide.AddChild(uiConsumption);
        leftSide.AddChild(uiStorage);



        // uiConsumption.Init(Flatten(transformer.Consumption));
        uiDelta.Init(transformer.Production);
        uiStorage.Init(transformer.Storage);

    }
    public override void _Draw()
    {
        if (transformer == null) { return; }
        //int index = tradeRoute.destination.GetTransformerTrade().tradeRoutes.IndexOf(tradeRoute);
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
            if (transformer.Situations.Count > 0)
            {
                situations.GetNode<Label>("NoSituation").Visible = false;
            }
            else
            {
                situations.GetNode<Label>("NoSituation").Visible = true;
            }
        }
        // for
        // tradeRoute
    }

    public void ShowDetails(bool toggled)
    {
        details.Visible = toggled;
    }
    // public void Remove(){
    // 	GetNode<PlayerTradeRoutes>("/root/Global/Player/Trade/Routes").DeregisterTradeRoute(tradeRoute);
    // 	Control parent = GetParent<Control>();
    // 	parent.RemoveChild(this);
    // 	parent.Update();
    // 	QueueFree();
    // }

    public void ReorderUp()
    {
        transformer.GetParent<Installation>().MoveChild(transformer, transformer.GetIndex() - 1);
        //FIXME
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = false;
        GetParent<Control>().GetParent<Control>().GetParent<Control>().Visible = true;

    }
    public void ReorderDown()
    {
        Installation par = transformer.GetParent<Installation>();
        int chindex = transformer.GetIndex() + 1;
        par.MoveChild(transformer, chindex);
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
        UISituation ui = (UISituation)p_situation.Instance();
        ui.Init(s);
        situations.AddChild(ui);
        situations.MoveChild(ui, index);
    }

    IEnumerable<Resource.IResource> Flatten(IEnumerable<TransformerInputType.Base> inputList)
    {
        foreach (TransformerInputType.Base i in inputList)
        {
            yield return i.Response;
        }
    }
}
