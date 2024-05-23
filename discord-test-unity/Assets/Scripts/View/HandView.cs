using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using Unity.VisualScripting;
using UnityEngine;

namespace BYNetwork.Poi
{
	[Serializable]
	public struct CardLayoutSetting
	{
		public int MaxNumberOfCards;
		public float CurveWidth;
		public float CurveHeight;
		public float CardZInterval;
		public float RotateRadius;
		public float ReorderTime;
	}

	public class HandView : MonoBehaviour
	{
		[SerializeField] private CardLayoutSetting _cardLayoutSetting;
		[SerializeField] private CardView _cardPrefab;
		[SerializeField] private Transform _cardParent;
		[SerializeField] private List<CardView> _cardViews = new();

		private CancellationTokenSource _cancellationTokenSource;
		private bool _isReordering;

		public CardData[] GetSelectedCards()
		{
			return _cardViews.Where(cardView => cardView.IsSelected).Select(cardView => cardView.CardData).ToArray();
		}

		private bool OnValidateCardSelection(SelectableView selectableView)
		{
			var selectedCount = _cardViews.Count(cardView => cardView.IsSelected);
			if (selectedCount == 0)
			{
				return true;
			}

			var selectedCard = _cardViews.FirstOrDefault(cardView => cardView.SelectableId == selectableView.Id);
			var currentElement = _cardViews.FirstOrDefault(cardView => cardView.IsSelected).CardData.Element;
			if (selectedCard != null && selectedCard.CardData.Element != currentElement)
			{
				return false;
			}

			return true;
		}

		public void Reset()
		{
			foreach (var cardView in _cardViews)
			{
				cardView.Reset();
			}
		}

		private void DisposeCancellationToken()
		{
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource.Dispose();
			}
		}

		private void InitCancellationToken()
		{
			DisposeCancellationToken();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		private void OnDestroy()
		{
			DisposeCancellationToken();
		}

		public void RefreshView()
		{
			foreach (var cardView in _cardViews)
			{
				Destroy(cardView.gameObject);
			}

			_cardViews.Clear();
		}

		public void AddCard(ref CardData cardData, CardViewSettingAsset cardViewSettingAsset)
		{
			var cardView = Instantiate(_cardPrefab, transform);
			cardView.gameObject.SetActive(true);
			cardView.SetCardData(ref cardData, cardViewSettingAsset);
			_cardViews.Add(cardView);
			// cardviewsをcardidでソート
			_cardViews = _cardViews.OrderBy(view => view.CardData.Id).ToList();

			InitCancellationToken();
			var token = _cancellationTokenSource.Token;
			ReOrderCardsTween(token).Forget();
			cardView.SelectableView.OnValidate = OnValidateCardSelection;
			cardView.SelectableView.OnIsSelectedChanged += isSelected =>
			{
				foreach (var view in _cardViews)
				{
					if (view == null)
					{
						return;
					}

					var isValidCard = OnValidateCardSelection(view.SelectableView);
					view.SetIsInteractive(isValidCard);
				}
			};
		}

		[ContextMenu("ReOrderCards")]
		public void ReOrderCards()
		{
			var childActiveCount = 0;
			for (var i = 0; i < transform.childCount; i++)
			{
				var child = transform.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					childActiveCount++;
				}
			}

			var cnt = 0;
			for (var i = 0; i < transform.childCount; i++)
			{
				if (!transform.GetChild(i).gameObject.activeSelf)
				{
					continue;
				}

				var t = GetCardTransform(cnt, childActiveCount, _cardLayoutSetting);
				transform.GetChild(i).localPosition = t.Item1;
				transform.GetChild(i).localRotation = t.Item2;
				cnt++;
			}
		}

		public async UniTask ReOrderCardsTween(CancellationToken token)
		{
			var cnt = 0;
			for (var i = 0; i < _cardViews.Count; i++)
			{
				var t = GetCardTransform(i, _cardViews.Count, _cardLayoutSetting);
				_cardViews[i].SetBasePosition(t.Item1, t.Item2);
			}
		}

		public static (Vector3, Quaternion) GetCardTransform(int i, int numberOfCards,
			CardLayoutSetting setting)
		{
			// 10枚の時に最大角PI
			var ratio = (float)numberOfCards / setting.MaxNumberOfCards;
			var maxAngle = ratio * Mathf.PI;
			var angle = (float)(i + 1) / numberOfCards * maxAngle + Mathf.PI / 2 * (1 - ratio); // Mathf.PIの周期に合わせて角度を計算
			var xPosition = Mathf.Cos(Mathf.PI - angle) * setting.CurveWidth; // カードのx座標
			var yPosition = Mathf.Sin(angle) * setting.CurveHeight;
			// var xPosition = i * (setting.CurveWidth / maxNumberOfCards); // カードのx座標
			// var yPosition = Mathf.Sin(xPosition / setting.CurveWidth * Mathf.PI) * setting.CurveHeight; // カードのy座標
			var zPosition = i * setting.CardZInterval;
			var spawnPosition = new Vector3(xPosition, zPosition, yPosition); // カードの配置位置

			// カードを法線の向きに傾ける
			var center = new Vector3(0, 0, -setting.RotateRadius);
			var normal = (spawnPosition - center).normalized;
			var rotation = Quaternion.FromToRotation(Vector3.forward, normal);

			return (spawnPosition, rotation);
			// transform.localPosition = spawnPosition;
			// transform.localRotation = rotation;
			// GameObject newCard = Instantiate(cardPrefab, spawnPosition, Quaternion.identity); // カードを生成
			// newCard.transform.SetParent(transform); // カードをこのオブジェクトの子にする
		}
	}
}