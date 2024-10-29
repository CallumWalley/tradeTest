using Godot;
using System;
using System.Text;
namespace Game;

public partial class UIConditionTiny : UIAccordian, Lists.IListable<ConditionBase>
{
    ConditionBase Condition;
    public ConditionBase GameElement { get { return Condition; } }
    public bool Destroy { get; set; } = false;

    [Export]
    public Button button;
    [Export]
    public RichTextLabel richTextLabelDetails;

    public void Init(ConditionBase _condition)
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
        if (Destroy || !IsInstanceValid(Condition)) { QueueFree(); return; }
        button.Text = Condition.Name;
        richTextLabelDetails.Text = Condition.Description;
    }
}