using System;
using UnityEngine;

namespace BYNetwork
{
	public struct Command
	{
		public string Message;
		public Action Action;
		public Color32 ButtonColor;
		public Color32 TextColor;
	}
}