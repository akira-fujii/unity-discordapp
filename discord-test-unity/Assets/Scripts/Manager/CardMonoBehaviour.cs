using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork.Poi
{
	[RequireComponent(typeof(Rigidbody))]
	public class CardMonoBehaviour : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;

		private bool _isDragged;

		private Vector3 _offset;

		public string cardId;
		public string owner;

		// public bool IsOwner
		// {
		//     get
		//     {
		//         // Debug.Log($"Card.owner: {owner}, NetworkManager.Instance.GameRoom.SessionId: {NetworkManager.Instance.GameRoom.SessionId}");
		//
		//         return owner == NetworkManager.Instance.GameRoom.SessionId;
		//     }
		// }

		private void Start()
		{
			LocalEvent.OnChangeCard += OnChangeCard;
		}

		// Unity event
		private void OnMouseDown()
		{
			return;
			// if (!IsOwner)
			// {
			//     return;
			// }
			//
			_rigidbody.isKinematic = true;
			_isDragged = true;
			var objPos = Camera.main.WorldToScreenPoint(transform.position);
			var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objPos.z);
			_offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
		}

		private void OnMouseDrag()
		{
			return;
			// if (!IsOwner)
			// {
			//     return;
			// }

			var objPos = Camera.main.WorldToScreenPoint(transform.position);

			var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objPos.z);
			var p = Camera.main.ScreenToWorldPoint(mousePos) + _offset;
			transform.position = CardLayoutUtil.ToFreeGrabPos(p);
			// NetworkManager.Instance.GameRoom.SendCardPosition(cardId, transform.position);
		}

		private void OnMouseExit()
		{
			return;
			_isDragged = false;
			_rigidbody.isKinematic = false;
		}

		private void OnMouseUp()
		{
			return;
			_isDragged = false;
			_rigidbody.isKinematic = false;
		}

		private Vector3 _lastPosition;

		private void Update()
		{
			return;
			// if (!IsOwner)
			// {
			//     return;
			// }
			if (_isDragged) return;
			var diffSqr = (transform.position - _lastPosition).sqrMagnitude;
			Debug.Log($"diffSqr: {diffSqr}");
			// if (diffSqr > 0.01f)
			// {
			//     _lastPosition = transform.position;
			//
			//     _ = NetworkManager.Instance.GameRoom.SendCardPosition(cardId, transform.position);
			//     
			// }
		}

		private void OnChangeCard(PlayerMoveInfo3D message)
		{
			// Debug.Log($"CardMonoBehaviour.OnChangeCard: {message.id} {message.position.x}");
			// if (IsOwner)
			// {
			//     return;
			// }
			if (_isDragged) return;

			if (message.id != cardId) return;
			Debug.Log($"CardMonoBehaviour.OnChangeCard: {message.id} {message.position.x}");
			transform.position = message.position;
		}
	}
}