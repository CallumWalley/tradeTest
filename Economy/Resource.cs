using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class Resource
{
    public Logger logger;

    // Interfaces
    /// <summary>
    /// Basic type representing a resource.
    /// </summary>
    public interface IResource
    // Interface for displaying basic resource icon.
    {
        int Type { get; }
        string Details { get; }
        string Name { get; }
        double Sum { get; }

        /// <summary> How much is being requested DOES NOT REPRESENT ACTUAL RESOURCE </summary>
        public double Request { get; set; }
        // No inputs if request fulfilled.

        /// <summary> Represents current state of request </summary>
        public int State { get; set; }
        /// 0 fulfilled.
        //  1 partially fulfilled.
        //  2 unfulfilled.


        /// Display 
        public string ValueFormat { get; set; }
        public bool IsHidden { get; set; }


        /// <summary>
        /// Sets the resource value.
        /// </summary>
        public abstract void Respond();
        // No fulfilled value returned if not fulfilled.
        public abstract void Respond(double value);

        public double Fraction();
    }

    /// <summary>
    ///  A collection of multiple resources of the same type. Enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceGroup<out T> : IEnumerable<T>, IResource where T : IResource
    {
        // public IEnumerator<T> GetEnumerator();
        public int Count { get; }
    }


    /// <summary>
    ///  A static resource is the most simple resource. It will be a number and stay that way until changed.
    /// </summary>
    public partial class RStatic : IResource
    {
        public virtual double Sum { get; protected set; }
        public int Type { get; }
        public virtual string Details { get; set; } = "Base value";
        public virtual string Name { get; set; } = "Unknown";
        public virtual double Request { get; set; }
        public bool IsHidden { get; set; } = false;
        public string ValueFormat { get; set; } = "{0:G}";


        // Request is actual amount given
        public virtual int State { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_sum"></param>
        /// <param name="_request"></param>
        /// <param name="_fulfilled">If true, base static will be set to request.</param>
        /// <param name="_name"></param>
        /// <param name="_details"></param>

        public RStatic(int _type = 0, double _sum = 0, double _request = 0, string _name = "Unknown", string _details = "Base value") : base()
        {
            Sum = _sum;
            Details = _details;
            Name = _name;
            Type = _type;
            Request = _request;
        }
        public RStatic() { }

        public static double operator -(RStatic a, RStatic b)
        {
            return a.Sum - b.Sum;
        }
        public virtual void Set(double newValue)
        {
            Sum = Math.Round(newValue, 2);
        }

        public virtual void Add(double adder)
        {
            Sum = Sum += adder;
        }

        // No inputs if request fulfilled.
        public virtual void Respond()
        {
            Respond(Request);
        }
        // No fulfilled value returned if not fulfilled.
        public virtual void Respond(double value)
        {
            Set(value);
            State = (value == Request) ? 0 : 1;
        }
        public virtual double Fraction()
        {
            return (Sum / Request);
        }
        public override string ToString()
        {
            return $"{Name}:{Sum}/{Request}";
        }

        public static void Clear() { return; }
        public Texture2D Icon { get { return Index(Type).icon; } }

        // public double ShipWeight { get { return Index(type).shipWeight; } }
        public bool Storable { get { return Index(Type).storable; } }


    }
    /// <summary>
    /// Group of multiple resources added/summed together.
    /// </summary>
    public partial class RGroup<T> : IResourceGroup<IResource> where T : IResource
    {

        public RGroup()
        {
            _adders = new();
            _muxxers = new();
        }

        public RGroup(string _name = "Sum", string _details = "Sum") : this()
        {
            Name = _name;
            Details = _details;
        }

        public RGroup(IEnumerable<T> _add, string _name = "Sum", string _details = "Sum") : this(_name, _details)
        {
            _adders = (List<T>)_add;
        }
        public RGroup(T _add, string _name = "Sum", string _details = "Sum") : this(_name, _details)
        {
            _adders = new();
            _adders.Add(_add);
        }
        public int Type
        {
            get { return (_adders.Count > 0) ? _adders.First<T>().Type : 0; }
        }
        public string Details { get; set; } = "Sum";
        public string Name { get; set; } = "Sum";

        public bool IsHidden { get; set; } = false;
        public string ValueFormat { get; set; } = "{0:G}";
        protected List<T> _adders { get; set; }
        protected List<T> _muxxers { get; set; }

        public IEnumerator<IResource> GetEnumerator()
        {
            foreach (T element in _adders)
            {
                yield return element;
            }
            foreach (T element in _muxxers)
            {
                yield return element;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Does not consider grandchild members.
        public int Count
        {
            get { return (_adders.Count + _muxxers.Count); }
        }
        public double Sum
        {
            get { return _AdderSubtotal() * _MuxxerSubtotal(); }
        }

        double _AdderSubtotal()
        {
            return (_adders.Count > 0) ? _adders.Sum(x => x.Sum) : 0;
        }
        double _MuxxerSubtotal()
        {
            double agg = 1;
            foreach (T muxxer in _muxxers) { agg *= muxxer.Sum; }
            return agg;
        }
        public void Add(T ra)
        {
            _adders.Add(ra);
        }
        public void Mux(T ra)
        {
            _muxxers.Add(ra);
        }
        public void UnAdd(T ra)
        {
            _adders.Remove(ra);
        }
        public void UnMux(T ra)
        {
            _muxxers.Remove(ra);
        }
        public IResource First()
        {
            return _adders[0];
        }
        public void Set(double newValue)
        {
            { throw new InvalidOperationException("Set method is not valid for groups"); }
        }
        public void Clear()
        {
            _adders.Clear();
        }

        public override string ToString()
        {
            return $"{Name}:{Sum}";
        }


        // Request is a second
        public double Request
        {
            get
            {
                double requestCum = 0;
                foreach (IResource i in _adders)
                {
                    requestCum += i.Request;
                }
                return requestCum;
            }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }

        public void Respond()
        {
            Respond(Request);
        }
        public void Respond(double value)

        {
            double fraction = value / Request;
            foreach (IResource i in _adders)
            {
                i.Respond(fraction * i.Request);
            }
        }

        // Request is actual amount given
        public int State
        {
            get { return _adders.Sum(x => ((IResource)x).State); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }

        public double Fraction()
        {
            return Sum / Request;
        }
    }


    // public partial class RStorage : RGroup, IResource
    // {
    //     double stockpile; //Amount in this storage.
    //     public RStorage(int type) : base(type) { stockpile = 0; }
    //     public RStorage(int _type, List<IResource> _add, double initValue = 0) : base(_type, _add)
    //     {
    //         stockpile = initValue;
    //     }

    //     public double Deposit(double value)
    //     {
    //         // returns amount space free.
    //         // if negative, this is resource that didn't fit.
    //         double leftover = Free() - value;
    //         stockpile = Mathf.Min(Sum, value + stockpile);
    //         return leftover;
    //     }
    //     public double Withdraw(double value)
    //     {
    //         // returns amount space free.
    //         // if negative, this is resource that didn't fit.
    //         stockpile = Mathf.Min(Sum, value + stockpile);
    //         return Deposit(-value);
    //     }
    //     public double Free()
    //     {
    //         return Sum - stockpile;
    //     }
    //     public double Stock()
    //     {
    //         return stockpile;
    //     }
    //     public void Fill()
    //     {
    //         stockpile = Sum;
    //     }
    //     public override string ToString()
    //     {
    //         return $"{Name}:{Stock()}/{Sum}";
    //     }


    // public partial class RStaticLinear : RStatic
    // {
    //     // Linear Request.  Modifies outputs to be same as fraction of inputs received.
    //     Resource.RStatic multiplier;
    //     Industry industry;

    //     public RStaticLinear(Industry _industry, Resource.RStatic _request) : base(_request)
    //     {
    //         multiplier = new Resource.RStatic(Type, 0);
    //         // ((Resource.RGroup<IResource>)_industry.Production[Type]).Multiply(multiplier);
    //         _industry.AddSituation(new Situations.OutputModifier(this, multiplier, _industry));
    //     }
    //     public override void Respond()
    //     {
    //         base.Respond();
    //         multiplier.Set(1f);
    //     }
    //     public override void Respond(double value)
    //     {
    //         base.Respond(value);
    //         multiplier.Set((value - Request.Sum) / Request.Sum);
    //     }
    // }
    // public partial class RGroup<TResource> : RGroup<TResource> where TResource : IResource
    // {
    //     public RGroup(int _type, IEnumerable<TResource> _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
    //     public double SumRequest { get { return _SumRequests()[1]; } }
    //     double[] _SumRequests()
    //     // Do not net to call cum seperately.
    //     {
    //         double addCum = 0;
    //         double multiCum = 1;
    //         double requestCum = 0;
    //         foreach (TResource i in Adders)
    //         {
    //             addCum += i.Sum;
    //             requestCum += i.Request;
    //         }
    //         foreach (TResource i in Muxxers)
    //         {
    //             multiCum += i.Sum;
    //         }
    //         return new double[] { addCum * multiCum, requestCum * multiCum };
    //     }
    // }
    // // Small class to handle 'request, vs receive'
    // public class Requester
    // {

    //     public Resource.IResource.RStatic request;
    //     public int Type { get { return request .Type; } }
    //     public double Sum { get { return request.Sum; } }
    //     public Resource.IResource.RStatic Response { get; set; }
    //     public Requester(Resource.RStatic _request)
    //     {
    //         request = _request;
    //     }
    //     public void Respond(Resource.RStatic _response, int _status)
    //     {
    //         Response = _response;
    //     }
    // }


    public struct ResourceType
    {
        public string name;
        public Texture2D icon;

        // If this resrource is something that can be store, or only instant;
        public bool storable;

        public ResourceType(string _name, Texture2D _icon, bool _storable)
        {
            name = _name;
            icon = _icon;
            storable = _storable;
        }
    }
    /// <summary>
    ///  This could be JSONified
    /// </summary>
    static Dictionary<int, ResourceType> _index = new Dictionary<int, ResourceType>(){
            {0, new ResourceType("Unset", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {1, new ResourceType("Minerals", GD.Load<Texture2D>("res://assets/icons/resources/minerals.dds"), true)},
            {2, new ResourceType("Fuel", GD.Load<Texture2D>("res://assets/icons/resources/energy.dds"), true)},
            {3, new ResourceType("Food", GD.Load<Texture2D>("res://assets/icons/resources/food.dds"),  true)},
            {4, new ResourceType("H2O", GD.Load<Texture2D>("res://assets/icons/resources/h2o.png"),  true)},
            {801, new ResourceType("Fulfillment", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {802, new ResourceType("Capability", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {811, new ResourceType("Freighter", GD.Load<Texture2D>("res://assets/icons/freighter.png"), false)},

            // {801, new ResourceType("Operational Capacity", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {802, new ResourceType("Capacity Utilisation", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {803, new ResourceType("Efficiency", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {901, new ResourceType("Scale", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},

        };


    /// <summary>
    /// Returns resource type for given resourceid
    /// </summary>
    /// <param name="resourceid"></param>
    /// <returns></returns>
    public static ResourceType Index(int resourceid)
    {
        return _index[resourceid];
    }
    /// <summary>
    /// Returns icon for given resource id.
    /// </summary>
    /// <param name="resourceid"></param>
    /// <returns></returns>
    public static Texture2D Icon(int resourceid)
    {
        return _index[resourceid].icon;
    }

    ///<summary>
    ///Returns generic name of a resource for a specified resourceid.
    ///</summary>
    public static string Name(int resourceid)
    {
        return _index[resourceid].name;
    }
    // public static double ShipWeight(int resourceCode)
    // {
    //     return _index[resourceCode].shipWeight;
    // }



    /// <summary>
    /// Dictionary of resources, keyed by resource id
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public partial class RDict<TResource> : IEnumerable<TResource> where TResource : IResource, new()
    {
        // if true, element will be created rather than returning null
        public bool CreateMissing = false;

        // Returns standard resources.
        public IEnumerable<TResource> Standard
        {
            get
            {
                return GetStandard();
            }
        }

        // TResource list is a list of resources...
        protected SortedDictionary<int, TResource> members;
        public RDict(IEnumerable<TResource> _members)
        {
            members = new SortedDictionary<int, TResource>();
            foreach (TResource resource in _members)
            {
                members.Add(resource.Type, resource);
            }
        }
        public RDict(TResource resource)
        {
            members = new SortedDictionary<int, TResource>() { { resource.Type, resource } };
        }
        public RDict(SortedDictionary<int, TResource> _members)
        {
            members = _members;
        }
        public RDict()
        {
            members = new SortedDictionary<int, TResource>();
        }
        // If true, top level elements will be added as static
        public TResource this[int index]
        {
            get { return _Get(index); }
            set { _Set(index, value); }
        }

        public virtual void _Set(int index, TResource value)
        {
            members.Add(index, value);
        }

        public void Add(TResource r)
        {
            members.Add(r.Type, r);
        }

        IEnumerable<TResource> GetStandard()
        {
            return GetRange(1, 100);
        }
        protected virtual TResource _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                members[index] = new();
            }
            return members[index];
        }
        public IEnumerator<TResource> GetEnumerator()
        {
            return members.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<TResource> GetRange(int min, int max)
        {
            foreach (int k in members.Keys)
            {
                if (min <= k && k <= max)
                {
                    yield return members[k];
                }
            }
        }
        public virtual void Clear()
        {
            members.Clear();
        }
        public bool ContainsKey(int key)
        {
            return members.ContainsKey(key);
        }
        public void Remove(TResource r)
        {
            foreach (KeyValuePair<int, TResource> kvp in members)
            {
                if (kvp.Value.Equals(r))
                {
                    members.Remove(kvp.Key);
                    return;
                }
            }
        }
        public override string ToString()
        {
            string returnString = "";
            foreach (TResource r in members.Values)
            {
                returnString += r.ToString() + ", ";
            }
            return returnString;
        }

        public RDict<TResource> Clone()
        {
            RDict<TResource> newRlist = new();

            foreach (KeyValuePair<int, TResource> kvp in members)
            {
                newRlist[kvp.Key] = kvp.Value;
            }

            return newRlist;
        }

    }
    // A list but the top level elements must be groups.

    // /// <summary>
    // /// Child class of resource list with that constructs resources with correct descriptions.
    // /// </summary>
    // public class TradeRouteResourceList : RStaticList
    // {
    //     string name;
    //     string description;
    //     public TradeRouteResourceList(string _name, string _description)
    //     {
    //         name = _name; description = _description;
    //     }
    //     protected override RStatic CreateNewMember(int index)
    //     {
    //         return new RStatic(index, 0, string.Format(name, Name(index)), string.Format(description, Name(index)));
    //     }
    // }
    // public partial class RGroupList<TResource> : RList<RGroup<TResource>> where TResource : IResource, new()
    // {
    //     // Same as RList, except all elements are groups.
    //     public RGroupList() : base() { }
    //     public RGroupList(RGroup<TResource> rGroup) : base(rGroup) { }

    //     // Currently if is inited with only groups, they will become the members.
    //     public RGroupList(TResource resource) : base((new RGroup<TResource>(resource))) { }
    //     public RGroupList(IEnumerable<TResource> _members) : base()
    //     {
    //         foreach (TResource m in _members)
    //         {
    //             members.Add(m.Type, new RGroup<TResource>(m));
    //         }
    //     }

    //     // override default clear behavior.
    //     // keep top level groups.
    //     public override void Clear()
    //     {
    //         foreach (KeyValuePair<int, RGroup<TResource>> member in members)
    //         {
    //             (member.Value).Clear();
    //         }
    //     }
    //     public void Insert(TResource r)
    //     {
    //         if (!members.ContainsKey(r.Type))
    //         {
    //             members[r.Type] = new RGroup<TResource>("Sum", "Sum");
    //         }
    //         else
    //         {
    //             this[r.Type].Add(r);
    //         }
    //     }
    // }


    // public partial class RStorageList<TResource> : RList<TResource> where TResource : RStorage
    // {
    //     protected override TResource _Get(int index)
    //     {
    //         if (!members.ContainsKey(index))
    //         {
    //             members[index] = (TResource)new RStorage(index);
    //         }
    //         return members[index];
    //     }
    //     public override void Clear()
    //     {
    //         foreach (KeyValuePair<int, TResource> member in members)
    //         {
    //             (member.Value).Clear();
    //         }
    //     }
    // }
    public partial class RDictStatic : RDict<RStatic>
    {
        protected override RStatic _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                members[index] = CreateNewMember(index);
            }
            return members[index];
        }
        protected virtual RStatic CreateNewMember(int index)
        {
            return new RStatic(index, 0);
        }
    }
    public class Ledger : IEnumerable<KeyValuePair<int, Ledger.Entry>>
    {

        // Consider moving to ResourcePool.

        /// <summary>
        /// Ledger is the complete list of all the resources at a specific location. 
        /// It is sparce on the resource axis, but dense on the type axis.
        /// </summary>
        public ResourcePool ResourcePool;
        /// Dummy method to make sure resource exists.

        public Dictionary<int, RStatic> Storage
        {
            get
            {
                Dictionary<int, RStatic> d = new Dictionary<int, RStatic>();
                foreach (KeyValuePair<int, Entry> kvp in this.Where(x => x.Key < 500))
                {
                    d[kvp.Key] = ((EntryAccrul)kvp.Value).Stored;
                }
                return d;
            }
        }

        public class Entry
        {
            /// <summary>
            /// Represents a resource, and a place. Can be either request or resource.
            /// </summary>
            public Ledger ledger;

            // Net is combo of Resource and Request.
            public RGroup<IResource> LocalGain; // How much is produced here.
            public RGroup<IResource> LocalLoss;

            // These are only collections of 'real' elements.
            RGroup<IResource> localNet;
            RGroup<IResource> tradeNet;
            RGroup<IResource> net;

            public IResource UplineLoss
            {
                get { return ledger.ResourcePool.Trade.UplineTraderoute == null ? null : ledger.ResourcePool.Trade.UplineTraderoute.ListTailLoss.ContainsKey(Type) ? ledger.ResourcePool.Trade.UplineTraderoute.ListTailLoss[Type] : null; }
            }
            public IResource UplineGain
            {
                get { return ledger.ResourcePool.Trade.UplineTraderoute == null ? null : ledger.ResourcePool.Trade.UplineTraderoute.ListTailGain.ContainsKey(Type) ? ledger.ResourcePool.Trade.UplineTraderoute.ListTailGain[Type] : null; }
            }
            public IEnumerable<IResource> DownlineGain
            {
                get
                {
                    foreach (TradeRoute tradeRoute in ledger.ResourcePool.Trade.DownlineTraderoutes)
                    {
                        if (tradeRoute.ListHeadGain.ContainsKey(Type))
                        {
                            yield return tradeRoute.ListHeadGain[Type];
                        }
                    }
                }
            }
            public IEnumerable<IResource> DownlineLoss
            {
                get
                {
                    foreach (TradeRoute tradeRoute in ledger.ResourcePool.Trade.DownlineTraderoutes)
                    {
                        if (tradeRoute.ListHeadLoss.ContainsKey(Type))
                        {
                            yield return tradeRoute.ListHeadLoss[Type];
                        }
                    }
                }
            }
            public RGroup<IResource> TradeNet
            {
                get
                {
                    tradeNet.Clear();
                    foreach (IResource item in DownlineGain)
                    {
                        tradeNet.Add(item);
                    }
                    if (UplineLoss != null)
                    {
                        tradeNet.Add(UplineLoss);
                    }
                    foreach (IResource item in DownlineLoss)
                    {
                        tradeNet.Add(item);
                    }
                    if (UplineGain != null)
                    {
                        tradeNet.Add(UplineGain);
                    }
                    return tradeNet;
                }
            }
            public RGroup<IResource> LocalNet
            {
                get
                {
                    localNet.Clear();
                    foreach (IResource item in LocalLoss)
                    {
                        localNet.Add(item);
                    }
                    foreach (IResource item in LocalGain)
                    {
                        localNet.Add(item);
                    }
                    return localNet;
                }
            }
            public RGroup<IResource> Net
            {
                get
                {
                    net.Clear();
                    net.Add(LocalNet);
                    net.Add(TradeNet);
                    return net;
                }
            }


            public int Type { get; set; }
            public Entry(int _type)
            {
                localNet = new("Local", "All production and consumption occuring at this ResourcePool.");
                net = new("Total", "All resources produced or traded");
                tradeNet = new("Trade", "All Exports and Imports to this ResourcePool");

                //Local = 
                LocalGain = new RGroup<IResource>("Local Production", "Sum Produced");
                LocalLoss = new RGroup<IResource>("Local Consumption", "Sum Requested");

                Type = _type;
            }

            public void Clear()
            {
                LocalGain.Clear();
                LocalLoss.Clear();
                // RequestLocal.Clear();
                // ResourceParent.Set(0);
                // RequestParent.Request = 0;
                // RequestChildren.Clear();
                // ResourceChildren.Clear();
                // NetLocal.Clear();
            }
            /// <summary>
            /// Return flat requested and actual, for buffer.
            /// </summary>
            /// <returns></returns>
            // public double Net()
            // {
            //     return Production.Sum + Surplus.Sum + Consumption.Sum + Demand.Sum;
            // }

            public override string ToString()
            {
                return $"{Resource.Name(Type)}: {LocalNet.Sum} {TradeNet.Sum} {Net.Sum}";
            }
        }

        public class EntryAccrul : Entry
        {
            public Resource.RStatic Stored;
            public Resource.RStatic Capacity;
            public Resource.RStatic Delta;

            public EntryAccrul(int _type) : base(_type)
            {
                Stored = new Resource.RStatic(_type, 0, 0, "Stored", string.Format("{0} stored here.", Name(_type)));
                Capacity = new Resource.RStatic(_type, 1000, 1000, "Capacity", string.Format("{0} capacity.", Name(_type)));
                Delta = new Resource.RStatic(_type, 1000, 1000, "Change", string.Format("{0} change.", Name(_type)));
            }

            // Attempt to withdraw amount, return actual.
            public double Withdraw(double amount)
            {
                // Max withdrawl is stored, 
                // Max deposit is available storage.
                Delta.Set(Mathf.Min((Mathf.Max(amount, -Stored.Sum)), Capacity - Stored));
                Stored.Add(Delta.Sum);

                return -Delta.Sum;
            }
        }


        // public IEnumerable<KeyValuePair<int, EntryAccrul>> Acrul()
        // {
        //     foreach (KeyValuePair<int, EntryAccrul> e in _acrul)
        //     {
        //         yield return e;
        //     }
        // }

        Dictionary<int, Entry> _all = new();
        /// <summary>
        ///  Dict containting only acrulable;
        /// </summary>
        // Dictionary<int, EntryAccrul> _acrul = new();


        public void Clear()
        {
            foreach (KeyValuePair<int, Ledger.Entry> kvp in _all)
            {
                kvp.Value.Clear();
            }
            // foreach (KeyValuePair<int, Ledger.EntryAccrul> kvp in _accrul)
            // {
            //     kvp.Value.Clear();
            // }
        }

        public Entry this[int type]

        {
            get
            {
                if (_all.ContainsKey(type))
                {
                    return _all[type];
                }
                else
                {
                    return InitType(type);
                }
            }
        }

        // create new resource type in ledger.
        public Entry InitType(int type)
        {
            Entry nre;
            // If type less than 500, it is accrul.
            if (type < 500)
            {
                nre = new EntryAccrul(type);
                nre.ledger = this;
            }
            else
            {
                nre = new Entry(type);
                nre.ledger = this;
            }
            _all[type] = nre;
            return nre;
        }

        public IEnumerator<KeyValuePair<int, Entry>> GetEnumerator()
        {
            foreach (KeyValuePair<int, Entry> element in _all)
            {
                yield return element;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Entry> Values()
        {
            foreach (KeyValuePair<int, Ledger.Entry> kvp in _all)
            {
                yield return kvp.Value;
            }
        }
        public override string ToString()
        {
            return string.Join("\n", _all.Select(x => x.Value.ToString()).ToArray());
        }


        public bool ContainsKey(int key)
        {
            return _all.ContainsKey(key);
        }
    }
}