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
        public void EFrameEarly(ResourcePool ResourcePool);
        public void EFrameLate(ResourcePool ResourcePool);
    }

    public static class ExportToParent //: ISystem
    {
        public static void EFrameEarly(ResourcePool ResourcePool)
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

            UpdateLedger(ResourcePool);

            if (ResourcePool.Order > 1) { return; }

            CalculateRequests(ResourcePool);
        }
        public static void EFrameLate(ResourcePool ResourcePool)
        {
            if (ResourcePool.Order > 1) { return; }
            ResolveRequests(ResourcePool);
        }
        /// <summary>
        /// This will go down the tree and work out unfulfilled demand for each child.
        /// </summary>
        /// <param name="ResourcePool"></param>
        static void CalculateRequests(ResourcePool ResourcePool)
        {
            // For each child trade route
            foreach (TradeRoute downline in ResourcePool.Trade.DownlineTraderoutes)
            {
                // call this funtion on child.
                CalculateRequests(downline.Tail);
                // Add ship demand to head.
                ResourcePool.Ledger[901].LocalLoss.Add(downline.ShipDemand);
            }

            // If no upline, end here.
            if (ResourcePool.Trade.UplineTraderoute == null)
            {
                return;
            }

            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in ResourcePool.Ledger)
            {
                double tally = kvp.Value.LocalGain.Sum;

                if (kvp.Value.UplineGain != null)
                {
                    tally += kvp.Value.UplineGain.Sum;
                }

                // Special rules for trade.
                // if (kvp.Key == 901)
                // {
                //     continue;
                // }

                // This could be made more efficient. Duplicating count.

                double def = ResourcePool.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Sum) +
                ResourcePool.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Request) +
                kvp.Value.LocalLoss.Request + kvp.Value.LocalGain.Sum;

                // Request the difference from parent.
                ResourcePool.Trade.UplineTraderoute.SetValue(kvp.Key, def);
            }
        }
        /// <summary>
        ///  Once all elements have 'CalculateRequests' this is run.
        /// </summary>
        /// <param name="ResourcePool"></param>
        static void ResolveRequests(ResourcePool ResourcePool)
        {
            /// <summary>
            /// Fraction inbound trade can be fulfilled.
            /// </summary>
            double freightFraction = 0;

            // If has upline requesting ships, I Should always resolve as I set the request.
            if (ResourcePool.Ledger.ContainsKey(901) && ResourcePool.Ledger[901].UplineLoss != null)
            {
                ResourcePool.Ledger[901].UplineLoss.Respond();
            }

            // If has downline.
            if (ResourcePool.Trade.DownlineTraderoutes.Count > 0)
            {
                freightFraction = Math.Min(ResourcePool.Ledger[901].TradeNet.Fraction(), 1);
            }
            // First resolve ship balance.
            // if importing ships that always resolved.

            // enumerate through elements in ledger 
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in ResourcePool.Ledger)
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
                // TODO
                if (kvp.Value is Resource.Ledger.EntryAccrul && kvp.Value.Net.Sum < 0)
                {                    
                    tally += ((Resource.Ledger.EntryAccrul)kvp.Value).Withdraw(kvp.Value.Net.Sum);
                }
                //  ...
                // recalculate shortfall.

                // Step Three. Allocate resources locally.

                // How much of this request can be filled.
                double resourceSupplyFraction = Math.Max(Math.Min(tally / -kvp.Value.LocalLoss.Request, 1), 0);

                foreach (Resource.IRequestable r in kvp.Value.LocalLoss)
                {
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

                foreach (Resource.IRequestable r in kvp.Value.DownlineLoss)
                {
                    double alloc = r.Request * resourceSupplyFraction;
                    r.Respond(r.Request * resourceSupplyFraction);
                    tally += alloc;

                }
            }

            foreach (TradeRoute child in ResourcePool.Trade.DownlineTraderoutes)
            {
                ResolveRequests(child.Tail);
            }
        }
    }


    public static void UpdateLedger(ResourcePool ResourcePool)
    {
        // Zero Ledger
        ResourcePool.Ledger.Clear();

        foreach (Feature rp in ResourcePool.GetChildren())
        {
            foreach (Resource.IRequestable f in rp.Factors)
            {
                if (f.Sum > 0)
                {
                    ResourcePool.Ledger[f.Type].LocalGain.Add(f);
                }
                else
                {
                    ResourcePool.Ledger[f.Type].LocalLoss.Add(f);
                }

            }
        }
    }
}
