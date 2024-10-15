using Godot;
using System;
using System.Text;
namespace Game;

public partial class UIConditionTiny : HBoxContainer, Lists.IListable<ConditionBase>
{
    ConditionBase Condition;
    public ConditionBase GameElement { get { return Condition; } }
    public bool Destroy { get; set; } = false;

    public Label nameLabel;
    public void Init(ConditionBase _condition)
    {
        Condition = _condition;
    }

    public override void _Ready()
    {
        base._Ready();
        nameLabel = GetNode<Label>("Name");
    }

    public override void _Draw()
    {
        base._Draw();
        // all icons currently the same.
        //GetNode<TextureRect>("Icon").Texture;
    }

    public void Update()
    {
        if (Destroy || !IsInstanceValid(Condition)) { QueueFree(); return; }
        nameLabel.Text = Condition.Name;
        nameLabel.TooltipText = Condition.Description;

    }
}