using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
public static partial class Resource
{
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

        // FixedSizedQueue<double> History { get; set; }

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
        // int lastEframe = -1;
        protected double sum;
        public virtual double Sum
        {
            get
            {
                return sum;
                // if (lastEframe != Global.Instance.eframeCount)
                // {
                //     lastEframe = Global.Instance.eframeCount;
                //     History.Enqueue(sum);
                //     return sum;
                // }
                // else
                // {
                //     return History.Peek();
                // }
            }
            set
            {
                if (double.IsNaN(value))
                {
                    throw new ArgumentException("Attempt to set NAN!!!");
                }
                sum = value;
            }
        }
        public virtual int Type { get; }
        public virtual string Details { get; set; } = "Base value";
        public virtual string Name { get; set; } = "Unknown";
        public virtual double Request { get; set; }
        public bool IsHidden { get; set; } = false;
        public string ValueFormat { get; set; } = "{0:F1}";
        public FixedSizedQueue<double> History { get; set; } = new();

        protected static Global global { get; set; }

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
        public RStatic()
        {
            global = Global.Instance;
        }

        public static double operator -(RStatic a, RStatic b)
        {
            return a.Sum - b.Sum;
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
            Sum = value;
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
        int type;

        protected int lastEframe = -1;
        public RGroup()
        {
            _adders = new();
            _muxxers = new();
        }

        public RGroup(int _type = 0, string _name = "Sum", string _details = "Sum") : this()
        {
            type = _type;
            Name = _name;
            Details = _details;
        }

        public RGroup(IEnumerable<T> _add, string _name = "Sum", string _details = "Sum") : this(0, _name, _details)
        {
            _adders = (List<T>)_add;
        }
        public RGroup(T _add, string _name = "Sum", string _details = "Sum") : this(0, _name, _details)
        {
            _adders = new();
            _adders.Add(_add);
        }
        public virtual int Type
        {
            // infer type if not set.
            get
            {
                if (type < 1) { if (_adders.Count > 0) { type = _adders.First().Type; } else if (_muxxers.Count > 0) { type = _muxxers.First().Type; } }
                return type;
            }
            set { type = value; }
        }
        public virtual string Details { get; set; } = "Sum";
        public virtual string Name { get; set; } = "Sum";

        public bool IsHidden { get; set; } = false;
        public string ValueFormat { get; set; } = "{0:G2}";

        public FixedSizedQueue<double> History = new();
        protected List<T> _adders { get; set; }
        protected List<T> _muxxers { get; set; }

        public virtual IEnumerator<IResource> GetEnumerator()
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
        public virtual int Count
        {
            get { return (_adders.Count + _muxxers.Count); }
        }
        public virtual double Sum
        {
            get
            {
                // if (lastEframe != Global.Instance.eframeCount)
                // {
                //     lastEframe = Global.Instance.eframeCount;
                //     History.Enqueue(_AdderSubtotal() * _MuxxerSubtotal());
                //     return History.Last(); ;
                // }
                // else
                // {
                //     return History.Last();
                // }
                return (_muxxers.Count + _adders.Count > 0) ? _AdderSubtotal() * _MuxxerSubtotal() : 0;
            }
            set { throw new InvalidOperationException("Set method is not valid for groups"); }
        }

        double _AdderSubtotal()
        {
            return (_adders.Count > 0) ? _adders.Sum(x => x.Sum) : 1;
        }
        double _MuxxerSubtotal()
        {
            double a = 1;
            foreach (var x in _muxxers) { a *= x.Sum; }
            return (_muxxers.Count > 0) ? a : 1;
        }
        // double _AdderSubtotalRequest()
        // {
        //     return (_adders.Count > 0) ? _adders.Sum(x => x.Request) : 0;
        // }
        // double _MuxxerSubtotalRequest()
        // {
        //     double agg = 1;
        //     foreach (T muxxer in _muxxers) { agg *= muxxer.Request; }
        //     return agg;
        // }
        public virtual void Add(T ra)
        {
            _adders.Add(ra);
        }
        public void Mux(T ra)
        {
            _muxxers.Add(ra);
        }
        public virtual void UnAdd(T ra)
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
        public virtual double Request
        {
            get { return (_adders.Count > 0) ? _adders.Sum(x => x.Request) : 0 * _MuxxerSubtotal(); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }

        public virtual void Respond()
        {
            Respond(Request);
        }
        public virtual void Respond(double value)

        {
            double fraction = value / Request;
            foreach (IResource i in _adders)
            {
                i.Respond(fraction * i.Request);
            }
        }

        // Request is actual amount given
        public virtual int State
        {
            get { return _adders.Sum(x => ((IResource)x).State); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }

        public virtual double Fraction()
        {
            return Sum / Request;
        }
    }

    /// <summary>
    ///  Returns min, or max of children.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class RLim<T> : RGroup<T>, IResourceGroup<IResource> where T : IResource
    {
        int type;
        public bool Max { get; set; } = false;
        List<T> _members { get; set; }
        public override int Count
        {
            get { return (_members.Count); }
        }
        public override int Type
        {
            // infer type if not set.
            get
            {
                if (type < 1) { if (_members.Count > 0) { type = _members.First().Type; } }
                return type;
            }
            set { type = value; }
        }
        public override int State
        {
            get
            {
                { return 0; }

            }
            set
            {
                { throw new InvalidOperationException("Set State method is not valid for RLim"); }
            }
        }
        public override double Request
        {
            get { return Max ? _members.Max(x => x.Request) : _members.Min(x => x.Request); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }
        public override double Sum
        {
            get { return Max ? _members.Max(x => x.Sum) : _members.Min(x => x.Sum); }
            set
            {
                { throw new InvalidOperationException("Set method is not valid for groups"); }
            }
        }
        public override IEnumerator<IResource> GetEnumerator()
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
        public override double Fraction()
        {
            { throw new InvalidOperationException("Fraction Method not Valid for RLim"); }
        }
        public override void Respond()
        {
            { throw new InvalidOperationException("Respond Method not Valid for RLim"); }
        }
        public override void Respond(double value)

        {
            Respond();
        }
        public override void Add(T ra)
        {
            _members.Add(ra);
        }
        public override void UnAdd(T ra)
        {
            _members.Remove(ra);
        }
        public RLim()
        {
            _members = new();
        }

        public RLim(int _type = 0, string _name = "Limit", string _details = "Limit") : this()
        {
            type = _type;
            Name = _name;
            Details = _details;
        }

        public RLim(IEnumerable<T> _add, string _name = "Limit", string _details = "Limit") : this(0, _name, _details)
        {
            _members = (List<T>)_add;
        }
        public RLim(T _add, string _name = "Limit", string _details = "Limit") : this(0, _name, _details)
        {
            _members = new();
            _members.Add(_add);
        }
    }


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
    public static Dictionary<int, ResourceType> _index = new Dictionary<int, ResourceType>(){
            {0, new ResourceType("Unset", GD.Load<Texture2D>("res://assets/icons/18x18/unity_grey.dds"), false)},
            {1, new ResourceType("Minerals", GD.Load<Texture2D>("res://assets/icons/18x18/minerals.dds"), true)},
            {2, new ResourceType("Fuel", GD.Load<Texture2D>("res://assets/icons/18x18/energy.dds"), true)},
            {3, new ResourceType("Food", GD.Load<Texture2D>("res://assets/icons/18x18/food.dds"),  true)},
            {4, new ResourceType("H2O", GD.Load<Texture2D>("res://assets/icons/18x18/h2o.png"),  true)},
            {801, new ResourceType("Fulfillment", GD.Load<Texture2D>("res://assets/icons/18x18/fulfillment.svg"), false)},
            {802, new ResourceType("Capability", GD.Load<Texture2D>("res://assets/icons/18x18/capability.svg"), false)},
            {811, new ResourceType("Freighter", GD.Load<Texture2D>("res://assets/icons/18x18/freighter.png"), false)},
            {812, new ResourceType("Payload", GD.Load<Texture2D>("res://assets/icons/18x18/payload.svg"), false)},

            // {801, new ResourceType("Operational Capacity", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {802, new ResourceType("Capacity Utilisation", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            // {803, new ResourceType("Efficiency", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), false)},
            {901, new ResourceType("Scale", GD.Load<Texture2D>("res://assets/icons/18x18/scale.svg"), false)},

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

    public static IEnumerable<KeyValuePair<int, ResourceType>> GetRegular()
    {
        return _index.Where(x => x.Key < 800);
    }

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
        public void Remove(int id)
        {
            members.Remove(id);
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

        public IEnumerable<int> Keys()
        {
            foreach (KeyValuePair<int, TResource> kvp in members)
            {
                yield return kvp.Key;
            }
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

        // Consider moving to Domain.

        /// <summary>
        /// Ledger is the complete list of all the resources at a specific location. 
        /// It is sparce on the resource axis, but dense on the type axis.
        /// </summary>
        public Domain Domain;
        /// Dummy method to make sure resource exists.

        public class Entry
        {
            /// <summary>
            /// Represents a resource, and a place. Can be either request or resource.
            /// </summary>
            public Ledger ledger;

            // Net is combo of Resource and Request.
            public RGroup<IResource> LocalLoss;
            public RGroup<IResource> LocalGain;
            public RGroup<IResource> LocalNet { get; protected set; }


            // public RGroup<IResource> Downline = new RGroup<IResource>("Imported", "Imported Downline");
            // public RGroup<IResource> DownlineLoss = new RGroup<IResource>("Exported", "Exported Downline");
            public double Upline;

            // These are only collections of 'real' elements.
            public RGroup<IResource> TradeNet;


            public int Type { get; set; }
            public Entry(int _type)
            {
                //Local = 
                Type = _type;
                LocalGain = new RGroup<IResource>(Type, "Local gain", "Sum Produced");
                LocalLoss = new RGroup<IResource>(Type, "Local loss", "Sum Produced");
                LocalNet = new RGroup<IResource>(Type, "Local Net", "Sum Produced");

                TradeNet = new RGroup<IResource>(Type, "Trade", "Net Trade");
            }

            public void Clear()
            {
                LocalLoss.Clear();
                LocalGain.Clear();
                LocalNet.Clear();
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
                return $"{Resource.Name(Type)}: {LocalGain}-{LocalLoss} {TradeNet}";
            }
        }

        public class EntryAccrul : Entry
        {
            // Reusing 'request' as capacity.
            public Resource.RStatic _Stored;
            public Resource.RStatic Stored { get { return _Stored; } protected set { _Stored = value; } }
            public double Delta;

            public EntryAccrul(int _type) : base(_type)
            {
                _Stored = new Resource.RStatic(_type, 0, 1000, "Stored", string.Format("{0} stored here.", Name(_type)));
            }

            // Attempt to withdraw amount, return actual.
            public double Withdraw(double amount)
            {
                // Max withdrawl is stored, 
                // Max deposit is available storage.
                Delta = (Mathf.Min((Mathf.Max(amount, -_Stored.Sum)), _Stored.Request - _Stored.Sum));
                _Stored.Add(Delta);

                return -Delta;
            }
            public override string ToString()
            {
                return $"{Resource.Name(Type)}: {LocalGain}-{LocalLoss} {TradeNet} ({_Stored})";
            }
        }

        Dictionary<int, Entry> _all = new();
        /// <summary>
        ///  Dict containting only acrulable;
        /// </summary>


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
        protected Entry InitType(int type)
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
    public class FixedSizedQueue<T> : Queue<T>
    {

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);

            while (Count > 10)
            {
                T outObj;
                base.TryDequeue(out outObj);
            }
        }
    }
}