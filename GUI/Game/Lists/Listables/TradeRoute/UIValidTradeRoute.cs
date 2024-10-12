using Godot;
using System;
namespace Game;

public partial class UIValidTradeRoute : Button, Lists.IListable<PlayerTrade.ValidTradeHead>
{
	public PlayerTrade.ValidTradeHead validTradeHead;
	public bool Destroy { get; set; }
	public PlayerTrade.ValidTradeHead GameElement { get { return validTradeHead; } }
	Label labelDistance;

	public Node Driver;


	public void Init(PlayerTrade.ValidTradeHead _validTradeHead)
	{
		validTradeHead = _validTradeHead;

		// These should be in '_Ready' but at moment, trying to instantiate summary without target causes error.
		GetNode<UIDomainTiny>("HBoxContainer/DomainSummary").Init(validTradeHead.Head);
		GetNode<UIResource>("HBoxContainer/UIResource").Init(validTradeHead.TradeWeight);
		labelDistance = GetNode<Label>("HBoxContainer/Distance");
	}
	public override void _Ready()
	{

	}

	public override void _Pressed()
	{
		base._Pressed();
		if (Driver != null)
		{
			((UIDropDownSetHead)Driver).OnButtonPressed(this);
		}
		Disabled = true;
	}

	public void Update()
	{
		labelDistance.Text = String.Format("{0:N2} ly", validTradeHead.distance);
	}



}
