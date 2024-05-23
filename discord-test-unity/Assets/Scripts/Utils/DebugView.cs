using System;
using System.Collections;
using System.Collections.Generic;
using BYNetwork.Poi;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BYNetwork
{
	public class DebugView : MonoBehaviour
	{
		public const string StateTextPrefix = "State: ";
		public const string ChannelNameTextPrefix = "Channel: ";
		[SerializeField] private TMP_Text _stateText;
		[SerializeField] private TMP_Text _channelNameText;

		public void SetStateText(object state)
		{
			_stateText.text = StateTextPrefix + state.ToString();
		}

		public void SetChannelNameText(string channelName)
		{
			_channelNameText.text = ChannelNameTextPrefix + channelName;
		}

		[SerializeField] private TMP_InputField _inputField;
		public TMP_InputField.SubmitEvent OnSubmit => _inputField.onSubmit;

		internal UnityEvent OnUpArrowAction = new();
		internal UnityEvent OnDownArrowAction = new();


		private void Start()
		{
			_inputField.onSubmit.AddListener((text) =>
			{
				_inputField.ActivateInputField();
				// EventSystem.current.SetSelectedGameObject(_inputField.gameObject, null);
				// _inputField.OnPointerClick(null);
			});
		}

		private void Update()
		{
			if (_inputField.isFocused)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					OnUpArrowAction?.Invoke();
				}

				if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					OnDownArrowAction?.Invoke();
				}
			}
		}

		public void SetInputField(string text)
		{
			_inputField.text = text;
		}
	}
}