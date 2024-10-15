using Godot;
using System;
namespace Game;

public partial class UIValidTradeRoute : Button, Lists.IListable<PlayerTrade.ValidTradeHead>
{
	public PlayerTrade.ValidTradeHead validTradeHead;
	public bool Destroy { get; set; }
	public PlayerTrade.ValidTradeHead GameElement { get { return validTradeHead; } }
	Label labelDistance;

	public void Init(PlayerTrade.ValidTradeHead _validTradeHead)
	{
		validTradeHead = _validTradeHead;

		// These should be in '_Ready' but at moment, trying to instantiate summary without target causes error.
		GetNode<UIDomainTiny>("HBoxContainer/DomainSummary").Init(validTradeHead.Head);
		GetNode<UIResource>("HBoxContainer/UIResource").Init(validTradeHead.TradeWeight);
		labelDistance = GetNode<Label>("HBoxContainer/Distance");
	}

	public void Update()
	{
		labelDistance.Text = String.Format("{0:N2} ly", validTradeHead.distance);
	}



}
