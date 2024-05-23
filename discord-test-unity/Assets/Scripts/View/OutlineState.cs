using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork.Poi
{
	public enum OutlineState
	{
		None = 0,
		Selected = 1,
		Confirmed = 2
	}

	public static class Outline
	{
		public static void SetOutline(this Transform parent, OutlineState state)
		{
			foreach (Transform child in parent)
				child.gameObject.layer = LayerMask.NameToLayer(LayerMap[state]);
		}

		public static readonly Dictionary<OutlineState, string> LayerMap = new()
		{
			{ OutlineState.None, "Default" },
			{ OutlineState.Selected, "SelectedOutline" },
			{ OutlineState.Confirmed, "ConfirmedOutline" }
		};
	}
}