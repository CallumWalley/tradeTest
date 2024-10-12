using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UIPanelLedger : HBoxContainer
{

	public Resource.Ledger Ledger;
	static readonly PackedScene prefab_LedgerEntry = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelLedgerEntry.tscn");
	UILedgerList uIList;

	partial class UILedgerList : UIList<Resource.Ledger.Entry>
	{
		public UIPanelLedger parent;
		protected override UIPanelLedgerEntry CreateNewElement(Resource.Ledger.Entry entry)
		{
			UIPanelLedgerEntry nui = (UIPanelLedgerEntry)base.CreateNewElement(entry);
			nui.parent = parent;
			return nui;
		}
	}
	public bool ShowTrade { get; set; } = true;
	public bool HideZeroCol { get; set; } = false;

	public override void _Ready()
	{
		uIList = new();
		uIList.parent = this;
		uIList.Init(Ledger.Values(), prefab_LedgerEntry);

		AddChild(uIList);
	}

	public void Update()
	{
		uIList.Update();
	}
}
