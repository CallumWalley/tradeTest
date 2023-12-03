using Godot;
using System;

public partial class UIPanelLedgerEntry : VBoxContainer, Lists.IListable<Resource.Ledger.Entry>
{
	public Resource.Ledger.Entry ledgerEntry;
	public Resource.Ledger.Entry GameElement { get { return ledgerEntry; } }
	public bool Destroy { get; set; } = false;

	public void Init(Resource.Ledger.Entry _ledgerEntry)
	{
		ledgerEntry = _ledgerEntry;
		UIResource netLocal = GetNode<UIResource>("NetLocal");
		UIResourceRequest consumption = GetNode<UIResourceRequest>("ConsumptionRequest");
		UIResourceRequest importExport = GetNode<UIResourceRequest>("ImportExport");
		UIResource net = GetNode<UIResource>("Net");
		UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");
		// UIResource importDemand = GetNode<UIResource>("ImportDemand");
		// UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		//UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");
		netLocal.Init(ledgerEntry.NetLocal);
		consumption.Init(ledgerEntry.RequestLocal);
		importExport.Init(ledgerEntry.NetRemote);
		net.Init(ledgerEntry.Net);

		// IF accruable also make storage.
		if (ledgerEntry.Type < 500)
		{
			storage.Init(((Resource.Ledger.EntryAccrul)ledgerEntry));
			storage.Visible = true;
		}
		else
		{
			storage.QueueFree();
		}


		netLocal.ShowBreakdown = true;
		consumption.ShowBreakdown = true;
		importExport.ShowBreakdown = true;
		net.ShowBreakdown = true;

		// importDemand.Init(ledgerEntry.ImportDemand);
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Draw()
	{
		base._Draw();
		// GD.Print(ledgerEntry);
	}
}
