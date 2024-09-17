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

            UpdateLedger(Domain);

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
            }

            // If no upline, end here.
            if (Domain.Trade.UplineTraderoute == null)
            {
                return;
            }

            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in Domain.Ledger)
            {
                double tally = kvp.Value.LocalGain.Sum;

                if (kvp.Value.UplineGain != null)
                {
                    tally += kvp.Value.UplineGain.Sum;
                }

                // Special rules for trade.
                // if (kvp.Key == 811)
                // {
                //     continue;
                // }

                // This could be made more efficient. Duplicating count.

                double def = Domain.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Sum) +
                Domain.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Request) +
                kvp.Value.LocalLoss.Request + kvp.Value.LocalGain.Sum;

                // Request the difference from parent.
                Domain.Trade.UplineTraderoute.SetValue(kvp.Key, def);
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
            double freightFraction = 0;

            // If has upline requesting ships, I Should always resolve as I set the request.
            if (Domain.Ledger.ContainsKey(811) && Domain.Ledger[811].UplineLoss != null)
            {
                Domain.Ledger[811].UplineLoss.Respond();
            }

            // If has downline.
            if (Domain.Trade.DownlineTraderoutes.Count > 0)
            {
                freightFraction = Math.Min(Domain.Ledger[811].TradeNet.Fraction(), 1);
            }
            // First resolve ship balance.
            // if importing ships that always resolved.

            // enumerate through elements in ledger 
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in Domain.Ledger)
            {
                // Start tally with parent exports as they always get thru.
                double tally = kvp.Value.LocalGain.Sum;

                if (kvp.Value.UplineGain != null)
                {
                    tally += kvp.Value.UplineGain.Sum;
                }
                // Step One. Approve imports from children. (equal to available ships)
                foreach (var r in kvp.Value.DownlineGain)
                {
                    double alloc = r.Request * freightFraction;
                    tally += alloc;
                    r.Respond(alloc);
                }

                // Step Two. Calculate storage withdrawl for local use.

                // Shortfall is how much extra resource required to fulfill.
                // Set storage element to cover difference. (if allowed)

                // whether store resourses are needed.

                if (kvp.Value is Resource.Ledger.EntryAccrul && kvp.Value.Net.Request < 0)
                {
                    tally += ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw(kvp.Value.Net.Request);
                }
                //  ...
                // recalculate shortfall.

                // Step Three. Allocate resources locally.

                // How much of this request can be filled.
                double resourceSupplyFraction = Math.Max(Math.Min(-tally / kvp.Value.LocalLoss.Request, 1), 0);

                foreach (Resource.IResource r in kvp.Value.LocalLoss)
                {
                    if (r.Request >= 0) { continue; }
                    double alloc = r.Request * resourceSupplyFraction;
                    r.Respond(alloc);
                    tally += alloc;
                }


                // Step Four. Calculate storage withdrawl.



                // Recaclculate resourceSupplyFractionfor trade.
                // Step three trade


                resourceSupplyFraction = Math.Min(Math.Min(tally, freightFraction), 1);

                // Set storage element to cover difference. (if allowed)
                // TODO
                // if shortfall > 0
                //  ...
                // recalculate shortfall.

                foreach (Resource.IResource r in kvp.Value.DownlineLoss)
                {
                    double alloc = r.Request * resourceSupplyFraction;
                    r.Respond(r.Request * resourceSupplyFraction);
                    tally += alloc;

                }

                if (kvp.Value is Resource.Ledger.EntryAccrul && kvp.Value.Net.Sum > 0)
                {
                    ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw(tally);
                }
            }

            foreach (TradeRoute child in Domain.Trade.DownlineTraderoutes)
            {
                ResolveRequests(child.Tail);
            }
        }
    }


    public static void UpdateLedger(Domain Domain)
    {
        // Zero Ledger
        Domain.Ledger.Clear();

        foreach (FeatureBase rp in Domain.GetChildren().Cast<FeatureBase>())
        {
            foreach (Resource.IResource f in rp.FactorsGlobalOutput)
            {
                Domain.Ledger[f.Type].LocalGain.Add(f);
            }
            foreach (Resource.IResource f in rp.FactorsGlobalInput)
            {
                Domain.Ledger[f.Type].LocalLoss.Add(f);
            }

        }
    }
}
