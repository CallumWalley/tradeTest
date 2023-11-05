using Godot;
using System;
using System.Collections.Generic;

public partial class UIPanelLedger : HBoxContainer
{

	public Resource.Ledger Ledger;
	static readonly PackedScene prefab_LedgerEntry = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelLedgerEntry.tscn");


	public override void _Ready()
	{
		UIList<Resource.Ledger.Entry> uIList = new();
		uIList.Init(Ledger.Values(), prefab_LedgerEntry);

		AddChild(uIList);
	}
}
