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
    }

    /// <summary>
    ///  A collection of multiple resources of the same type. Enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceGroup<T> : IResource where T : IResource
    {
        public IEnumerator<T> GetEnumerator();
        public int Count { get; }
    }

    /// <summary>
    ///  A static resource is the most simple resource. It will be a number and stay that way until changed.
    /// </summary>
    public partial class RStatic : IResource
    {
        public virtual double Sum { get; protected set; }
        public int Type { get; }
        public string Details { get; set; }
        public string Name { get; set; }

        public RStatic(int _type, double _sum = 0, string _name = "Unknown", string _details = "Base value")
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
            Sum = newValue;
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
    public partial class RGroup<T> : IEnumerable<T>, IResourceGroup<T> where T : IResource
    {
        public int Type { get; }
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
            get { return _members.Sum(x => x.Sum); }
        }
        public RGroup(int _type, string _name = "Sum", string _details = "Sum")
        {
            Type = _type;
            Name = _name;
            Details = _details;
            _members = new();
        }
        public RGroup(int _type, IEnumerable<T> _add, string _name = "Sum", string _details = "Sum") : this(_type, _name, _details)
        {
            _members = (List<T>)_add;
        }
        public RGroup(int _type, T _add, string _name = "Sum", string _details = "Sum") : this(_type, _name, _details)
        {
            _members = new();
            _members.Add(_add);
        }

        public void Add(T ra)
        {
            //throw new
            _members.Add(ra);
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
    public partial class RGroupRequests<T> : RGroup<IRequestable>, IRequestable
    {
        // Same as in group but also sum requests.
        public RGroupRequests(int _type, IEnumerable<IRequestable> _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
        public RGroupRequests(int _type, IRequestable _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
        public RGroupRequests(int _type, string _name = "Sum", string _details = "Sum") : base(_type, _name, _details) { }

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
        public int State { get; set; } = -1;
    }
    public interface IResourceTransformers
    {
        public RList<IRequestable> Consumption { get; protected set; }
        public RList<IResource> Production { get; protected set; }
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

    // }
    // Type of resource.

    // public partial class RExchange : IRequestable
    // {
    //     /// <summary>
    //     /// Represents a request from one ledger to another.
    //     /// </summary>
    //     /// <param name="_from"></param>
    //     /// 

    //     // Parameters unique to responder.
    //     public string Details { get; set; }
    //     public string Name { get; set; }

    //     // Requester is 'real' request
    //     public RRequest Requester;


    //     // Look through to requester.

    //     public int Type { get { return Requester.Type; } }
    //     public double Sum { get { return -Requester.Sum; } }
    //     public int Count { get { return 1; } } //unused
    //     public int State { get { return Requester.State; } set { Requester.State = value; } }
    //     public Resource.RStatic Request { get; set; }
    //     public double RequestSum { get; }
    //     // No inputs if request fulfilled.
    //     public void Respond()
    //     {

    //     }
    //     // No fulfilled value returned if not fulfilled.
    //     public void Respond(double value) { }

    //     public RExchange(RRequest _from, string _name = "unset", string _details = "unset")
    //     {
    //         Requester = _from;
    //         Name = _name;
    //         Details = _details;
    //     }
    // }

    public partial class RRequest : RStatic, IRequestable
    {
        // This is a dummy request. Does nothing.
        // Request is the amount of resource this consumer needs and points at another resource.
        public double Request { get; set; }

        // Request is actual amount given
        public int State { get; set; } = -1;

        public RRequest(int _type, double _request, string _name = "Unknown", string _details = "Base value") : base(_type, 0, _name, _details: _details)
        {
            Request = _request;
        }
        // No inputs if request fulfilled.
        public virtual void Respond()
        {
            base.Set(Request);
            State = 0;
        }
        // No fulfilled value returned if not fulfilled.
        public virtual void Respond(double value)
        {
            base.Set(value);
            State = 1;
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

        protected virtual void _Set(int index, TResource value)
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
                return default(TResource);
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

    /// <summary>
    /// Child class of resource list with that constructs resources with correct descriptions.
    /// </summary>
    public class TradeRouteResourceList : RStaticList
    {
        string name;
        string description;
        public TradeRouteResourceList(string _name, string _description)
        {
            name = _name; description = _description;
        }
        protected override RStatic CreateNewMember(int index)
        {
            return new RStatic(index, 0, string.Format(name, Name(index)), string.Format(description, Name(index)));
        }
    }
    public partial class RGroupList<TResource> : RList<RGroup<TResource>> where TResource : IResource
    {
        // Same as RList, except all elements are groups.
        public RGroupList() : base() { }
        public RGroupList(RGroup<TResource> rGroup) : base(rGroup) { }

        // Currently if is inited with only groups, they will become the members.
        public RGroupList(TResource resource) : base((new RGroup<TResource>(resource.Type, resource))) { }
        public RGroupList(IEnumerable<TResource> _members) : base()
        {
            foreach (TResource m in _members)
            {
                members.Add(m.Type, new RGroup<TResource>(m.Type, m));
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
                members[r.Type] = new RGroup<TResource>(r.Type, "Sum", "Sum");
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
    /// <summary>
    /// Represents the other party in a traded resource.
    /// </summary>
    public class RTradedResource : IResource
    {
        public Ledger ledger;
        public IResource request;
        public int Type { get { return request.Type; } }
        public string Name { get; set; }
        public string Details { get; set; }
        public double Sum
        {
            get { return -request.Sum; }
        }
        public RTradedResource(IResource _request)
        {
            request = _request;
        }

    }
    public class RTradedRequest : IRequestable
    {
        public Ledger ledger;
        public IRequestable request;
        public int Type { get { return request.Type; } }
        public int State
        {
            get { return request.State; }
            set { request.State = value; }
        }

        public double Request
        {
            get { return -request.Request; }
            set { request.Request = -value; }
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public double Sum
        {
            get { return -request.Sum; }
        }
        public RTradedRequest(IRequestable _request)
        {
            request = _request;
        }

    }

    // public class ResourceRequestGroup : IResource
    // {
    //     List<ResourceRequest> _resourceRequests;
    // }
    public class Ledger : IEnumerable<KeyValuePair<int, Ledger.Entry>>
    {
        /// <summary>
        /// Ledger is the complete list of all the resources at a specific location. 
        /// </summary>

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


            // Net is combo of Resource and Request.

            public Resource.RGroup<Resource.IResource> ResourceLocal; // How much is produced here. 
            public Resource.RGroupRequests<Resource.IRequestable> RequestLocal;
            public Resource.RStatic ResourceParent; // Surplus to parent
            public Resource.IRequestable RequestParent; // How much I request of parent.

            // public Resource.RGroup<Resource.IResource> ExportSurplus;  // + paying requests to children;
            // ExportSurplus + Export to children.
            public Resource.RGroup<Resource.IResource> ResourceChildren; // Surplus from children
            public Resource.RGroupRequests<Resource.IRequestable> RequestChildren;  // How much my children request of me.
                                                                                    // Export demand contains a reference to an Import Demand in child.
                                                                                    // Export demand contains a reference to an Import Demand in child.

            public Resource.RGroup<Resource.IResource> NetRemote
            {
                get
                {
                    RGroup<IResource> net = new(Type, ResourceChildren.Concat(RequestChildren), "Trade Net", "Trade Net");
                    net.Add(RequestParent);
                    net.Add(ResourceParent);
                    return net;
                }
            }

            public Resource.RGroup<Resource.IResource> NetLocal
            {
                get
                {
                    return new RGroup<IResource>(Type, RequestLocal.Concat(ResourceLocal).ToList(), "Local Net", "Local Net");
                }
            }
            public Resource.RGroup<Resource.IResource> Net
            {
                get
                {
                    return new RGroup<IResource>(Type, NetRemote.Concat(NetLocal), "Total Net", "Total Net");
                }
            }


            public int Type { get; set; }
            public Entry(int _type)
            {
                //Local = 
                ResourceLocal = new Resource.RGroup<Resource.IResource>(_type, "Total", "Sum Produced");
                RequestLocal = new Resource.RGroupRequests<Resource.IRequestable>(_type, "Total", "Sum Requested");
                //ConsumptionRequest = new Resource.RGroupRequests<Resource.IRequestable>(_type, "Total", "Sum Requests");

                ResourceParent = new Resource.RStatic(_type, 0, "Total", "Surplus Being Exported to parent");
                RequestParent = new Resource.RRequest(_type, 0, "Total", "Demanding this from parent");

                ResourceChildren = new Resource.RGroup<Resource.IResource>(_type, "Total", "Imports");
                RequestChildren = new RGroupRequests<IRequestable>(_type);

                //Surplus = new Resource.RGroup<Resource.IResource>(_type, "Exports", "Total Exports");
                Type = _type;
            }

            public void Clear()
            {
                ResourceLocal.Clear();
                RequestLocal.Clear();
                ResourceParent.Set(0);
                RequestParent.Request = 0;
                RequestChildren.Clear();
                ResourceChildren.Clear();
                NetLocal.Clear();
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
                return $"{Resource.Name(Type)}: {ResourceLocal.Sum} {RequestLocal.Sum} {NetLocal.Sum} {RequestParent.Sum}";
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


        Dictionary<int, Entry> _all = new();
        /// <summary>
        ///  Dict containting only acrulable;
        /// </summary>
        Dictionary<int, Entry> _acrul = new();

        //Dictionary<int, EntryAccrul> _accrul = new();


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
                    Entry nre;
                    // If type less than 500, it is accrul.
                    if (type < 500)
                    {
                        nre = new EntryAccrul(type);
                    }
                    else
                    {
                        nre = new Entry(type);
                    }
                    _all[type] = nre;
                    return nre;
                }
            }
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
        // public void AddRequest(Resource.IRequestable r)
        // {
        //     this[r.Type].Consumption.Add(r);
        // }
        // public void AddResource(Resource.IResource r)
        // {
        //     this[r.Type].Production.Add(r);
        // }
        // public void AddSurplus(Resource.IResource r)
        // {
        //     this[r.Type].Surplus.Add(r);
        // }
        // public void AddDemand(Resource.IRequestable r)
        // {
        //     this[r.Type].Demand.Add(r);
        // }

        /// <summary>
        /// Return flat 'sum' of resource difference for use in next step.
        /// </summary>
        /// <returns></returns>
        // public IEnumerable<KeyValuePair<int, double[]>> GetBuffer()
        // {
        //     foreach (KeyValuePair<int, Entry> entry in _dictionary)
        //     {
        //         yield return new KeyValuePair<int, double[]>(
        //             entry.Key, new double[] { entry.Value.Production.Sum + entry.Value.Consumption.Sum + entry.Value.Surplus.Sum + entry.Value.Demand.Sum,
        //             entry.Value.Consumption.Sum, entry.Value.Surplus.Sum, entry.Value.Demand.Sum });
        //     }
        // }
    }
}

// TResource GetType(int code, bool createMissing = false)
// {
//     // if list is 
//     foreach (TResource r in members)
//     {
//         if (r.Type == code)
//         {
//             return r;
//         }
//     }
//     if (createMissing)
//     {
//         Resource.IResource nr;
//         if (Shallow)
//         {
//             nr = new Resource.RStatic(code, 0);
//         }
//         else
//         {
//             nr = new Resource.RGroup(code);
//         }
//         members.Add((TResource)nr);
//         return (TResource)nr;
//     }
//     else
//     {
//         return default(TResource);
//     }

// }

// Will add resource as a new (adder) element.
// public void Add (Resource.IResource _resource)
// {
//     Resource.IResource rot = GetType(_resource.Type);
//     // If no existing element, this is it.
//     if (rot == null)
//     {
//         GD.Print((this.members.GetType()));
//         GD.Print((_resource.GetType()));

//         members.Add((TResource)_resource);
//         return;
//     }
//     // If existing element is static. Create parent and add both.
//     else if (rot is Resource.RStatic)
//     {
//         // Not allowed
//         //members.Remove((TResource)rot);
//         //Add((TResource)(new Resource.RGroup(rot.Type, new List<IResource> { rot, _resource })));
//     }
//     // Otherwise just add to agg
//     else
//     {
//         // if (rot == null)
//         // {
//         //     rot = new Resource.RGroup(_resource.Type);
//         //     members.Add((TResource)rot);
//         // }
//         ((Resource.RGroup)rot).Add(_resource);
//     }


//     // // If no elements of this type, add as static.
//     // if (existing.Count < 1){
//     // 	members.Add(_resource);
//     // 	members.Remove(existing);
//     // }else if(existing is TResource.RGroup){
//     // // If no resourceGroup of this type exists, add to that.
//     // 	((Resource.RGroup)existing).Add(_resource);
//     // }else{
//     // // If exists but is static, replace with TResource.RGroup containing both.
//     // 	members.Add(new TResource.RGroup(existing .Type, new List<Resource>{existing, _resource}));
//     // 	members.Remove(existing);
//     // }
// }

// public void Multiply(TResource _resource)
// {
//     (()GetType<Resource.RGroup>(_resource.Type)).Multiply(_resource);

//     // // If no elements of this type, add as static.
//     // if (existing.Count < 1){
//     // 	members.Add(_resource);
//     // 	members.Remove(existing);
//     // }else if(existing is TResource.RGroup){
//     // // If no resourceGroup of this type exists, add to that.
//     // 	((Resource.RGroup)existing).Add(_resource);
//     // }else{
//     // // If exists but is static, replace with TResource.RGroup containing both.
//     // 	members.Add(new TResource.RGroup(existing .Type, new List<Resource>{existing, _resource}));
//     // 	members.Remove(existing);
//     // }
// }



// Get resources with code between range.
// public IEnumerable<TResource> GetRange(int min, int max)
// {
//     int i = min;
//     while (i <= max)
//     {
//         if (min <= r.Type && r.Type <= max)
//         {
//             yield return r;
//         }
//     }
// }

// void Coalesce(Resource.RStatic member, TResource nonmember)
// {
//     members.Remove(member);
//     Add
// }


// public void Clear()
// {
//     foreach  (Resource.IResource m in members)
//     {
//         ((Resource.RStatic)m).Set(0);
//     }
// }
// public void RemoveZeros()
// {
//     members.RemoveAll(m => m.Sum == 0);
// }


