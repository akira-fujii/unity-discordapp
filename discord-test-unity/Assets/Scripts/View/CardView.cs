using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace BYNetwork.Poi
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private SelectableView _selectableView;
		[SerializeField] private TMP_Text _title;
		[SerializeField] private TMP_Text _descriptionText;
		[SerializeField] private TMP_Text _powerText;
		[SerializeField] private TMP_Text _defenseText;
		[SerializeField] private TMP_Text _costText;
		[SerializeField] private SpriteRenderer[] _typeImages;
		[SerializeField] private SpriteRenderer _cardSpriteRenderer;
		private CardData _cardData;
		private Vector3 _offset;
		private Vector3 _basePosition;
		private Quaternion _baseRotation;
		private CancellationTokenSource _cancellationTokenSource;

		public CardData CardData => _cardData;
		public SelectableView SelectableView => _selectableView;
		public string SelectableId => _selectableView.Id;
		public bool IsSelected => _selectableView.IsSelected;

		public void Reset()
		{
			_selectableView.Reset();
		}

		public Action<CardData> OnClicked;
		public Action OnPointerOver;
		public Action OnPointerExit;

		private void Start()
		{
			_selectableView.OnDragStart += OnDragStart;
			_selectableView.OnDragging += OnDragging;
			_selectableView.OnDragEnd += OnDragEnd;
		}

		private void OnDestroy()
		{
			DisposeCancellationToken();
		}

		public void SetCardData(ref CardData cardData, CardViewSettingAsset cardViewSettingAsset)
		{
			_cardData = cardData;
			_title.text = cardData.Title;
			_descriptionText.text = cardData.Description;
			_costText.text = cardData.Cost.ToString();
			_powerText.text = cardData.Attack.ToString();
			_defenseText.text = cardData.Defense.ToString();
			var elementViewData =
				cardViewSettingAsset.GetElementByIndex(cardData.Element);
			foreach (var img in _typeImages)
			{
				img.color = elementViewData.Color;
			}

			var texture = cardData.Texture;
			if (texture == null)
			{
				return;
			}

			var ratio = 10f / texture.width;
			var newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
				new Vector2(.5f, .5f));
			_cardSpriteRenderer.sprite = newSprite;
		}

		// Unity event
		private void OnDragStart()
		{
			var objPos = Camera.main.WorldToScreenPoint(transform.position);
			var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objPos.z);
			_offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
			var pos = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
			var rot = Quaternion.identity;

			InitCancellationToken();
			var token = _cancellationTokenSource.Token;
			WaitSetPositionAsync(pos, rot, 0f, token).Forget();
			OnClicked?.Invoke(_cardData);
		}

		public bool IsInteractive { get; private set; }

		public void SetIsInteractive(bool isInteractive)
		{
			_cardSpriteRenderer.color = isInteractive ? Color.white : Color.gray;
			IsInteractive = isInteractive;
		}

		private void OnDragging()
		{
			var objPos = Camera.main.WorldToScreenPoint(transform.position);
			var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objPos.z);
			var p = Camera.main.ScreenToWorldPoint(mousePos) + _offset;
			transform.position = p;
		}

		private void OnDragEnd()
		{
			BackToBasePosition();
		}

		public void SetBasePosition(Vector3 position, Quaternion rotation)
		{
			_basePosition = position;
			_baseRotation = rotation;
			InitCancellationToken();
			var token = _cancellationTokenSource.Token;
			MoveCardAsync(position, rotation, 0.1f, token).Forget();
		}

		private void BackToBasePosition()
		{
			InitCancellationToken();
			var token = _cancellationTokenSource.Token;
			MoveCardAsync(_basePosition, _baseRotation, 0.1f, token).Forget();
		}

		public async UniTask WaitSetPositionAsync(Vector3 position, Quaternion rotation, float duration,
			CancellationToken token)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(duration));
			if (token.IsCancellationRequested)
			{
				return;
			}

			transform.localPosition = position;
			transform.localRotation = rotation;
		}

		public async UniTask MoveCardAsync(Vector3 position, Quaternion rotation, float duration,
			CancellationToken cancellationToken)
		{
			_basePosition = position;
			_baseRotation = rotation;
			var startPos = transform.localPosition;
			var positionHandle = LMotion
				.Create(startPos, position, duration)
				.WithEase(Ease.OutQuad)
				.BindToLocalPosition(transform);
			var rotationHandle = LMotion
				.Create(transform.localRotation, rotation, duration)
				.WithEase(Ease.OutQuad)
				.BindToLocalRotation(transform);
			await UniTask.WhenAll(
				positionHandle.ToUniTask(cancellationToken),
				rotationHandle.ToUniTask(cancellationToken)
			);
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
	}
}