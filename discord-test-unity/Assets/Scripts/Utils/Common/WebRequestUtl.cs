#region

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace BYNetwork
{
	public static class WebRequestUtl
	{
		public static async UniTask<Texture2D> GetWebTextureAsync(string url)
		{
			var www = UnityWebRequestTexture.GetTexture(url);
			Debug.Log($"Loading Texture... {url}");
			await www.SendWebRequest();
			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
				return null;
			}
			else
			{
				Debug.Log($"Texture Loaded! {url}");
				var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				return texture;
			}
		}
	}
}