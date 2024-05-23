using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardViewSettingAsset", menuName = "poi/CardViewSettingAsset")]
public class CardViewSettingAsset : ScriptableObject
{
	[Serializable]
	public struct Element
	{
		public string Name;
		public byte Index;
		public Color32 Color;
	}

	public Element[] Elements;

	public Element GetElementByIndex(byte elementIndex)
	{
		return Elements.FirstOrDefault
		(element =>
			element.Index == elementIndex);
	}
}