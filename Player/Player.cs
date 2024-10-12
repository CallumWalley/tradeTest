using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class Player : Node
{
	new public string Name { get { return base.Name; } set { base.Name = value; } }
	public PlayerTech tech;
	public PlayerTrade trade;
	public PlayerFeatureTemplates featureTemplates;

	public List<Domain> Domains { get; set; } = new List<Domain>();

	public override void _Ready()
	{
		tech = GetNode<PlayerTech>("PlayerTech");
		trade = GetNode<PlayerTrade>("PlayerTrade");
		featureTemplates = GetNode<PlayerFeatureTemplates>("PlayerFeatureTemplates");
	}
}
