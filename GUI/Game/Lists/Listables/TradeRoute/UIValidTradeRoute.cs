using Godot;
using System;

public partial class UIValidTradeRoute : Button // UIList.IListable<PlayerTrade.ValidTradeHead>
{
	public PlayerTrade.ValidTradeHead validTradeHead;

	// public PlayerTrade.ValidTradeHead GameElement { get { return validTradeHead; } }
	// public bool Destroy { get; set; }

	public void Init(PlayerTrade.ValidTradeHead _validTradeHead)
	{
		validTradeHead = _validTradeHead;

		// These should be in '_Ready' but at moment, trying to instantiate summary without target causes error.
		GetNode<UIResourcePoolTiny>("HBoxContainer/ResourcePoolSummary").Init(validTradeHead.Head);
		GetNode<UIResource>("HBoxContainer/UIResource").Init(validTradeHead.TradeWeight);
	}
	public override void _Ready()
	{
		GetNode<Label>("HBoxContainer/Distance").Text = String.Format("{0:N2} ly", validTradeHead.distance);
	}

	public override void _Pressed()
	{
		base._Pressed();
		validTradeHead.Create();
		Disabled = true;

		GetNode<UIDropDownSetHead>("../../../../").CloseRequested();
		GetNode<UIDropDownSetHead>("../../../../").SetButtonContent();
	}
}
