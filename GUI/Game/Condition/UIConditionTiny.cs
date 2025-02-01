using Godot;
using System;
using System.Text;
namespace Game;

public partial class UIConditionTiny : PanelContainer, Lists.IListable<Entities.ICondition>
{
    Entities.ICondition Condition;
    public object GameElement { get { return Condition; } }
    public bool Destroy { get; set; } = false;

    [Export]
    public TextureRect icon;
    [Export]
    public RichTextLabel label;

    public void Init(Entities.ICondition _condition)
    {
        Condition = _condition;
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public void Update()
    {
        Visible = Condition.Visible;
        if (Destroy || !IsInstanceValid((Node)Condition)) { QueueFree(); return; }
        label.Text = Condition.Name;
        // icon = Condition.Icon;
        TooltipText = Condition.Description;
    }
}