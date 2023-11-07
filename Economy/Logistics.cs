using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Contains code relevent to distrubution of resources.
/// </summary>
public partial class Logistics
{
    /// <summary>
    /// What sytem is used to disrubute resources.
    /// </summary>
    public interface ISystem
    {
        public void EFrameEarly(Installation installation);
    }

    public static class ExportToParent //: ISystem
    {
        public static void EFrameEarly(Installation installation)
        {
            // Sum all production and consumption.
            // Go downline, fetch all requests and surplus.
            // Add surplus to production/consumption.
            // Add demand 

            // request     resource
            // --------------------
            // consume      -  produce
            // import       -  importDemand
            // export       -  exportDemand
            // exportDemand -  importDemand
            // export       -  exportSurplus

            // EFrameLate
            // Starting at each 0 node, call GetNet()
            //      for each child
            //         either surplus -> import    
            //                demand  -> exportDemand
            //     calculate and return own surplus/demand

            // (surplus + import)        ->     import
            // (exportDemand + demand)   ->     exportDemand

            // Add export to import and DEMAND to exportDemand.

            // Divide produce + import by consume + exportDemand
            // For each export demand, add import to child
            //  
            // 
            // Add 

            // resourceBuffer2 = (Dictionary<int, double[]>)Ledger.GetBuffer();

            // delta.Clear();
            // consumed.Clear();
            // produced.Clear();
            // import.Clear();
            // export.Clear();

            if (installation.Order > 1)
            {
                // Not market head.
                return;
            }
            CalculateRequests(installation);
        }
        public static void EFrameLate(Installation installation)
        {
            // // Add result of last step to storage.
            // foreach (Resource.IResource r in resourceBuffer)
            // {
            //     Storage.Add(r.Type, r.Sum);
            // }
            // foreach (KeyValuePair<int, double[]> r in resourceBuffer2)
            // {
            //     Storage.Add(r.Key, r.Value[1]);
            // }
            // resourceBuffer.Clear();

            // foreach (Resource.IResource r in produced)
            // {
            //     productionBuffer[r.Type] = r.Sum;
            // }
            installation.Ledger.Clear();

            installation.GetProducers();
            installation.GetConsumers();

            // foreach (Resource.IResource r in delta)
            // {
            //     if (r.Type < 500)
            //     {
            //         resourceBuffer.Add(r);
            //     }
            // }
        }
        public static void CalculateRequests(Installation installation)
        {
            // For each child trade route
            foreach (TradeRoute downline in installation.Trade.DownlineTraderoutes)
            {
                // call this funtion on child.
                CalculateRequests(downline.Tail);
                // Iterate over ledger. add their ExportSurplus to my Import and their ImportDemand to my ExportDemand. 
                foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in downline.Tail.Ledger)
                {
                    downline.HeadImport[kvp.Key].Set(-kvp.Value.ExportSurplus.Sum); // Set trade route to export surplus of child.
                    downline.HeadExportRequest[kvp.Key].Set(-kvp.Value.ImportDemand.Sum); // Set trade route to request shortfall 
                }
            }
            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
            {   

                foreach (TradeRoute tr in installation.Trade.DownlineTraderoutes){
                    kvp.Value.Import.Add(tr.HeadImport[kvp.Key]);
                }

                // if (kvp.Key > 500) { }
                double net = kvp.Value.Production.Sum +
                                kvp.Value.ConsumptionRequest.Sum +
                                installation.Trade.DownlineTraderoutes.Sum((x => x.HeadImport[kvp.Key].Sum)) +
                                installation.Trade.DownlineTraderoutes.Sum((x => x.HeadExportRequest[kvp.Key].Sum)) +
                                kvp.Value.ExportDemand.Sum;

                // If balance is negative. Request the difference from parent.
                if (net < 0)
                {
                    kvp.Value.ExportSurplus.Set(0);
                    kvp.Value.ImportDemand.Set(-net);

                }
                // Otherwise, set export surplus to difference.
                else
                {
                    kvp.Value.ExportSurplus.Set(-net);
                    kvp.Value.Export.Add( kvp.Value.ExportSurplus );
                    kvp.Value.ImportDemand.Set(0);
                }
            }
        }
    }

}
