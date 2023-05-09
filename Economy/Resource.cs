using Godot;
using System;
using System.Collections.Generic;
public partial class Resource
{
    public Logger logger;
    public interface IResource
    {
        double Sum();
        int Type();
        string Details();
        string Name();
    }
    // Class for various resource related things.
    public abstract class RBase : IResource
    {
        protected int type;
        protected string details;
        public int Type()
        {
            return type;
        }
        public string Details()
        {
            return details;
        }
        public string Name()
        {
            return Resource.Index(type).name;
        }
        public abstract double Sum();
        public abstract int Count();

    }
    public partial class RGroup : RBase, IResource
    {
        public IEnumerable<IResource> GetAdd { get { return add; } }
        public IEnumerable<IResource> GetMulti { get { return multi; } }
        List<IResource> add;
        // currently no constructos accept multiplyers.
        List<IResource> multi = new List<IResource>();

        // Does not consider child members.
        public override int Count() { return (add.Count + multi.Count); }

        public RGroup(int _type, List<IResource> _add, string _details = "Sum")
        {
            type = _type;
            add = _add;
            details = _details;
        }
        public RGroup(int _type, IResource r, string _details = "Sum")
        {
            // add to empty group rather than replace.
            type = _type;
            add = new List<IResource>() { r };
            details = _details;
        }
        public RGroup(int _type, string _details = "Sum")
        {
            type = _type;
            details = _details;
            add = new List<IResource>();
        }

        public Resource.IResource First()
        {
            return add[0];
        }

        public void Clear()
        {
            add.Clear();
            multi.Clear();
        }

        public static explicit operator RStatic(RGroup ra)
        {
            //throw new
            if (ra.Count() != 1) { throw new InvalidCastException("Can only cast single member Resource.IResource.RGroup to Resource.IResource.RStatic"); }
            return (RStatic)ra.First();
        }
        public void Add(Resource.IResource ra)
        {
            //throw new
            add.Add(ra);
        }
        public void Multiply(Resource.IResource rm)
        {
            //throw new
            multi.Add(rm);
        }

        public override double Sum()
        {
            double addCum = 0;
            double multiCum = 1;
            foreach (Resource.IResource i in add)
            {
                addCum += i.Sum();
            }
            foreach (Resource.IResource i in multi)
            {
                multiCum += i.Sum();
            }
            return addCum * multiCum;
        }
        public override string ToString()
        {
            string returnString = "";
            foreach (IResource r in add)
            {
                returnString += r.ToString() + ", ";
            }
            return returnString;
        }
    }
    public partial class RStatic : RBase, IResource
    {
        double sum;
        public RStatic(int _type, double _sum, string _details = "Base value")
        {
            type = _type;
            sum = _sum;
            details = _details;
        }
        // cast static to agr.
        public static implicit operator RGroup(RStatic rs)
        {
            return new RGroup(rs.type, new List<IResource> { rs });
        }
        public void Set(double newValue)
        {
            sum = newValue;
        }
        public override double Sum()
        {
            return sum;
        }

        public override int Count() { return 1; }
        public void Clear() { return; }
        public Texture2D Icon { get { return Index(type).icon; } }
        public double ShipWeight { get { return Index(type).shipWeight; } }
        public bool Storable { get { return Index(type).storable; } }

        public override string ToString()
        {
            return $"{Name()}:{sum}";
        }
    }

    public partial class RStorage : RGroup, IResource
    {
        double stockpile; //Amount in this storage.
        public RStorage(int type) : base(type) { stockpile = 0; }
        public RStorage(int _type, List<IResource> _add, double initValue = 0) : base(_type, _add)
        {
            stockpile = initValue;
        }

        public double Deposit(double value)
        {
            // returns amount space free.
            // if negative, this is resource that didn't fit.
            double leftover = Free() - value;
            stockpile = Mathf.Min(Sum(), value + stockpile);
            return leftover;
        }
        public double Free()
        {
            return Sum() - stockpile;
        }
        public double Stock()
        {
            return stockpile;
        }
        public void Fill()
        {
            stockpile = Sum();
        }
        public override string ToString()
        {
            return $"{Name()}:{Stock()}/{Sum()}";
        }

    }
    // Type of resource.


    public struct ResourceType
    {
        public string name;
        public Texture2D icon;
        public double shipWeight;

        // If this resrource is something that can be store, or only instant;
        public bool storable;

        public ResourceType(string _name, Texture2D _icon, double _shipWeight, bool _storable)
        {
            name = _name;
            icon = _icon;
            shipWeight = _shipWeight;
            storable = _storable;
        }
    }

    static Dictionary<int, ResourceType> _index = new Dictionary<int, ResourceType>(){
            {0, new ResourceType("Unset", GD.Load<Texture2D>("res://assets/icons/resources/unity_grey.dds"), 1f, false)},
            {1, new ResourceType("Minerals", GD.Load<Texture2D>("res://assets/icons/resources/minerals.dds"), 1f, true)},
            {2, new ResourceType("Fuel", GD.Load<Texture2D>("res://assets/icons/resources/energy.dds"), 0.5f, true)},
            {3, new ResourceType("Food", GD.Load<Texture2D>("res://assets/icons/resources/food.dds"), 0.5f, true)},
            {4, new ResourceType("H2O", GD.Load<Texture2D>("res://assets/icons/resources/h2o.png"), 1f, true)},
            {901, new ResourceType("Freighter", GD.Load<Texture2D>("res://assets/icons/freighter.png"), -1f, false)}
        };
    public static ResourceType Index(int resourceCode)
    {
        return _index[resourceCode];
    }
    public static Texture2D Icon(int resourceCode)
    {
        return _index[resourceCode].icon;
    }
    public static double ShipWeight(int resourceCode)
    {
        return _index[resourceCode].shipWeight;
    }

    public partial class RList<TResource> : IEnumerable<TResource> where TResource : IResource
    {

        // if true, element will be created rather than returning null
        public bool CreateMissing = false;
        // TResource list is a list of resources...
        protected SortedDictionary<int, TResource> members;
        public RList(IEnumerable<TResource> _members)
        {
            members = new SortedDictionary<int, TResource>();
            foreach (TResource resource in _members)
            {
                members.Add(resource.Type(), resource);
            }
        }
        public RList(TResource resource)
        {
            members = new SortedDictionary<int, TResource>() { { resource.Type(), resource } };
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
            members.Add(r.Type(), r);
        }

        public IEnumerable<TResource> GetStandard()
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
        public void Clear()
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
    public partial class RGroupList<TResource> : RList<TResource> where TResource : RGroup
    {
        // Same as RList, except all elements are groups.
        public RGroupList() : base() { }
        public RGroupList(RGroup rGroup) : base((TResource)rGroup) { }

        // Currently if is inited with only groups, they will become the members.
        public RGroupList(TResource resource) : base(resource)
        {
            GD.Print("Questionable behaviour");
        }
        public RGroupList(RStatic resource) : base((TResource)(new RGroup(resource.Type(), resource))) { }
        public RGroupList(IEnumerable<RStatic> _members) : base()
        {
            foreach (RStatic m in _members)
            {
                members.Add(m.Type(), (TResource)new RGroup(m.Type(), m));
            }
        }
        protected override TResource _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                if (CreateMissing)
                {
                    members[index] = (TResource)new RGroup(index);
                }
                else
                {
                    return default(TResource);
                }

            }
            return members[index];
        }
    }
    public partial class RStorageList<TResource> : RList<TResource> where TResource : RStorage
    {
        protected override TResource _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                members[index] = (TResource)new RStorage(index);
            }
            return members[index];
        }
    }

    public partial class RStaticList<TResource> : RList<TResource> where TResource : RStatic
    {
        protected override TResource _Get(int index)
        {
            if (!members.ContainsKey(index))
            {
                members[index] = (TResource)new RStatic(index, 0);
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
//         if (r.Type() == code)
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
//     Resource.IResource rot = GetType(_resource.Type());
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
//         //Add((TResource)(new Resource.RGroup(rot.Type(), new List<IResource> { rot, _resource })));
//     }
//     // Otherwise just add to agg
//     else
//     {
//         // if (rot == null)
//         // {
//         //     rot = new Resource.RGroup(_resource.Type());
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
//     // 	members.Add(new TResource.RGroup(existing .Type(), new List<Resource>{existing, _resource}));
//     // 	members.Remove(existing);
//     // }
// }

// public void Multiply(TResource _resource)
// {
//     (()GetType<Resource.RGroup>(_resource.Type())).Multiply(_resource);

//     // // If no elements of this type, add as static.
//     // if (existing.Count < 1){
//     // 	members.Add(_resource);
//     // 	members.Remove(existing);
//     // }else if(existing is TResource.RGroup){
//     // // If no resourceGroup of this type exists, add to that.
//     // 	((Resource.RGroup)existing).Add(_resource);
//     // }else{
//     // // If exists but is static, replace with TResource.RGroup containing both.
//     // 	members.Add(new TResource.RGroup(existing .Type(), new List<Resource>{existing, _resource}));
//     // 	members.Remove(existing);
//     // }
// }



// Get resources with code between range.
// public IEnumerable<TResource> GetRange(int min, int max)
// {
//     int i = min;
//     while (i <= max)
//     {
//         if (min <= r.Type() && r.Type() <= max)
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
//     members.RemoveAll(m => m.Sum() == 0);
// }


