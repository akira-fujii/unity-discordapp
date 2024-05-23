using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYNetwork
{
	public class CommandView : MonoBehaviour
	{
		public TMP_Text Label;
		public Button Button;
		public Button.ButtonClickedEvent OnButtonClicked => Button.onClick;
		private Command _command;

		public void SetCommand(Command command)
		{
			_command = command;
			Label.text = command.Message;
			Label.color = command.TextColor;
			Button.image.color = command.ButtonColor;
			OnButtonClicked.AddListener(() => command.Action?.Invoke());
		}
	}
}