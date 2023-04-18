using Godot;
using System;

public class UISituation : VBoxContainer
{
    RichTextLabel name;
    RichTextLabel description;
    UIResource cause;
    UIResource effect;

    public Situations.Base situation;

    public void Init(Situations.Base _situation)
    {
        situation = _situation;
    }
    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("Name");
        description = GetNode<RichTextLabel>("Description");
        cause = GetNode<UIResource>("HboxContainer/Cause");
        effect = GetNode<UIResource>("HboxContainer/Effect");
    }

    public override void _Draw()
    {
        name.BbcodeText = $"[b]{situation.Name}[/b]";
        description.Text = situation.Description;
        if (situation is Situations.OutputModifier)
        {
            if (((Situations.OutputModifier)situation).cause != null)
            {
                cause.Init(((Situations.OutputModifier)situation).cause);
            }
            if (((Situations.OutputModifier)situation).effect != null)
            {
                effect.Init(((Situations.OutputModifier)situation).effect);
            }

        }

    }
}
