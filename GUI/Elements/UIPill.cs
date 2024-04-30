using Godot;
using System;

public partial class UIPill : Label
{
	public Features.FeatureTag tag;
	public override void _Ready()
	{
		Text = tag.Name;
		TooltipText = tag.Description;

	}
}
