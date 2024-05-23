using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace BYNetwork.Poi
{
	public class SelectableView : MonoBehaviour
	{
		public Action<bool> OnIsSelectedChanged;
		public Action OnDragStart;
		public Action OnDragging;
		public Action OnDragEnd;

		public delegate bool ValidateSelectionDelegate(SelectableView view);

		public ValidateSelectionDelegate OnValidate = (view) => true;

		public enum SelectableState
		{
			None = 0,
			Reorder = 1,
			Grab = 2
		}

		private SelectableState _selectableState;
		private float _selectStartTime;
		public const float SelectTime = 0.2f;
		private Vector3 _dragStartPos;
		public bool IsSelected { get; private set; }
		[SerializeField] private int _group;
		[SerializeField] private bool _allowMultiSelect;
		private static List<SelectableView> _selectableViews;
		public string Id { get; private set; }

		private void Start()
		{
			if (_selectableViews == null)
			{
				_selectableViews = new List<SelectableView>();
			}

			_selectableViews.Add(this);
			Id = Guid.NewGuid().ToString();
		}

		public void Reset()
		{
			SetIsSelected(false);
		}

		private void OnDestroy()
		{
			_selectableViews.Remove(this);
		}

		public void SetOutline(OutlineState state)
		{
			transform.SetOutline(state);
		}

		private void SetIsSelected(bool isSelected)
		{
			var newValue = isSelected && OnValidate(this);
			IsSelected = newValue;
			SetOutline(IsSelected ? OutlineState.Confirmed : OutlineState.None);
			OnIsSelectedChanged?.Invoke(newValue);
		}

		// Unity event
		private void OnMouseDown()
		{
			_dragStartPos = Input.mousePosition;
			_selectableState = SelectableState.Grab;
			OnDragStart?.Invoke();
			_selectStartTime = Time.time;
		}

		private void OnMouseDrag()
		{
			if (_selectableState != SelectableState.Grab)
			{
				return;
			}

			// つかんで一定時間経過後、自動選択
			// if ((_dragStartPos - Input.mousePosition).sqrMagnitude > 0.3f &&
			//     Time.time - _selectStartTime > SelectTime)
			// {
			// 	IsSelected = true;
			// 	OnIsSelectedChanged?.Invoke(IsSelected);
			// 	SetOutline(IsSelected ? OutlineState.Confirmed : OutlineState.Selected);
			// }

			OnDragging?.Invoke();
		}

		private void OnMouseEnter()
		{
			Debug.Log("OnMouseEnter");
			SetOutline(IsSelected ? OutlineState.Confirmed : OutlineState.Selected);
		}

		private void OnMouseExit()
		{
			Debug.Log("OnMouseExit");
			SetOutline(IsSelected ? OutlineState.Confirmed : OutlineState.None);
		}

		private void OnMouseUp()
		{
			if (_selectableState == SelectableState.Grab)
			{
				_selectableState = SelectableState.None;
				OnDragEnd?.Invoke();
			}


			if ((_dragStartPos - Input.mousePosition).magnitude > 5f)
			{
				return;
			}

			// select
			if (_allowMultiSelect)
			{
				SetIsSelected(!IsSelected);
			}
			else
			{
				var currentSelected = _selectableViews.Where(view => view.IsSelected && view._group == _group);
				if (currentSelected.Count() > 0)
				{
					foreach (var view in currentSelected)
					{
						if (view._allowMultiSelect)
						{
							continue;
						}

						view.SetIsSelected(false);
					}
				}

				SetIsSelected(true);
			}
		}
	}
}