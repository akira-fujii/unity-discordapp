using BYNetwork.Poi;
using UnityEngine;

namespace BYNetwork
{
	public class DebugController : MonoBehaviour
	{
		[SerializeField] private NetworkManager _networkManager;
		[SerializeField] private CardGameManager _cardGameManager;
		[SerializeField] private DebugView _view;
		private CommandExecuter _commandExecuter;

		private void Start()
		{
			_commandExecuter = new CommandExecuter(_networkManager);
			_view.OnSubmit.AddListener(_commandExecuter.OnSubmit);
			_view.OnUpArrowAction.AddListener(_commandExecuter.LoadHistoryPrev);
			_view.OnDownArrowAction.AddListener(_commandExecuter.LoadHistoryNext);
			_commandExecuter.OnLoadHistory.AddListener(_view.SetInputField);

			// state debug
			LocalEvent.OnJoin += () => { _view.SetStateText("Join"); };
			LocalEvent.OnLeave += (m) => _view.SetStateText("Leave");

			_cardGameManager.OnGamePhaseChange += (c) =>
			{
				_view.SetStateText(c);
				_view.SetChannelNameText(DiscordSDKManager.GameInfo.channelName);
			};
		}
	}
}