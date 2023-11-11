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
		UIResource production = GetNode<UIResource>("Production");
		UIResourceRequest consumption = GetNode<UIResourceRequest>("ConsumptionRequest");
		UIResource import = GetNode<UIResource>("Import");
		UIResource export = GetNode<UIResource>("Export");
		UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");
		// UIResource importDemand = GetNode<UIResource>("ImportDemand");
		// UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		//UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");
		production.Init(ledgerEntry.ResourceLocal);
		consumption.Init(ledgerEntry.RequestLocal);
		import.Init(ledgerEntry.ResourceChildren);
		export.Init(ledgerEntry.NetLocal);

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


		production.ShowBreakdown = true;
		consumption.ShowBreakdown = true;
		import.ShowBreakdown = true;
		export.ShowBreakdown = true;

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
