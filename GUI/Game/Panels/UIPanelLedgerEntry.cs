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
		UIResource consumption = GetNode<UIResource>("Consumption");
		UIResource import = GetNode<UIResource>("Import");
		UIResource export = GetNode<UIResource>("Export");
		// UIResource importDemand = GetNode<UIResource>("ImportDemand");
		// UIResource exportDemand = GetNode<UIResource>("ExportDemand");


		//UIResourceStorage storage = GetNode<UIResourceStorage>("Storage");
		production.Init(ledgerEntry.Production);
		consumption.Init(ledgerEntry.ConsumptionRequest);
		import.Init(ledgerEntry.Import);
		export.Init(ledgerEntry.Export);

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
