using Godot;
using System;
using System.Text;
namespace Game;

public partial class UIActionFull : UIAccordian, Lists.IListable<Entities.IAction>
{
    Entities.IAction Action;
    public Entities.IAction GameElement { get { return Action; } }
    public bool Destroy { get; set; } = false;

    [Export]
    public RichTextLabel richTextLabelDetails;

    public void Init(Entities.IAction _action)
    {
        Action = _action;
    }

    public override void _Ready()
    {
        base._Ready();
        button.TooltipText = "Click to expand details";
    }

    public virtual void Update()
    {
        if (Action == null) { return; }
        Visible = Action.Visible;
        if (Destroy || !IsInstanceValid((Node)Action)) { QueueFree(); return; }
        button.Text = Action.Name;
        richTextLabelDetails.Text = Action.Description;
    }
}