using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            CalculateResources(installation);

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
        /// <summary>
        /// This will go down the tree and work out unfulfilled demand for each child.
        /// </summary>
        /// <param name="installation"></param>
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
                    // if surplus, do surplus stuff.
                    if (kvp.Value.ResourceParent.Sum > 0)
                    {
                        downline.HeadImport[kvp.Key].Set(-kvp.Value.ResourceParent.Sum); // Set trade route to export surplus of child.
                        installation.Ledger[kvp.Key].ResourceChildren.Add(downline.HeadImport[kvp.Key]); // Add this to general import list.
                    }
                    // Else do deficit stuff.
                    else
                    {
                        downline.HeadExportRequest[kvp.Key].Set(-kvp.Value.RequestParent.Sum); // Set trade route to request shortfall 
                        installation.Ledger[kvp.Key].RequestChildren.Add(new Resource.RRequest(kvp.Key, downline.HeadImport[kvp.Key].Sum)); // Add this to general import list.
                    }
                }
            }
            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
            {

                foreach (TradeRoute tr in installation.Trade.DownlineTraderoutes)
                {
                    kvp.Value.ResourceChildren.Add(tr.HeadImport[kvp.Key]);
                }

                // if (kvp.Key > 500) { }

                double request = installation.Trade.DownlineTraderoutes.Sum((x => x.HeadExportRequest[kvp.Key].Sum)) + kvp.Value.RequestLocal.Request;
                double resource = installation.Trade.DownlineTraderoutes.Sum((x => x.HeadImport[kvp.Key].Sum)) + kvp.Value.ResourceLocal.Sum;
                double net = resource + request;

                // If balance is negative. Request the difference from parent.
                if (net < 0)
                {
                    kvp.Value.ResourceParent.Set(0);
                    kvp.Value.RequestParent.Request = -net;

                }
                // Otherwise, set export surplus to difference.
                else
                {
                    kvp.Value.ResourceParent.Set(-net);
                    kvp.Value.RequestParent.Request = 0;
                }
            }
        }

        public static void CalculateResources(Installation installation)
        {
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger.Where(x=>x.Key < 500))
            {
                // If non accruable, skip.
                if (kvp.Value.RequestLocal.Request == 0)
                {
                    // TODO: investigate why this zero.
                    continue;
                }

                double requestTotal = kvp.Value.RequestLocal.Request;
                double resourceTotal = installation.Trade.DownlineTraderoutes.Sum((x => x.HeadImport[kvp.Key].Sum)) + kvp.Value.ResourceLocal.Sum;
                double supplyFaction = resourceTotal / -requestTotal ;

                // Supply local.
                foreach (Resource.RRequest r in kvp.Value.RequestLocal)
                {
                    // Respond with fraction.
                    if (supplyFaction < 1)
                    {
                        r.Respond(r.Request * supplyFaction);
                    }
                    else
                    {
                        r.Respond();
                    }
                }
                // If not enough resources to supply local, not enough to export.
                if (supplyFaction < 1){
                    continue;
                }

                // Any leftovers go to exports.

                // foreach (Resource.RRequest r in kvp.Value. .Adders)
                // {
                //     r.Respond(supplyFaction * r.Request);
                //     kvp.Value.Production.Add(r);
                // }
                //=                //double netRequestRemote = installation.Trade.DownlineTraderoutes.Sum((x => x.HeadExportRequest[kvp.Key].Sum));


            }
        }
    }

}
