#region

using UnityEngine;

#endregion

namespace BYNetwork.Poi
{
	public struct CardData
	{
		public byte Element;
		public byte Cost;
		public ushort Attack;
		public ushort Defense;
		public string Id;
		public string Title;
		public string Description;
		public string Image;
		public Texture2D Texture;
	}
}