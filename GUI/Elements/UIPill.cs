using Godot;
using System;

public partial class UIPill : Label
{
	public string tag;
	public override void _Ready()
	{
		Text = Tags.Name(tag);
		TooltipText = Tags.Description(tag);

	}
}
