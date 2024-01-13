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

            UpdateLedger(installation);

            if (installation.Order > 1) { return; }

            CalculateRequests(installation);
        }
        public static void EFrameLate(Installation installation)
        {
            if (installation.Order > 1) { return; }
            ResolveRequests(installation);
        }
        /// <summary>
        /// This will go down the tree and work out unfulfilled demand for each child.
        /// </summary>
        /// <param name="installation"></param>
        static void CalculateRequests(Installation installation)
        {
            // For each child trade route
            foreach (TradeRoute downline in installation.Trade.DownlineTraderoutes)
            {
                // call this funtion on child.
                CalculateRequests(downline.Tail);
                // Add ship demand to head.
                installation.Ledger[901].LocalLoss.Add(downline.ShipDemand);
            }

            // If no upline, end here.
            if (installation.Trade.UplineTraderoute == null)
            {
                return;
            }

            // For each element in ledger calculate net resource and set ImportDemand / Export appropriately.
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
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

                double def = installation.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Sum) +
                installation.Trade.DownlineTraderoutes.Where(x => x.ListHeadGain.ContainsKey(kvp.Key)).Sum(x => x.ListHeadGain[kvp.Key].Request) +
                kvp.Value.LocalLoss.Request + kvp.Value.LocalGain.Sum;

                // Request the difference from parent.
                installation.Trade.UplineTraderoute.SetValue(kvp.Key, def);
            }
        }
        /// <summary>
        ///  Once all elements have 'CalculateRequests' this is run.
        /// </summary>
        /// <param name="installation"></param>
        static void ResolveRequests(Installation installation)
        {
            /// <summary>
            /// Fraction inbound trade can be fulfilled.
            /// </summary>
            double freightFraction = 1;

            // If has upline requesting ships, I Should always resolve as I set the request.
            if (installation.Ledger.ContainsKey(901) && installation.Ledger[901].UplineLoss != null)
            {
                installation.Ledger[901].UplineLoss.Respond();
            }

            // If has downline.
            // if (installation.Trade.DownlineTraderoutes.Count > 0)
            // {
            //     freightFraction = Math.Min(installation.Ledger[901].RequestLocal.Fraction(), 1);
            // }
            // First resolve ship balance.
            // if importing ships that always resolved.

            // enumerate through elements in ledger 
            foreach (KeyValuePair<int, Resource.Ledger.Entry> kvp in installation.Ledger)
            {
                // Start tally with parent exports as they always get thru.
                double tally = kvp.Value.LocalGain.Sum;

                if (kvp.Value.UplineGain != null)
                {
                    tally += kvp.Value.UplineGain.Sum;
                }
                // Step One. Approve imports from children.
                foreach (var r in kvp.Value.DownlineGain)
                {
                    double alloc = r.Request * freightFraction;
                    tally += alloc;
                    r.Respond(alloc);
                }

                // Step Two. Calculate storage withdrawl for local use.

                // Shortfall is how much extra resource required to fulfill.
                // double shortfall = kvp.Value.Net.Sum - kvp.Value.RequestLocal.Request;
                // Set storage element to cover difference. (if allowed)
                // TODO
                // if shortfall > 0
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

            foreach (TradeRoute child in installation.Trade.DownlineTraderoutes)
            {
                ResolveRequests(child.Tail);
            }
        }
    }


    public static void UpdateLedger(Installation installation)
    {
        // Zero Ledger
        installation.Ledger.Clear();

        foreach (Industry rp in installation.Industries.GetChildren())
        {
            foreach (Resource.IRequestable output in rp.Production)
            {
                installation.Ledger[output.Type].LocalGain.Add(output);
            }
        }

        foreach (Industry rp in installation.Industries.GetChildren())
        {
            foreach (Resource.IRequestable input in rp.Consumption)
            {
                installation.Ledger[input.Type].LocalLoss.Add(input);
            }
        }
    }
}
