using Godot;
using System;
using System.Collections.Generic;
public class ResourceList : IEnumerable<Resource>
{
	// Resource list is a list of resources...
	private List<Resource> members;

	// If true, top level elements will be added as static
	public bool Shallow { get; set; }
	public Resource this[int index]  
	{  
		get { return members[index]; }  
		set { members.Insert(index, value); }  
	} 

	public IEnumerator<Resource> GetEnumerator()
	{
		return members.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
	public Resource Find(Predicate<Resource> match)
	{
		return members.Find(match);
	}
	public ResourceList(IEnumerable<Resource> _members, bool _shallow = false)
	{
		Shallow = _shallow;
		members = new List<Resource>() { };
		foreach (Resource r in _members)
		{
			Add(r);
		}
	}
	public ResourceList(bool _shallow = false)
	{
		Shallow = _shallow;
		{
			members = new List<Resource>() { };
		}
	}


	public Resource GetType(int code, bool createMissing = false)
	{
		// if list is 
		foreach (Resource r in members)
		{
			if (r.Type == code)
			{
				return r;
			}
		}
		if (createMissing)
		{
			Resource nr;
			if (Shallow)
			{
				nr = new ResourceStatic(code, 0);
			}
			else
			{
				nr = new ResourceAgr(code);
			}
			members.Add(nr);
			return nr;
		}
		else
		{
			return null;
		}

	}

	// Will add resource as a new (adder) element.
	public void Add(Resource _resource)
	{
		Resource rot = GetType(_resource.Type);
		// If no existing element and shallow. This is it.
		if (rot == null && Shallow)
		{
			members.Add(_resource);
			return;
		}
		// If existing element is static. Create parent and add both.
		else if (rot is ResourceStatic)
		{
			members.Remove(rot);
			members.Add(new ResourceAgr(rot.Type, new List<Resource> { rot, _resource }));
		}
		// Otherwise just add to agg
		else
		{
			if (rot == null)
			{
				rot = new ResourceAgr(_resource.Type);
				members.Add(rot);
			}
			((ResourceAgr)rot).Add(_resource);
		}


		// // If no elements of this type, add as static.
		// if (existing.Count < 1){
		// 	members.Add(_resource);
		// 	members.Remove(existing);
		// }else if(existing is ResourceAgr){
		// // If no resourceAgr of this type exists, add to that.
		// 	((ResourceAgr)existing).Add(_resource);
		// }else{
		// // If exists but is static, replace with ResourceAgr containing both.
		// 	members.Add(new ResourceAgr(existing.Type, new List<Resource>{existing, _resource}));
		// 	members.Remove(existing);
		// }
	}

	public void Multiply(Resource _resource)
	{
		((ResourceAgr)GetType(_resource.Type)).Multiply(_resource);

		// // If no elements of this type, add as static.
		// if (existing.Count < 1){
		// 	members.Add(_resource);
		// 	members.Remove(existing);
		// }else if(existing is ResourceAgr){
		// // If no resourceAgr of this type exists, add to that.
		// 	((ResourceAgr)existing).Add(_resource);
		// }else{
		// // If exists but is static, replace with ResourceAgr containing both.
		// 	members.Add(new ResourceAgr(existing.Type, new List<Resource>{existing, _resource}));
		// 	members.Remove(existing);
		// }
	}

	public void Remove(Resource r){
		members.Remove(r);
	}

	// Get resources with code between range.
	public IEnumerable<Resource> GetRange(int min, int max)
	{
		foreach (Resource r in members)
		{
			if (min <= r.Type && r.Type <= max)
			{
				yield return r;
			}
		}
	}

	// void Coalesce(ResourceStatic member, Resource nonmember)
	// {
	//     members.Remove(member);
	//     Add
	// }

	public IEnumerable<Resource> GetStandard()
	{
		return GetRange(1, 100);
	}
	public void Clear()
	{
		foreach (Resource m in members)
		{
			m.Sum = 0;
		}
	}
	public void RemoveZeros()
	{
		members.RemoveAll(m => m.Sum == 0);
	}
}
