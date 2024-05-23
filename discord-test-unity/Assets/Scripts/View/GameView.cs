using System;
using System.Threading;
using BYNetwork;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
	[SerializeField] private RectTransform _telopRoot;
	[SerializeField] private TMP_Text _telopText;
	[SerializeField] private CommandMenuView _systemMenu;
	[SerializeField] private CommandMenuView _gameMenu;
	[SerializeField] private Button _readyButton;

	private Command[] _menuCommands;
	private Command[] _gameMenuCommands;

	public Action OnLeaveButton;
	public Action OnFightButton;
	public Button.ButtonClickedEvent OnReadyButtonClicked => _readyButton.onClick;

	private void Start()
	{
		_menuCommands = new[]
		{
			new Command()
			{
				Message = "ImageUpload", Action = () => DiscordSDKManager.Instance.InitiateImageUpload(),
				ButtonColor = Color.white, TextColor = Color.black
			},
			new Command()
			{
				Message = "Invite", Action = () => DiscordSDKManager.Instance.OpenInviteDialog(),
				ButtonColor = Color.white, TextColor = Color.black
			},
			new Command()
			{
				Message = "Leave", Action = () => OnLeaveButton?.Invoke(),
				ButtonColor = Color.white, TextColor = Color.black
			}
		};

		_gameMenuCommands = new[]
		{
			new Command() { Message = "Heal 10", Action = () => { }, ButtonColor = Color.white, TextColor = Color.black },
			new Command() { Message = "Draw 1", Action = () => { }, ButtonColor = Color.white, TextColor = Color.black },
			new Command() { Message = "Surrender", Action = () => { }, ButtonColor = Color.white, TextColor = Color.black },
			new Command() { Message = "Win", Action = () => { }, ButtonColor = Color.white, TextColor = Color.black },
			new Command() { Message = "    ", Action = () => { }, ButtonColor = Color.white, TextColor = Color.black },
			new Command() { Message = "FIGHT", Action = () => { OnFightButton?.Invoke(); }, ButtonColor = Color.white, TextColor = Color.black }
		};
		_systemMenu.SetCommands(_menuCommands);
		_gameMenu.SetCommands(_gameMenuCommands);
	}

	public void SetReadyButtonActive(bool isActive)
	{
		_readyButton.gameObject.SetActive(isActive);
	}

	public void SetTelopTurnStart(int turn)
	{
		SetTelopTurnStartAsync(turn, default).Forget();
	}

	private async UniTask SetTelopTurnStartAsync(int turn, CancellationToken token)
	{
		_telopRoot.gameObject.SetActive(true);
		_telopText.text = $"TURN  {turn}  START!";
		await UniTask.Delay(1500, cancellationToken: token);
		_telopRoot.gameObject.SetActive(false);
	}

	public void SetTelopGameSet(string winner)
	{
		_telopRoot.gameObject.SetActive(true);
		_telopText.text = $"GAME SET\n <color=yellow>{winner}</color> WIN!!!";
	}
}