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
        public void EFrameLate(Installation installation);
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
                // // Iterate over ledger. add their ExportSurplus to my Import and their ImportDemand to my ExportDemand. 
                // foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in downline.Tail.Ledger)
                // {
                //     // if surplus, do surplus stuff.
                //     if (kvp.Value.ResourceParent.Sum > 0)
                //     {
                //         downline.Tail.Ledger[kvp.Key].ResourceParent.Set(-kvp.Value.ResourceParent.Sum);                            // Set trade route to export surplus of child.
                //         installation.Ledger[kvp.Key].ResourceChildren.Add(new Resource.RTradedResource(downline.Tail.Ledger[kvp.Key].ResourceParent));    // Add this to general import list.
                //     }
                //     // Else do deficit stuff.
                //     else
                //     {
                //         downline.Tail.Ledger[kvp.Key].RequestParent.Request = -kvp.Value.ResourceParent.Sum;                                    // Set trade route to export surplus of child.
                //         installation.Ledger[kvp.Key].RequestChildren.Add(new Resource.RTradedRequest(downline.Tail.Ledger[kvp.Key].RequestParent));
                //     }
                // }

                // // This is bad and should be done in a way that doesn't redestroy items.
                // foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
                // {
                //     kvp.Value.RequestExportToChildren.Clear();
                //     kvp.Value.RequestImportFromChildren.Clear();
                // }

                // foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in downline.Tail.Ledger)
                // {


                // }
            }


            // If no upline, end here.
            if (installation.Trade.UplineTraderoute == null)
            {
                return;
            }

            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
            {
                // Special rules for trade.
                // if (kvp.Key == 901)
                // {
                //     continue;
                // }

                // if (kvp.Key > 500) { }
                // Net is equal to all requests from downline, plus local.
                double net = installation.Trade.DownlineTraderoutes.Where(x => x.ListRequestHead.ContainsKey(kvp.Key)).Sum((x => x.ListRequestHead[kvp.Key].Request)) +
                kvp.Value.RequestLocal.Request + kvp.Value.ResourceLocal.Sum;

                // Request the difference from parent.
                kvp.Value.RequestToParent.Request = net;
            }
        }
        /// <summary>
        ///  Once all elements have 'CalculateRequests' this is run.
        /// </summary>
        /// <param name="installation"></param>
        static void CalculateResources(Installation installation)
        {
            /// how much of this request can be fulfilled.
            double supplyFaction = 0;
            double shipsAvailable = 0;

            // First resolve 
            // enumerate through elements in ledger 
            // in reverse order so ships are resolved first.
            //
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger.Reverse()){
                double max = kvp.Value.Net.Sum; //+ stored.
                // First allocate resources locally. 
                // TODO: if supply from stockpile 
                if (max > 0){
                    double remainder = ShareResources(max, kvp.Value.RequestLocal);
                    if (installation.Trade.DownlineTraderoutes.Count > 0 && remainder > 0){
                        double requestedShips = installation.Ledger[901].RequestFromChildren.Sum(x => x.Request);
                        double resourceShippingFraction = requestedShips == 0 ? 1 : installation.Ledger[901].Net.Sum / requestedShips;
                        // If resources remain, allocate to requesters. 
                        remainder = ShareResources(remainder, kvp.Value.RequestFromChildren, resourceShippingFraction);
                        if (remainder > 0 && kvp.Key < 500 ){
                            //do storage
                        }
                    }
                }
            }
            
            
             //.Where(x => x.Key < 500)
  



            // if (installation.Trade.UplineTraderoute != null)
            // {
            //     installation.Trade.ShipDemand.Request = 0;
            // }
            // if (installation.Trade.DownlineTraderoutes.Count() > 0)
            // {
            //     shipsAvailable = installation.Ledger[901].Net.Sum;
            // }

            // // Determine if enough ships for trade.
            // foreach (TradeRoute tradeRoute in installation.Trade.DownlineTraderoutes)
            // {
            //     installation.Trade.ShipDemand.Request += tradeRoute.ShipDemand.Sum;
            // }

            // if (installation.Trade.UplineTraderoute != null)
            // {
            //     foreach (Resource.RRequest r in installation.Trade.ShipDemand)
            //     {
            //         r.Respond(Math.Min(shipsAvailable, installation.Trade.ShipDemand.Request));
            //     }
            // }
            // if (installation.Trade.DownlineTraderoutes.Count() > 0)
            // {
            //     supplyFaction = installation.Trade.ShipDemand.Sum / installation.Trade.ShipDemand.Request;
            // }
            // Tell each trade route how many ships it gets.
            // foreach (TradeRoute tradeRoute in installation.Trade.DownlineTraderoutes)
            // {
            //     tradeRoute.Tail.Trade.outboundShipDemand.Respond(Math.Min(tradeRoute.Tail.Trade.ShipDemand.Request * supplyFaction, tradeRoute.Tail.Trade.outboundShipDemand.Request));
            //     tradeRoute.Tail.Trade.inboundShipDemand.Respond(Math.Min(tradeRoute.Tail.Trade.ShipDemand.Request * supplyFaction, tradeRoute.Tail.Trade.inboundShipDemand.Request));
            // }

            /// Actualise imports.
            // foreach (TradeRoute tradeRoute in installation.Trade.DownlineTraderoutes)
            // {
            //     foreach (Resource.RRequestHead request in tradeRoute.ListRequestHead)
            //     {
            //         if (request.Request < 0) { continue; } // No exports yet.
            //         request.Respond(tradeRoute.Tail.Trade.outboundShipDemand.Fraction() * request.Request);
            //     }
            // }

            // foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger) //.Where(x => x.Key < 500)
            // {

            //     double total = kvp.Value.ResourceLocal.Sum + kvp.Value.RequestFromChildren.Sum(x => x.Sum);

            //     // Divide total resources amoung local.
            //     double resourceLeftovers = ShareResources(total, kvp.Value.RequestLocal);

            //     //If not enough resources to supply local, not enough to export.
            //     if (resourceLeftovers == 0) { continue; }

            //     // Remaining resources to fulfill exports.
            //     resourceLeftovers = ShareResources(resourceLeftovers, installation.Trade.DownlineTraderoutes.Where(x => x.ListRequestHead.ContainsKey(kvp.Key)).Select(x => x.ListRequestHead[kvp.Key]));

            //     // Then store remainder

            // }
            foreach (TradeRoute child in installation.Trade.DownlineTraderoutes)
            {
                CalculateResources(child.Tail);
            }
        }

        /// <summary>
        /// Given a number resources, allocates evenly among requesters.
        /// </summary>
        /// <param name="resourceTotal"></param>
        /// <param name="requesters"></param>
        /// <param name="resourceShippingFraction"></param>
        /// <returns>
        ///  leftover
        /// </returns>
        static double ShareResources(double resourceTotal, IEnumerable<Resource.IRequestable> requesters, double resourceShippingFraction=1)
        {
            // Supply fraction is the fraction of resources that can be allocated.
            double requestTotal = requesters.Sum(x => x.Request);
            double newResourceTotal = resourceTotal;

            // Supply fraction used is lowest of available and ship constraints.
            double resourceSupplyFraction = resourceTotal / -requestTotal;

            foreach (Resource.IRequestable r in requesters)
            {
                // Respond with fraction.
                // double resourceShippingFraction = 1;
                // if (r is Resource.RRequestHead)
                // {
                //     resourceShippingFraction = ((Resource.RRequestHead)r).tradeRoute.Fraction();
                // }
                double allocated = Math.Min(Math.Min(resourceSupplyFraction, resourceShippingFraction), 1) * r.Request;
                newResourceTotal -= allocated;
                r.Respond(allocated);
                // newResourceTotal -= allocated;
            }
            return newResourceTotal;
        }
    }

}
