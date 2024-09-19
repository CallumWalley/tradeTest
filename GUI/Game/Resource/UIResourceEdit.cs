using Godot;
using System;

public partial class UIResourceEdit : UIResource
{
    LineEdit lineEdit;
    public override void _Ready()
    {
        lineEdit = GetNode<LineEdit>("LineEdit");
        lineEdit.Connect("text_submitted", new Callable(this, "OnlineEditTextSubmitted"));
        base._Ready();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right)
        {
            OnLineEditTextSubmitted(lineEdit.Text);
        }
    }
    public void OnLineEditTextSubmitted(string text)
    {
        double newVal;
        try
        {
            newVal = Convert.ToDouble(text);
            resource.Request = newVal;
        }
        catch (Exception)
        {
            GD.Print($"{text}Not convertable to double.");
        }
        lineEdit.Text = string.Format(resource.ValueFormat, resource.Sum);
        lineEdit.Deselect();
    }
    public override void _Draw()
    {

        ((TextureRect)GetNode("Icon")).Texture = Resource.Icon((resource != null) ? resource.Type : 0);
        value.Text = string.Format(resource.ValueFormat, resource.Sum);
    }
}
