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
	}
	public override void _Ready()
	{
		UIResource production = GetNode<UIResource>("Production");
		UIResource consumption = GetNode<UIResource>("Consumption");
		UIResource import = GetNode<UIResource>("Import");
		UIResource export = GetNode<UIResource>("Export");
		UIResource importDemand = GetNode<UIResource>("ImportDemand");
		UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		//UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");

		production.Init(ledgerEntry.Production);
		consumption.Init(ledgerEntry.Consumption);
		// import.Init(ledgerEntry.Import);
		// export.Init(ledgerEntry.Export);
		// importDemand.Init(ledgerEntry.ImportDemand);
		// exportDemand.Init(ledgerEntry.ExportDemand);
		//storage.Init(ledgerEntry);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
