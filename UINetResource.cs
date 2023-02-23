using Godot;
using System;

public class UINetResource : HBoxContainer
{
	Node NetResource;
	Node resourceGroup;
	PackedScene ResourceElement;
	
	public override void _Ready()
	{
		NetResource = GetNode("NetResource");
		ResourceElement = GD.Load<PackedScene>("res://templates/GUI/ResourceIcon.tscn");
		resourceGroup = GetNode("../../ResourceGroup");
		 
		UpdateNumbers();
	}

	public void UpdateNumbers(){
		foreach (ResourceGenerator resource in resourceGroup.GetChildren()){
			var rp = ResourceElement.Instance();
			Label label = (Label)rp.GetNode("State/Number");
			TextureRect icon = (TextureRect)rp.GetNode("State/Icon");
			icon.Texture = Resources.Icon(resource.type);
			label.Text = resource.delta.ToString();
			NetResource.AddChild(rp);
		}
	}
}
