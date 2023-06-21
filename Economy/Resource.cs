using Godot;
using System;
using System.Collections.Generic;
public partial class Resource
{
    public Logger logger;


    public interface IResource
    // Interface for displaying basic resource icon.
    {
        int Type { get; }
        string Details { get; }
        string Name { get; }
        double Sum { get; }
    }

    public partial class RStatic : IResource
    {
        // A static resource is the most simple resource. It will be a number and stay that way until changed.
        public virtual double Sum { get; protected set; }
        public int Type { get; }
        public string Details { get; set; }
        public string Name { get; set; }
        public virtual int Count { get { return 0; } }

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
    public partial class RGroup<TResource> : RStatic where TResource : IResource
    {
        public List<TResource> Adders { get; protected set; }
        public List<TResource> Muxxers { get; protected set; }

        // Does not consider grandchild members.
        public override int Count
        {
            get { return (Adders.Count + Muxxers.Count); }
        }
        public override double Sum
        {
            get { return _Sum(); }
        }

        public RGroup(int _type, IEnumerable<TResource> _add, string _name = "Sum", string _details = "Sum") : base(_type)
        {
            Adders = (List<TResource>)_add;
            Muxxers = new List<TResource>();
            Name = _name;
            Details = _details;
        }
        public RGroup(int _type, TResource r, string _name = "Sum", string _details = "Sum") : this(_type, new List<TResource>() { r }, _name, _details) { }
        public RGroup(int _type, string _name = "Sum", string _details = "Sum") : this(_type, new List<TResource>(), _name, _details) { }

        public void Add(TResource ra)
        {
            //throw new
            Adders.Add(ra);
        }
        public void Multiply(TResource rm)
        {
            Muxxers.Add(rm);
        }

        public IResource First()
        {
            return Adders[0];
        }
        public override void Set(double newValue)
        {
            { throw new InvalidOperationException("Set method is not valid for groups"); }
        }
        public new void Clear()
        {
            Adders.Clear();
            Muxxers.Clear();
        }

        double _Sum()
        {
            double addCum = 0;
            double multiCum = 1;
            foreach (IResource i in Adders)
            {
                addCum += i.Sum;
            }
            foreach (IResource i in Muxxers)
            {
                multiCum += i.Sum;
            }
            return addCum * multiCum;
        }
        public override string ToString()
        {
            string returnString = "";
            foreach (IResource r in Adders)
            {
                returnString += r.ToString() + ", ";
            }
            return returnString;
        }
    }

    public partial class RGroupRequests : RGroup<IRequestable>
    {
        // Same as in group but also sum requests.

        public RGroupRequests(int _type, IEnumerable<IRequestable> _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
        public RGroupRequests(int _type, IRequestable _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
        public RGroupRequests(int _type, string _name = "Sum", string _details = "Sum") : base(_type, _name, _details) { }


        public double[] _RequestSum()
        {

            double addCum = 0;
            double requestCum = 0;
            double multiCum = 1;
            foreach (IRequestable i in Adders)
            {
                addCum += i.Sum;
                requestCum += i.Request.Sum;
            }
            foreach (IRequestable i in Muxxers)
            {
                multiCum += i.Sum;
            }
            return new double[] { addCum * multiCum, requestCum * multiCum };
        }
    }

    // public partial class RStaticInvert : IResource
    // {
    //     // wrapper class that returns negative value of leader.
    //     static RStatic leader;
    //     public RStaticInvert(RStatic _leader)
    //     {
    //         leader = _leader;
    //     }
    //     public double Sum { get { return -leader.Sum; } }
    //     public int Count { get { return leader.Count; } }
    //     public int Type { get { return leader.Type; } }
    //     public string Details { get { return leader.Details; } set }
    //     public string Name { get { return leader.Name; } }

    //     public new string ToString()
    //     {
    //         return $"{Name}:{Sum}";
    //     }
    // }
    // Interface allowing object to be member of resourceOrder
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

    public interface IRequestable : IResource
    {
        public int State { get; protected set; }
        //     // 0 fulfilled.
        //     // 1 partially fulfilled.
        //     // 2 unfulfilled.
        public Resource.RStatic Request { get; protected set; }
        public double SumRequest { get; }
        // No inputs if request fulfilled.
        public void Respond() { }
        // No fulfilled value returned if not fulfilled.
        public void Respond(double value) { }
    }
    public partial class RRequestBase : RStatic, IRequestable
    {
        // This is a dummy request. Does nothing.
        // Request is the amount of resource this consumer needs and points at another resource.
        public RStatic Request { get; set; }
        public double SumRequest { get { return Request.Sum; } }

        // Request is actual amount given
        public int State { get; set; } = -1;

        public RRequestBase(Resource.RStatic _request) : base(_request.Type, 0, _request.Name, _request.Name)
        {
            Request = _request;
        }
        // No inputs if request fulfilled.
        public virtual void Respond()
        {
            base.Set(Request.Sum);
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
            return $"{Sum}/{Request.Sum}";
        }
    }
    public partial class RRequestLinear : RRequestBase
    {
        // Linear Request.  Modifies outputs to be same as fraction of inputs received.
        Resource.RStatic multiplier;
        Industry industry;

        public RRequestLinear(Industry _industry, Resource.RStatic _request) : base(_request)
        {
            multiplier = new Resource.RStatic(Type, 0);
            ((Resource.RGroup<IResource>)_industry.Production[Type]).Multiply(multiplier);
            _industry.AddSituation(new Situations.OutputModifier(this, multiplier, _industry));
        }
        public override void Respond()
        {
            base.Respond();
            multiplier.Set(1f);
        }
        public override void Respond(double value)
        {
            base.Respond(value);
            multiplier.Set((value - Request.Sum) / Request.Sum);
        }
    }

    public partial class RGroupRequests<TResource> : RGroup<TResource> where TResource : IRequestable
    {
        public RGroupRequests(int _type, IEnumerable<TResource> _add, string _name = "Sum", string _details = "Sum") : base(_type, _add, _name, _details) { }
        public double SumRequest { get { return _SumRequests()[1]; } }
        double[] _SumRequests()
        // Do not net to call cum seperately.
        {
            double addCum = 0;
            double multiCum = 1;
            double requestCum = 0;
            foreach (TResource i in Adders)
            {
                addCum += i.Sum;
                addCum += i.Sum;
                requestCum += i.Request.Sum;
            }
            foreach (TResource i in Muxxers)
            {
                multiCum += i.Sum;
            }
            return new double[] { addCum * multiCum, requestCum * multiCum };
        }
    }
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
            this[r.Type].Add(r);
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
                members[index] = new RStatic(index, 0);
            }
            return members[index];
        }
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


