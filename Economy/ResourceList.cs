using Godot;
using System;
using System.Collections.Generic;
public class ResourceList //: IEnumerable<Resource>
{ 
	private List<Resource> _members;
	// Resource list is a list of resources...
	public ResourceList(){
	
	}

	// public Resource GetType(int code)
	// {
	//     foreach (Resource r in _members)
	//     {
	//         if (r.Type == code)
	//         {
	//             return r;
	//         }
	//     }
	//     ResourceAgr newResource = new ResourceAgr(code, new List<Resource> {});
	//     _members.Add(newResource);
	//     return newResource;
	// }
	public ResourceStatic GetType(int code)
	{
		// if list is 
		foreach (ResourceStatic r in _members)
		{
			if (r.Type == code)
			{
				return r;
			}
		}
		ResourceStatic newResource = new ResourceStatic(code, 0);
		_members.Add(newResource);
		return newResource;
	}
	// Will add resource as a new element.
	private void Add(Resource _resource, List<ResourceAgr> list)
	{
		//Check if aggrigator already exists.
		GetType(_resource.Type, list).add.Add(_resource);
	}
	// Will modify value of resource.

	private void AddResource(Resource _resource, List<ResourceStatic> list)
	{
		//Check if aggrigator already exists.
		GetType(_resource.Type, list).Sum += _resource.Sum;
	}

	// Wrapper functions.
	// public IEnumerable<ResourceAgr> GetIncome(){
	// 	foreach (ResourceAgr r in members){
	// 		if (r.Sum > 0){
	// 			yield return r;
	// 		}
	// 	} 
	// }
	// public IEnumerable<ResourceAgr> GetExpenses(){
	// 	foreach (ResourceAgr r in members){
	// 		if (r.Sum < 0){
	// 			yield return r;
	// 		}
	// 	} 
	// }
	// public IEnumerable<Transformer> GetTradeRoutes(){
	// 	foreach (Transformer t in GetChildren()){
	// 		if (t is TransformerTrade){
	// 			yield return t;
	// 		} 
	// 	}
	// }
	// Get resources with code between range.
	public IEnumerable<ResourceAgr> GetRange(int min, int max)
	{
		foreach (ResourceAgr r in resourceDeltaProduced)
		{
			if (min <= r.Type && r.Type <= max)
			{
				yield return r;
			}
		}
	}

	public IEnumerable<ResourceAgr> GetStandard()
	{
		return GetRange(1, 100);
	}

}
