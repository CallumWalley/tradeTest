using Godot;
using System;
using System.Collections.Generic;
using System.Text;
namespace Game;

public partial class UIActionFull<T> : UIAccordian, Lists.IListable<T> where T : ActionBase
{
    T Action;
    public object GameElement { get { return Action; } }
    public bool Destroy { get; set; } = false;

    [Export]
    public RichTextLabel richTextLabelDetails;

    public void Init(T _action)
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
        if (Destroy) { QueueFree(); return; }
        button.Text = Action.Name;
        richTextLabelDetails.Text = Action.Description;
    }
}