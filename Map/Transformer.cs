using Godot;
using System;
using System.Collections.Generic;

public class Transformer : EcoNode
{ 
	// Small class to handle 'request, vs receive'
	public class Requester{

		public ResourceStatic request;
		// 0 fulfilled.
		// 1 partially fulfilled.
		// 2 unfulfilled.
		public int Type{get{return request.Type;}}
		public float Sum{get{return request.Sum;}}
		public ResourceStatic Response{get;set;}
		public Requester(ResourceStatic _request){
			request =_request;
		}
		public void Respond(ResourceStatic _response, int _status){
			Response = _response;
		}
	}

	[Export]
	public string slug;
	public Resource output;
	public List<Requester> costUpkeep;
	public List<Requester> costOperation;
	public List<Resource> costProduction;
	public List<Resource> storage;


	List<Situations.Base> situations;
	List<Requester> requesters;
	// 0-100 
	// Decays without maintainance.
	float breakDown;

	//How many 'buildings' this industry contains.
	int weight = 1;

	//How many 'buildings' are currently offline.
	int weightOffline = 1;
	
	public TransformerRegister.TransformerType ttype;
	public string TypeName {get{return ttype.Name;}}
	public string TypeSlug {get{return ttype.Slug;}}
	public string TypeClass {get{return ttype.Superclass;}}
	public string TypeSubclass {get{return ttype.Subclass;}}
	public string TypeImage {get{return ttype.Image;}}

	//public string TypeRequirements{get{ttype.Requiremnts}}

	public string[] Tags {get;set;}
	public string Description {get;set;}
	public int Prioroty {get;set;}
	
	public override void _Ready()
	{
		base._Ready();

		// If instantiated in editor
		if (ttype is null){
			ttype = GetNode<TransformerRegister>("/root/Global/TransformerRegister").GetFromSlug(slug);
		}

		Tags = ttype.Tags;
		Description = ttype.Description;
		Prioroty = ttype.defaultPrioroty;
		
		costUpkeep = new List<Requester>(GetFromTemplate(ttype.Upkeep));
		costOperation = new List<Requester>(GetFromTemplate(ttype.Operation));

		costProduction = new List<Resource>(GetStaticFromTemplate(ttype.Production));
		storage = new List<Resource>(GetStaticFromTemplate(ttype.Storage));
	}

	//How much this transformer is requesting from pool.
	public virtual IEnumerable<Requester> Requests(){
		foreach (Requester r in costUpkeep){
			yield return r;
		}foreach (Requester r in costOperation){
			yield return r;
		}
	}
	public virtual IEnumerable<Resource> Produced(){
		return costProduction;
	}

	IEnumerable<Requester> GetFromTemplate(Dictionary<int, float> template){
		if (template == null){yield break;}
		foreach (KeyValuePair<int, float> kvp in template){
			yield return new Requester(new ResourceStatic(kvp.Key, kvp.Value, Name));
		}
	}
	IEnumerable<ResourceStatic> GetStaticFromTemplate(Dictionary<int, float> template){
		if (template == null){yield break;}
		foreach (KeyValuePair<int, float> kvp in template){
			yield return new ResourceStatic(kvp.Key, kvp.Value, Name);
		}
	}
}
