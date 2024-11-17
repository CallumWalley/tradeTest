using Godot;
using System;
namespace Game;

public partial class UIPanelLedgerEntry : VBoxContainer, Lists.IListable<Resource.Ledger.Entry>
{
	public Resource.Ledger.Entry ledgerEntry;
	public UIPanelLedger parent;
	public Resource.Ledger.Entry GameElement { get { return ledgerEntry; } }
	public bool Destroy { get; set; } = false;
	UIResource netLocal;
	UIResource netRemote;
	UIResource net;
	UIResourceStorage storage;
	public void Init(Resource.Ledger.Entry _ledgerEntry)
	{
		ledgerEntry = _ledgerEntry;
		netLocal = GetNode<UIResource>("NetLocal");
		netRemote = GetNode<UIResource>("NetRemote");
		// net = GetNode<UIResource>("Net");
		// UIResource importDemand = GetNode<UIResource>("ImportDemand");
		// UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		netLocal.Init(ledgerEntry.LocalNet);
		netRemote.Init(ledgerEntry.TradeNet);
		// net.Init(ledgerEntry.Net);

		// IF accruable also make storage.
		if (ledgerEntry.Type < 300)
		{
			storage = (UIResourceStorage)GD.Load<PackedScene>("res://GUI/Game/Panels/UIResourceStorage.tscn").Instantiate();
			AddChild(storage);
			storage.Init(((Resource.Ledger.EntryAccrul)ledgerEntry));
			storage.Visible = true;
		}


		netLocal.ShowBreakdown = true;
		netRemote.ShowBreakdown = true;
		// net.ShowBreakdown = true;

		// importDemand.Init(ledgerEntry.ImportDemand);
	}
	public override void _Ready()
	{
		Update();
	}


	public void Update()
	{
		bool isZero = true;
		netRemote.Visible = parent.ShowTrade;
		if (netRemote.Visible)
		{
			if (netRemote.value.Text != "-")
			{
				isZero = false;
			}
			netRemote.Update();
		}
		netLocal.Update();
		if (netLocal.value.Text != "-")
		{
			isZero = false;
		}
		// net.Update();
		// if (net.value.Text != "-")
		// {
		// 	isZero = false;
		// }
		if (ledgerEntry.Type < 300)
		{
			storage.Update();
			if (storage.value.Text != "0%")
			{
				isZero = false;
			}
		}
		Visible = !(parent.HideZeroCol && isZero);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Draw()
	{
		base._Draw();
	}
}
