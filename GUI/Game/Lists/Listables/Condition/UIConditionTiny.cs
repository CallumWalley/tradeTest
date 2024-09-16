using Godot;
using System;

public partial class UIConditionTiny : HBoxContainer, Lists.IListable<ConditionBase>
{
    ConditionBase Condition;
    public ConditionBase GameElement { get { return Condition; } }
    public bool Destroy { get; set; } = false;

    public void Init(ConditionBase _condition)
    {
        Condition = _condition;
    }

    public override void _Draw()
    {
        base._Draw();
        // all icons currently the same.
        //GetNode<TextureRect>("Icon").Texture;
    }

    public void Update()
    {
        GetNode<Label>("Name").Text = Condition.Name;
    }
}