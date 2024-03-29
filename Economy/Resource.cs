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
    }
    /// <summary>
    ///  Basic type representing a resource request.
    /// </summary>
    public interface IRequestable : IResource
    {

        /// <summary> Represents current state of request </summary>
        public int State { get; set; }
        /// 0 fulfilled.
        //  1 partially fulfilled.
        //  2 unfulfilled.

        /// <summary> How much is being requested DOES NOT REPRESENT ACTUAL RESOURCE </summary>
        public double Request { get; set; }
        // No inputs if request fulfilled.
        /// <summary>
        /// Sets the resource value.
        /// </summary>
        public void Respond() { }
        // No fulfilled value returned if not fulfilled.
        public void Respond(double value) { }

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
        public virtual string Details { get; set; }
        public virtual string Name { get; set; }
        public RStatic(int _type = 0, double _sum = 0, string _name = "Unknown", string _details = "Base value")
        {
            Type = _type;
            Sum = _sum;
            Details = _details;
            Name = _name;
        }

        // Cast static to group.
        // public static implicit operator RGroup(RStatic rs)
        // {
        //     return new RGroup(rs.Type, new List<IResource> { rs });
        // }
        public virtual void Set(double newValue)
        {
            Sum = Math.Round(newValue, 2);
        }
        public static void Clear() { return; }
        public Texture2D Icon { get { return Index(Type).icon; } }

        // public double ShipWeight { get { return Index(type).shipWeight; } }
        public bool Storable { get { return Index(Type).storable; } }

        public override string ToString()
        {
            return $"{Name}:{Sum}";
        }

    }
    public partial class RGroup<T> : IResourceGroup<T> where T : IResource
    {
        public int Type
        {
            get { return (_members.Count > 0) ? _members.First<T>().Type : 0; }
        }
        public string Details { get; set; }
        public string Name { get; set; }
        protected List<T> _members { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T element in _members)
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
            get { return (_members.Count); }
        }
        public double Sum
        {
            get { return (_members.Count > 0) ? _members.Sum(x => x.Sum) : 0; }
        }
        public RGroup(string _name = "Sum", string _details = "Sum")
        {
            Name = _name;
            Details = _details;
            _members = new();
        }
        public RGroup(IEnumerable<T> _add, string _name = "Sum", string _details = "Sum") : this(_name, _details)
        {
            _members = (List<T>)_add;
        }
        public RGroup(T _add, string _name = "Sum", string _details = "Sum") : this(_name, _details)
        {
            _members = new();
            _members.Add(_add);
        }

        public void Add(T ra)
        {
            //throw new
            _members.Add(ra);
        }
        public void Remove(T ra)
        {
            //throw new
            _members.Remove(ra);
        }
        public IResource First()
        {
            return _members[0];
        }
        public void Set(double newValue)
        {
            { throw new InvalidOperationException("Set method is not valid for groups"); }
        }
        public void Clear()
        {
            _members.Clear();
        }

        public override string ToString()
        {
            return $"{Name}:{Sum}";
        }
        // public override string ToString()
        // {
        //     string returnString = "";
        //     foreach (IResource r in Adders)
        //     {
        //         returnString += r.ToString() + ", ";
        //     }
        //     return returnString;
        // }
    }
    public partial class RGroupRequests<T> : RGroup<IRequestable>, IResourceGroup<IRequestable>, IRequestable
    {
        // Same as in group but also sum requests.
        public RGroupRequests(IEnumerable<IRequestable> _add, string _name = "Sum", string _details = "Sum") : base(_add, _name, _details) { }
        public RGroupRequests(IRequestable _add, string _name = "Sum", string _details = "Sum") : base(_add, _name, _details) { }
        public RGroupRequests(string _name = "Sum", string _details = "Sum") : base(_name, _details) { }

        // Request is a second
        public double Request
        {
            get
            {
                double requestCum = 0;
                foreach (IRequestable i in _members)
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

        // Request is actual amount given
        public int State
        {
            get { return _members.Sum(x => x.State); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }

        public double Fraction()
        {
            return Sum / Request;
        }
        public double Weight
        {
            get
            {
                return _members.Sum(x => x.Request);
            }
        }
        public override string ToString()
        {
            return $"{Sum}/{Request}";
        }
    }
    public interface IResourceTransformers
    {
        public RList<IRequestable> Consumption { get; protected set; }
        public RList<IRequestable> Production { get; protected set; }
        public System.Object Driver { get; }
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

    public partial class RRequest : RStatic, IRequestable
    {
        // This is a dummy request. Does nothing.
        // Request is the amount of resource this consumer needs and points at another resource.
        public virtual double Request { get; set; }

        // Request is actual amount given
        public virtual int State { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_request"></param>
        /// <param name="_fulfilled">If true, base static will be set to request.</param>
        /// <param name="_name"></param>
        /// <param name="_details"></param>
        public RRequest(int _type, double _request, string _name = "Unknown", string _details = "Base value", bool _fulfilled = false) : base(_type, _fulfilled ? _request : 0, _name, _details: _details)
        {
            Request = Math.Round(_request, 2);
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
            return (base.Sum / Request);
        }
        public override string ToString()
        {
            return $"{Sum}/{Request}";
        }
    }
    // public partial class RRequestLinear : RRequest
    // {
    //     // Linear Request.  Modifies outputs to be same as fraction of inputs received.
    //     Resource.RStatic multiplier;
    //     Industry industry;

    //     public RRequestLinear(Industry _industry, Resource.RStatic _request) : base(_request)
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
    // public partial class RGroupRequests<TResource> : RGroup<TResource> where TResource : IRequestable
    // {
    //     public RGroupRequests(int _type, IEnumerable<TResource> _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
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
    static Dictionary<int, ResourceType> _index = new Dictionary<int, ResourceType>(){
            {0, new ResourceType("Unset", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {1, new ResourceType("Minerals", GD.Load<Texture2D>("res://assets/icons/resources/minerals.dds"), true)},
            {2, new ResourceType("Fuel", GD.Load<Texture2D>("res://assets/icons/resources/energy.dds"), true)},
            {3, new ResourceType("Food", GD.Load<Texture2D>("res://assets/icons/resources/food.dds"),  true)},
            {4, new ResourceType("H2O", GD.Load<Texture2D>("res://assets/icons/resources/h2o.png"),  true)},
            // {801, new ResourceType("Operational Capacity", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {802, new ResourceType("Capacity Utilisation", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {803, new ResourceType("Efficiency", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {901, new ResourceType("Freighter", GD.Load<Texture2D>("res://assets/icons/freighter.png"), false)}
        };
    public static ResourceType Index(int resourceCode)
    {
        return _index[resourceCode];
    }
    public static Texture2D Icon(int resourceCode)
    {
        return _index[resourceCode].icon;
    }

    ///<summary>
    ///Returns generic name of a resource for a specified code.
    ///</summary>
    public static string Name(int resourceCode)
    {
        return _index[resourceCode].name;
    }
    // public static double ShipWeight(int resourceCode)
    // {
    //     return _index[resourceCode].shipWeight;
    // }

    // Lists
    public partial class RList<TResource> : IEnumerable<TResource> where TResource : IResource
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
        public RList(IEnumerable<TResource> _members)
        {
            members = new SortedDictionary<int, TResource>();
            foreach (TResource resource in _members)
            {
                members.Add(resource.Type, resource);
            }
        }
        public RList(TResource resource)
        {
            members = new SortedDictionary<int, TResource>() { { resource.Type, resource } };
        }
        public RList()
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
                members[index] = default(TResource);
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
    public partial class RGroupList<TResource> : RList<RGroup<TResource>> where TResource : IResource
    {
        // Same as RList, except all elements are groups.
        public RGroupList() : base() { }
        public RGroupList(RGroup<TResource> rGroup) : base(rGroup) { }

        // Currently if is inited with only groups, they will become the members.
        public RGroupList(TResource resource) : base((new RGroup<TResource>(resource))) { }
        public RGroupList(IEnumerable<TResource> _members) : base()
        {
            foreach (TResource m in _members)
            {
                members.Add(m.Type, new RGroup<TResource>(m));
            }
        }

        // override default clear behavior.
        // keep top level groups.
        public override void Clear()
        {
            foreach (KeyValuePair<int, RGroup<TResource>> member in members)
            {
                (member.Value).Clear();
            }
        }
        public void Insert(TResource r)
        {
            if (!members.ContainsKey(r.Type))
            {
                members[r.Type] = new RGroup<TResource>("Sum", "Sum");
            }
            else
            {
                this[r.Type].Add(r);
            }
        }
    }


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
    public partial class RStaticList : RList<RStatic>
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
            public RGroupRequests<IRequestable> LocalGain; // How much is produced here.
            public RGroupRequests<IRequestable> LocalLoss;

            // These are only collections of 'real' elements.
            RGroupRequests<IRequestable> localNet;
            RGroupRequests<IRequestable> tradeNet;
            RGroupRequests<IRequestable> net;

            public IRequestable UplineLoss
            {
                get { return ledger.ResourcePool.Trade.UplineTraderoute == null ? null : ledger.ResourcePool.Trade.UplineTraderoute.ListTailLoss.ContainsKey(Type) ? ledger.ResourcePool.Trade.UplineTraderoute.ListTailLoss[Type] : null; }
            }
            public IRequestable UplineGain
            {
                get { return ledger.ResourcePool.Trade.UplineTraderoute == null ? null : ledger.ResourcePool.Trade.UplineTraderoute.ListTailGain.ContainsKey(Type) ? ledger.ResourcePool.Trade.UplineTraderoute.ListTailGain[Type] : null; }
            }
            public IEnumerable<IRequestable> DownlineGain
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
            public IEnumerable<IRequestable> DownlineLoss
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
            public RGroupRequests<IRequestable> TradeNet
            {
                get
                {
                    tradeNet.Clear();
                    foreach (IRequestable item in DownlineGain)
                    {
                        tradeNet.Add(item);
                    }
                    if (UplineLoss != null)
                    {
                        tradeNet.Add(UplineLoss);
                    }
                    foreach (IRequestable item in DownlineLoss)
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
            public RGroupRequests<IRequestable> LocalNet
            {
                get
                {
                    localNet.Clear();
                    foreach (IRequestable item in LocalLoss)
                    {
                        localNet.Add(item);
                    }
                    foreach (IRequestable item in LocalGain)
                    {
                        localNet.Add(item);
                    }
                    return localNet;
                }
            }
            public RGroupRequests<IRequestable> Net
            {
                get
                {
                    net.Clear();
                    net.Add(localNet);
                    net.Add(tradeNet);
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
                LocalGain = new RGroupRequests<IRequestable>("Local Production", "Sum Produced");
                LocalLoss = new RGroupRequests<IRequestable>("Local Consumption", "Sum Requested");

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

            public EntryAccrul(int _type) : base(_type)
            {
                Stored = new Resource.RStatic(_type, 0, "Stored", "Stored");
                Capacity = new Resource.RStatic(_type, 1000, "Capacity", "Capacity");
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