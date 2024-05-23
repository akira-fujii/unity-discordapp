using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork
{
	public class CommandMenuView : MonoBehaviour
	{
		[SerializeField] private Transform _root;
		[SerializeField] private CommandView _commandViewPrefab;
		private List<CommandView> _commandViews;

		public void SetCommands(IEnumerable<Command> commands)
		{
			_commandViews = new List<CommandView>();
			foreach (var command in commands)
			{
				var view = Instantiate(_commandViewPrefab, _root);
				view.SetCommand(command);
				view.gameObject.SetActive(true);
				_commandViews.Add(view);
			}
		}
	}
}