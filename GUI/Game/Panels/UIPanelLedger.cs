using Godot;
using System;
using System.Collections.Generic;

public partial class UIPanelLedger : HBoxContainer
{

	public Resource.Ledger Ledger;
	static readonly PackedScene prefab_LedgerEntry = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelLedgerEntry.tscn");
	UIList<Resource.Ledger.Entry> uIList;

	public override void _Ready()
	{
		uIList = new();
		uIList.Init(Ledger.Values(), prefab_LedgerEntry);

		AddChild(uIList);
	}

	public void Update()
	{
		uIList.Update();
	}
}
