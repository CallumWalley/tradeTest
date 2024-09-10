using Godot;
using System;
using System.Collections.Generic;

// [Tool]
public partial class UIDropDown : VBoxContainer
{
    // Children members

    protected Popup popup;
    protected PanelContainer popupPanel;
    protected VBoxContainer popupPanelList;
    protected PanelContainer button;
    protected MarginContainer buttonContent;
    protected Label buttonDefault;
    protected TextureButton buttonSettings;
    ButtonGroup buttonGroup = new ButtonGroup();




    public bool buttonDefaultVisible { get { return buttonDefault.Visible; } set { buttonDefault.Visible = value; buttonContent.Visible = !value; } }

    Control displayedElement;

    public override void _Ready()
    {
        //Connect("about_to_popup", new Callable(this, "AboutToPopup"));
        // Get Children
        button = GetNode<PanelContainer>("Button");
        buttonContent = button.GetNode<MarginContainer>("HBoxContainer/Content");
        buttonDefault = button.GetNode<Label>("HBoxContainer/Default");

        popup = GetNode<Popup>("Popup");

        buttonSettings = button.GetNode<TextureButton>("HBoxContainer/Settings");
        popupPanel = popup.GetNode<PanelContainer>("Panel");
        popupPanelList = popupPanel.GetNode<VBoxContainer>("List");


        // Connect Signals

        //popup.Connect("close_requested", new Callable(this, "_PopupHide"));
        // popup.Connect("go_back_requested", new Callable(this, "_PopupHide"));
        popup.Connect("popup_hide", new Callable(this, "_PopupHide"));
        // popup.Connect("gui_focus_changed", new Callable(this, "_PopupHide"));

        // popup.Connect("focus_exited", new Callable(this, "_PopupHide"));

        popup.Connect("about_to_popup", new Callable(this, "_AboutToPopup"));

        buttonSettings.Connect("button_up", new Callable(this, "_ButtonUp"));
        buttonSettings.Connect("toggled", new Callable(this, "_Toggled"));
    }

    //public IEnumerable<Control> EnumChildren()


    // public bool IsDisabled()
    // {
    //     return vBox.GetChildCount() < 1;
    // }
    // To avoid failure if no trade route.

    public virtual void _Toggled(bool toggled)
    {
        if (toggled)
        {
            popup.Popup(new Rect2I(GetWindow().Position + (Vector2I)GlobalPosition + new Vector2I(0, (int)Size.Y), new Vector2I((int)Size.X, 0)));
        }
        else
        {
            popup.Hide();
        }
    }

    public virtual void _AboutToPopup()
    {
    }
    public virtual void _PopupHide()
    {
        buttonSettings.ButtonPressed = false;
    }
    public virtual void _ButtonUp()
    {

    }


    public virtual void CloseRequested()
    {
        //popup.Hide();
        buttonSettings.ButtonPressed = false;
    }
    // protected Control NoSelection()
    // {
    //     Label label = new();
    //     label.Text = "No value selected";
    //     return label;
    // }

    public override void _Draw()
    {
        base._Draw();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
