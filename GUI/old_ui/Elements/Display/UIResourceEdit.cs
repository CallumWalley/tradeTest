using Godot;
using System;

public partial class UIResourceEdit : UIResource
{
    public override void _Ready()
    {
        // base._Ready();
        // GetNode<UIIncriment>("Incriment").callback = ChangeValue;
        // // Replace static value with editable one.
        // value = GetNode<UIEditValue>("UIEditValue");
        // ((UIEditValue)value).textEntered = ValidateAndSet;
        // AddChild(value);
        // MoveChild(value, 1);
    }

    public void ChangeValue(double _delta)
    {
        ((Resource.RStatic)resource).Set(resource.Sum + _delta);
        CallDeferred("_draw");
    }
    public void SetValue(double _value)
    {
        ((Resource.RStatic)resource).Set(_value);
        CallDeferred("_draw");
    }
    public bool ValidateAndSet(string _string)
    {
        bool valid = double.TryParse(_string, out double f);
        SetValue(f);
        return valid;
    }
}
