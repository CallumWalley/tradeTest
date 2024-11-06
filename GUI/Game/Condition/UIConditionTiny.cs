using Godot;
using System;
using System.Text;
namespace Game;

public partial class UIConditionTiny : UIAccordian, Lists.IListable<Entities.ICondition>
{
    Entities.ICondition Condition;
    public Entities.ICondition GameElement { get { return Condition; } }
    public bool Destroy { get; set; } = false;

    [Export]
    public RichTextLabel richTextLabelDetails;

    public void Init(Entities.ICondition _condition)
    {
        Condition = _condition;
    }

    public override void _Ready()
    {
        base._Ready();
        button.TooltipText = "Click to expand details";
    }

    public void Update()
    {
        if (Destroy || !IsInstanceValid((Node)Condition)) { QueueFree(); return; }
        button.Text = Condition.Name;
        richTextLabelDetails.Text = Condition.Description;
    }
}