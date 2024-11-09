using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Linq;
namespace Game;

/// <summary>
/// Regestry for templates.
/// </summary>
public partial class PlayerFeatureTemplates : Node, IEnumerable<PlayerFeatureTemplate>
{
	public List<Domain> Zones { get; set; }

	public IEnumerator<PlayerFeatureTemplate> GetEnumerator()
	{
		foreach (PlayerFeatureTemplate f in GetChildren().Cast<PlayerFeatureTemplate>())
		{
			yield return f;
		}
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Returns buildable templates for a domain.
	/// </summary>
	/// <param name="domain"></param>
	/// <returns></returns>
	public IEnumerable<PlayerFeatureTemplate> GetValid(Entities.IPosition domain)
	{
		foreach (PlayerFeatureTemplate template in this)
		{
			foreach (string tag in template.NeedsTags)
			{
				if (!domain.Tags.Contains(tag)) { continue; }
			}
			yield return template;
		}
	}
}
