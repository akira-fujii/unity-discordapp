using System.Runtime.InteropServices;
using UnityEngine;

namespace JaveScriptExecuter
{
	public static class JavascriptExecuter
	{
		[DllImport("__Internal")]
		private static extern void execute(string methodName, string parameter);

		public static void Execute(string methodName, string parameter = "{}")
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			execute(methodName, parameter);
#else
			Debug.Log($"call native method: {methodName}, parameter : {parameter}");
#endif
		}
	}
}