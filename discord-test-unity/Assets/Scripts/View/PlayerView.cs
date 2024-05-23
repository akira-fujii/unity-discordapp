using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BYNetwork.Poi
{
	public class PlayerView : MonoBehaviour
	{
		[SerializeField] private SelectableView _selectableView;
		public SelectableView SelectableView => _selectableView;
		[SerializeField] private SpriteRenderer _readyStateRenderer;
		[SerializeField] private PlayerViewUGUI _playerViewUGUI;
		[SerializeField] private TMP_Text _label;
		[SerializeField] private Transform _labelParent;
		public Transform LabelParent => _labelParent;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Renderer _faceRenderer;
		[SerializeField] private Renderer _bodyRenderer;
		[SerializeField] private float _imageSize = 10f;
		public Quaternion Normal;
		private Material _faceMaterial;
		private static readonly Color32 ReadyColor = new(0, 255, 0, 100);
		private static readonly Color32 WaitColor = new(0, 0, 0, 80);

		public bool IsSelected => _selectableView.IsSelected;

		public void Reset()
		{
			_selectableView.Reset();
		}

		public string ClientId { get; private set; }

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
			_playerViewUGUI.gameObject.SetActive(active);
		}

		public void SetPlayerData(string id, string name, bool isLocalClient)
		{
			if (isLocalClient)
			{
				name = "[YOU]" + name;
			}

			_label.text = name;
			_playerViewUGUI.Label.text = name;
			ClientId = id;
		}

		public void SetIsReady(bool isReady)
		{
			_readyStateRenderer.color = isReady ? ReadyColor : WaitColor;
		}

		public void SetTexture(Texture2D texture)
		{
			var ratio = _imageSize / texture.width;
			var newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
				new Vector2(.5f, .5f));
			_spriteRenderer.sprite = newSprite;
			_spriteRenderer.size = Vector2.one * ratio;

			// _faceMaterial = _faceRenderer.material;
			// _faceMaterial.mainTexture = texture;

			_playerViewUGUI.AvatarRawImage.texture = texture;
		}

		private void OnDestroy()
		{
			Destroy(_faceMaterial);
		}
		//
		// private void Update()
		// {
		// 	var lookAt = Camera.main.transform.position - transform.position;
		// 	// look at camera
		// 	_labelParent.transform.rotation = Quaternion.identity;
		// 	// _label.transform.forward = -lookAt;
		// }

		public void SetOutline(bool isOn)
		{
			foreach (Transform child in transform)
				child.gameObject.layer =
					isOn ? LayerMask.NameToLayer("PlayerOutline") : LayerMask.NameToLayer("Default");
		}
	}
}