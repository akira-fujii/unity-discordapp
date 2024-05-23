#region

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace BYNetwork.Poi
{
	public class GameDataHandler : MonoBehaviour
	{
		public bool IsInitialized => _cardData != null;
		[SerializeField] private string _masterSheetUrl;

		public string MasterSheetUrl => _masterSheetUrl;

		private void SetMasterSheetUrl(string url)
		{
			_masterSheetUrl = url;
		}

		[SerializeField] private CardViewSettingAsset _cardViewSettingAsset;
		public CardViewSettingAsset CardViewSettingAsset => _cardViewSettingAsset;

		private CardData[] _cardData;
		public CardData[] CardData => _cardData;

		private async void Start()
		{
			// override url if Discord
#if UNITY_WEBGL && !UNITY_EDITOR
			SetMasterSheetUrl(DiscordSDKManager.DataUrl +
			                  "/spreadsheets/d/1rXTsMsG5KVWg_NF5c2N0bWLYSAmwrQ-6iXAlyyL4J0s/gviz/tq?tqx=out:csv&sheet=data");
#endif
			// load csv data
			var request = UnityWebRequest.Get(_masterSheetUrl);
			await request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"GameDataHandler Failed to load data: {request.error}");
				return;
			}

			var data = request.downloadHandler.text;
			Debug.Log($"GameDataHandler Loaded data: {data}");
			_cardData = ParseCardCsv(data);
			List<UniTask> loadTextureTasks = new();
			for (var i = 0; i < _cardData.Length; i++)
			{
				loadTextureTasks.Add(LoadCardTextureAsync(i, this.GetCancellationTokenOnDestroy()));
			}

			await UniTask.WhenAll(loadTextureTasks);
		}

		private async UniTask LoadCardTextureAsync(int index, CancellationToken token)
		{
			var cardData = _cardData[index];
			Texture2D texture;
			try
			{
				texture = await WebRequestUtl.GetWebTextureAsync(cardData.Image);
				if (texture == null)
				{
					Debug.LogError($"Failed to load image: {cardData.Image}");
					return;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to load image: {cardData.Image}");
				return;
			}

			_cardData[index].Texture = texture;
		}

		private static CardData[] ParseCardCsv(string data)
		{
			var cards = new List<CardData>();

			var rows = data.Split('\n');
			for (var i = 0; i < rows.Length; i++)
			{
				if (i == 0) // ヘッダー行をスキップ
				{
					continue;
				}

				var row = rows[i];
				var fields = row.Split(',');

				if (fields.Length < 3)
				{
					Console.WriteLine($"Error parsing line {i}: Insufficient fields");
					continue;
				}

				CardData card = new()
				{
					Id = fields[0].Trim('"'),
					Title = fields[1].Trim('"'),
					Description = fields[2].Trim('"'),
					Cost = byte.Parse(fields[3].Trim('"')),
					Attack = ushort.Parse(fields[4].Trim('"')),
					Defense = ushort.Parse(fields[5].Trim('"')),
					Element = byte.Parse(fields[6].Trim('"')),
					Image = fields[7].Trim('"')
				};

				cards.Add(card);
			}

			return cards.ToArray();
		}
	}
}