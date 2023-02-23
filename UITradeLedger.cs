using Godot;
using System;

public class UITradeLedger : HBoxContainer
{
	[Export]
	Node tradeGroup;
	PackedScene ResourceIcon;
	
	public override void _Ready()
	{

		tradeGroup = (Node)GetNode("../../../Trade");
		ResourceIcon = (PackedScene)GD.Load<PackedScene>("res://templates/GUI/ResourceIcon.tscn");
		UpdateNumbers();
	}

	public void UpdateNumbers(){
		Node export = GetNode("Balance/Export");
		Node import = GetNode("Balance/Import");
		foreach (ResourceGenerator trade in tradeGroup.GetChildren()){
			var ri = ResourceIcon.Instance();
			TextureRect icon = (TextureRect)ri.GetNode("State/Icon");
			Label label = (Label)ri.GetNode("State/Number");
			icon.Texture = Resources.Icon(trade.type);
			if (trade.delta > 0){
				export.AddChild(ri);
				label.Text = (trade.delta).ToString();
			}else if (trade.delta < 0){
				import.AddChild(ri);
				label.Text = (-trade.delta).ToString();
			}
			//get_node("Label").add_color_override("font_color", Color(1,0,0,1))
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
