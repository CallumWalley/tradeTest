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
        public void EFrameEarly(Domain Domain);
        public void EFrameLate(Domain Domain);
    }

    public static class ExportToParent //: ISystem
    {
        public static void EFrameEarly(Domain Domain)
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
            Domain.Ledger.Clear();

            foreach (FeatureBase rp in Domain.GetChildren().Cast<FeatureBase>())
            {
                foreach (Resource.IResource f in rp.FactorsGlobalOutput)
                {
                    Domain.Ledger[f.Type].LocalGain.Add(f);
                    Domain.Ledger[f.Type].LocalNet.Add(f);
                }
                foreach (Resource.IResource f in rp.FactorsGlobalInput)
                {
                    Domain.Ledger[f.Type].LocalLoss.Add(f);
                    Domain.Ledger[f.Type].LocalNet.Add(f);
                }
            }
            if (Domain.Order > 1) { return; }

            CalculateRequests(Domain);
        }
        public static void EFrameLate(Domain Domain)
        {
            if (Domain.Order > 1) { return; }
            ResolveRequests(Domain);
            foreach (FeatureBase f in Domain)
            {
                f.OnEFrame();
            }
        }
        /// <summary>
        /// This will go down the tree and work out unfulfilled demand for each child.
        /// </summary>
        /// <param name="Domain"></param>
        static void CalculateRequests(Domain Domain)
        {


            // For each child trade route
            foreach (TradeRoute downline in Domain.Trade.DownlineTraderoutes)
            {
                // call this funtion on child.
                CalculateRequests(downline.Tail);
                // Add ship demand to head.
                Domain.Ledger[811].LocalLoss.Add(downline.ShipDemand);
                Domain.Ledger[811].LocalNet.Add(downline.ShipDemand);
            }

            // // If no upline, end here.
            // if (Domain.Trade.UplineTraderoute == null)
            // {
            //     return;
            // }

            // Change trade request according to own logic.
            Domain.Trade.DownlineTraderoutes.ForEach(x => x.SetRequest());

            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in Domain.Ledger)
            {
                // Need to zero out upline from previous upline.
                kvp.Value.TradeNet.Clear();
            }
            // Update trade list.
            foreach (var tr in Domain.Trade.DownlineTraderoutes)
            {
                foreach (var r in tr.ListHead)
                {
                    Domain.Ledger[r.Type].TradeNet.Add(r);
                }
            }

            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in Domain.Ledger)
            {
                kvp.Value.Upline = -(
                    kvp.Value.LocalLoss.Request +
                    kvp.Value.LocalGain.Request +
                    kvp.Value.TradeNet.Request);
            }
        }
        /// <summary>
        ///  Once all elements have 'CalculateRequests' this is run.
        /// </summary>
        /// <param name="Domain"></param>
        static void ResolveRequests(Domain Domain)
        {
            /// <summary>
            /// Fraction inbound trade can be fulfilled.
            /// </summary>

            // tranch0
            // Resolve all uplines
            // tranch1
            // Resolve all local positive values
            // tranch2
            // Resolve all downlines

            double freightFraction = 0;


            // // If has upline requesting ships, I Should always resolve as I set the request.
            // if (Domain.Ledger.ContainsKey(811) && Domain.Ledger[811].Upline != null)
            // {
            //     Domain.Ledger[811].UplineLoss.Respond();
            // }

            // If has downline.
            if (Domain.Trade.DownlineTraderoutes.Count > 0)
            {
                freightFraction = Math.Min(Domain.Ledger[811].LocalNet.Fraction(), 1);
            }

            // First resolve ship balance.
            // if importing ships that always resolved.

            // enumerate through elements in ledger 
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in Domain.Ledger.Reverse())
            {
                // Start tally with parent exports as they always get thru.
                double tally = kvp.Value.LocalGain.Sum;
                double def = kvp.Value.LocalLoss.Request;


                // Get approved materials from upline.
                if (Domain.Trade.UplineTraderoute != null && Domain.Trade.UplineTraderoute.ListTail.ContainsKey(kvp.Key))
                {
                    tally += Domain.Trade.UplineTraderoute.ListTail[kvp.Key].Sum;
                }

                // // Step One. Approve imports from children. (equal to available ships)
                // foreach (var r in kvp.Value.DownlineGain)
                // {
                //     double alloc = r.Request * freightFraction;
                //     tally += alloc;
                //     r.Respond(alloc);
                // }

                // Step Two. Calculate storage withdrawl for local use.

                // Shortfall is how much extra resource required to fulfill.
                // Set storage element to cover difference. (if allowed)

                // whether store resourses are needed for tranch2

                if (kvp.Value is Resource.Ledger.EntryAccrul && (tally + def) < 0)
                {
                    tally += ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw((tally + def));
                }
                //  ...
                // recalculate shortfall.

                // Step Three. Allocate resources locally.

                // How much of this request can be filled.
                double resourceSupplyFraction = (tally + def < 0) ? Math.Max(Math.Min(-tally / def, 1), 0) : 1;

                foreach (Resource.IResource r in kvp.Value.LocalLoss)
                {
                    double alloc = r.Request * resourceSupplyFraction;
                    r.Respond(alloc);
                    tally += alloc;
                    def -= alloc;
                }

                def += Domain.Trade.DownlineTraderoutes.Where(x => x.ListHead.ContainsKey(kvp.Key)).Sum(x => x.ListHead[kvp.Key].Request);


                // Step Four. Calculate storage withdrawl.
                if (kvp.Value is Resource.Ledger.EntryAccrul && tally + def < 0)
                {
                    tally += ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw(tally + def);
                }


                // Recaclculate resourceSupplyFractionfor trade.
                // Step three trade
                resourceSupplyFraction = (def < 0) ? Math.Max(Math.Min(-tally / def, 1), 0) : 1;
                resourceSupplyFraction = Math.Min(resourceSupplyFraction, freightFraction);

                // Set storage element to cover difference. (if allowed)
                // TODO
                // if shortfall > 0
                //  ...
                // recalculate shortfall.


                foreach (Resource.IResource r in kvp.Value.TradeNet)
                {
                    double alloc = r.Request * resourceSupplyFraction;
                    r.Respond(r.Request * resourceSupplyFraction);
                    tally += alloc;
                }

                // Deposit extra.
                if (kvp.Value is Resource.Ledger.EntryAccrul && tally > 0)
                {
                    ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw(tally);
                }
            }
            if (Domain.Trade.UplineTraderoute != null)
            {
                foreach (var r in Domain.Trade.UplineTraderoute.ListTail)
                {
                    Domain.Ledger[r.Type].TradeNet.Add(r);
                }
            }
            foreach (TradeRoute child in Domain.Trade.DownlineTraderoutes)
            {
                ResolveRequests(child.Tail);
            }
        }
    }

    // public static void UpdateTradeDownline(Domain Domain)
    // {
    //     foreach (KeyValuePair<int, Resource.Ledger.Entry> entry in Domain.Ledger)
    //     {
    //         entry.Value.DownlineGain.Clear();
    //         entry.Value.DownlineLoss.Clear();
    //     }
    //     foreach (TradeRoute tradeRoute in Domain.Trade.DownlineTraderoutes)
    //     {
    //         foreach (Resource.IResource r in tradeRoute.ListHeadLoss)
    //         {
    //             Domain.Ledger[r.Type].DownlineLoss.Add(r);
    //         }
    //         foreach (Resource.IResource r in tradeRoute.ListHeadGain)
    //         {
    //             Domain.Ledger[r.Type].DownlineGain.Add(r);
    //         }
    //     }
    // }
}
