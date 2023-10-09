using Godot;
using System;
public partial class UIAccordianIndustry : UIAccordian, UIList<Industry>.IListable<Industry>
{
    // Game object this UI element follows.
    public Industry GameElement { get { return Industry; } }
    public bool Destroy { get; set; }
    public Industry Industry;
    static readonly PackedScene p_situation = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/Entities/UI_Industry_Full.tscn");
    static readonly PackedScene p_UIstorage = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResourceStorage.tscn");
    static readonly PackedScene p_uirequest = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResourceRequest.tscn");

    TextureButton moveUpButton;
    TextureButton moveDownButton;

    Control container;
    Button button;
    Control situations;

    // Element to update on change.
    //Control callback;
    public virtual void Init(Industry _Industry)
    {
        Industry = _Industry;
    }
    public override void _Ready()
    {
        base._Ready();
        button = GetNode<Button>("Accordian/Button");
        container = GetNode<Container>("Accordian/Container");

        //VBoxContainer leftSide = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/Left");

        // button.Connect("toggled", new Callable(this, "ShowDetails"));

        // Set button text
        button.GetNode<Label>("HBoxContainer/Label").Text = Industry.Name;

        // // Set reorder buttons
        // moveUpButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveUp");
        // moveDownButton = button.GetNode<TextureButton>("AlignRight/Incriment/MoveDown");
        // moveUpButton.Connect("pressed", new Callable(this, "ReorderUp"));
        // moveDownButton.Connect("pressed", new Callable(this, "ReorderDown"));

        //situations = details.GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/TabContainer/Situation");
        container.GetNode<Label>("Label").Text = Industry.Description;

        // Init resource pool display. // new UIResourceList();
        UIResourceList uiProduction = new(); //leftSide.GetNode<UIResourceList>("/Production");
        UIList<Resource.IRequestable> uiConsumption = new();//details.GetNode<UIResourceList>("VBoxContainer/HSplitContainer/Left/Consumption");
        UIResourceList uiStorage = new(); //details.GetNode<UIResourceList>("VBoxContainer/HSplitContainer/Left/Storage");

        // leftSide.AddChild(uiProduction);
        // leftSide.AddChild(uiConsumption);
        // leftSide.AddChild(uiStorage);

        // uiProduction.Init(Industry.Production);
        // uiConsumption.Init(Industry.Consumption, p_uirequest);

        //uiConsumption.Init(Industry.Consumption);
        //uiDelta.Init(Industry.Production);
        // uiStorage.Init(Industry.stored);
    }
    public override void _Draw()
    {
        if (Industry == null) { return; }
        if (Destroy)
        {
            QueueFree();
        }
    }

    // public void ShowDetails(bool toggled)
    // {
    //     container.Visible = toggled;
    // }

    void UpdateSituations(Situations.Base s, int index)
    {
        // foreach (UISituation uis in situations.GetNode<VBoxContainer>("VBoxContainer").GetChildren())
        // {
        //     if (uis.situation == s)
        //     {
        //         situations.MoveChild(uis, index);
        //         return;
        //     }
        // }
        // // If doesn't exist, add it and insert at postition.
        // UISituation ui = (UISituation)p_situation.Instantiate();
        // ui.Init(s);
        // situations.AddChild(ui);
        // situations.MoveChild(ui, index);
    }
}

