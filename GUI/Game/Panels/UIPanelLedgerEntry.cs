using Godot;
using System;

public partial class UIPanelLedgerEntry : VBoxContainer, Lists.IListable<Resource.Ledger.Entry>
{
	public Resource.Ledger.Entry ledgerEntry;
	public Resource.Ledger.Entry GameElement { get { return ledgerEntry; } }
	public bool Destroy { get; set; } = false;
	UIResource netLocal;
	UIResource consumption;
	UIResource importExport;
	UIResource net;
	UIResourceStorage storage;
	public void Init(Resource.Ledger.Entry _ledgerEntry)
	{
		ledgerEntry = _ledgerEntry;
		netLocal = GetNode<UIResource>("NetLocal");
		consumption = GetNode<UIResource>("ConsumptionRequest");
		importExport = GetNode<UIResource>("ImportExport");
		net = GetNode<UIResource>("Net");
		// UIResource importDemand = GetNode<UIResource>("ImportDemand");
		// UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		netLocal.Init(ledgerEntry.NetLocal);
		consumption.Init(ledgerEntry.RequestLocal);
		importExport.Init(ledgerEntry.NetRemote);
		net.Init(ledgerEntry.Net);

		// IF accruable also make storage.
		if (ledgerEntry.Type < 500)
		{
			storage = (UIResourceStorage)GD.Load<PackedScene>("res://GUI/Game/Panels/UIResourceStorage.tscn").Instantiate();
			AddChild(storage);
			storage.Init(((Resource.Ledger.EntryAccrul)ledgerEntry));
			storage.Visible = true;
		}


		netLocal.ShowBreakdown = true;
		consumption.ShowBreakdown = true;
		importExport.ShowBreakdown = true;
		net.ShowBreakdown = true;

		// importDemand.Init(ledgerEntry.ImportDemand);
	}
	public override void _Ready()
	{
		Update();
	}


	public void Update()
	{
		netLocal.Update();
		consumption.Update();
		importExport.Update();
		net.Update();
		if (ledgerEntry.Type < 500)
		{
			storage.Update();
		}

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Draw()
	{
		base._Draw();
		// GD.Print(ledgerEntry);
	}
}
