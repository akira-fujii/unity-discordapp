#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Colyseus;
using Cysharp.Threading.Tasks;
using UnityEngine;

#endregion

namespace BYNetwork.Poi
{
	public class CardGameManager : MonoBehaviour
	{
		[SerializeField] private NetworkManager _networkManager;
		[SerializeField] private GameDataHandler _gameDataHandler;
		[SerializeField] private CardMonoBehaviour _cardPrefab;
		[SerializeField] private GameView _gameView;
		[SerializeField] private HandView _handView;
		[SerializeField] private ResultView _resultView;
		[SerializeField] private FieldView _fieldView;
		private List<PlayerView> PlayerViews => _fieldView.PlayerViews;

		public Action<byte> OnGamePhaseChange;
		private readonly Dictionary<string, CardMonoBehaviour> _cardGameObjects = new();

		private int _playerIndex;
		private ColyseusRoom<MyRoomState> GameRoom => _networkManager.GameRoom;

		// Start is called before the first frame update
		public async void Start()
		{
			var roomName = DiscordSDKManager.IsInitialized ? DiscordSDKManager.GetChannelGameRoomName : "room_debug";
			_networkManager.OnCreateRoom += async room =>
			{
				Debug.Log($"Room Created! {_gameDataHandler.MasterSheetUrl}");
				// set room csv
				await room.Send("setCsv", new
				{
					csvUrl = _gameDataHandler.MasterSheetUrl
				});
			};
			// wait card initialize
			await UniTask.WaitUntil(() => _gameDataHandler.IsInitialized);
			await _networkManager.JoinOrCreateAsync(roomName);
			SetNetworkCallback();
			SetViewEvent();
		}

		// Update is called once per frame
		private void Update()
		{
		}

		private async UniTask SendPlayerNameAsync()
		{
			var userName = DiscordSDKManager.UserInfo.username;
			if (string.IsNullOrEmpty(userName))
			{
				userName = "empty";
			}

			Debug.Log($"Set Player Name: {userName}");
			await GameRoom.Send(GameRoom.State.setPlayerName, new { name = userName });
		}

		// view callback setting
		private void SetViewEvent()
		{
			_gameView.OnReadyButtonClicked.AddListener(async () =>
			{
				Debug.Log("Ready Button Clicked!");
				await GameRoom.Send(GameRoom.State.ready, new { isReady = true });
			});

			_gameView.OnFightButton += async () => await SendPlayCardAsync();
			_gameView.OnLeaveButton += async () =>
			{
				await GameRoom.Leave();
				Application.Quit();
			};

			_resultView.OnShowResultEnd += async () => await GameRoom.Send(GameRoom.State.solved);
			_resultView.SetCardViewSettingAsset(_gameDataHandler.CardViewSettingAsset);
		}

		private async UniTask SendPlayCardAsync(CancellationToken token = default)
		{
			var cards = _handView.GetSelectedCards();
			var cardIds = cards.Select(card => card.Id).ToArray();
			var targetId = PlayerViews.Where(view => view.IsSelected).Select(view => view.ClientId).FirstOrDefault();
			// validate
			if (cardIds.Length == 0 || string.IsNullOrEmpty(targetId))
			{
				Debug.Log("No card or target selected!");
				return;
			}

			_handView.Reset();
			foreach (var playerView in PlayerViews)
			{
				playerView.Reset();
			}

			await GameRoom.Send(GameRoom.State.playCard, new { cardIds, targetId });
		}

		// network callback setting
		private void SetNetworkCallback()
		{
			GameRoom.State.players.OnAdd((key, player) => { OnPlayerJoined(key, player); });
			GameRoom.State.players.OnRemove((key, player) => { onPlayerLeft(key); });
			GameRoom.OnLeave += (message) => Debug.Log($"You have left the Game!");

			GameRoom.State.players.OnRemove((key, player) => { Debug.Log($"Player {key} has left the Game!"); });

			// message
			GameRoom.OnMessage<PlayCardResult[]>("cardResult",
				message => OnCardResult(message));

			// property
			GameRoom.State.gamePhase.OnChange(() =>
			{
				OnGamePhaseChange?.Invoke(GameRoom.State.gamePhase.current);
				OnGamePhaseChangeCallback(GameRoom.State.gamePhase.current);
				// LocalEvent.OnGamePhaseChange?.Invoke(current);
			});

			// Event.OnAddCard += OnAddCard;
		}

		private List<CardData> GetCardDatasFromPlayer(Player player)
		{
			List<CardData> cards = new();
			foreach (var cardId in player.currentCardIds.items.Values)
			{
				Debug.Log($"GetCardDatasFromPlayer CardId: {cardId}");
				var cardData = _gameDataHandler.CardData.FirstOrDefault(c => c.Id == cardId);
				cards.Add(cardData);
			}

			return cards;
		}

		private List<CardData> GetCardDatasFromPIds(string[] cardIds)
		{
			List<CardData> cards = new();
			foreach (var cardId in cardIds)
			{
				Debug.Log($"GetCardDatasFromPlayer CardId: {cardId}");
				var cardData = _gameDataHandler.CardData.FirstOrDefault(c => c.Id == cardId);
				cards.Add(cardData);
			}

			return cards;
		}


		// message callback
		private void OnCardResult(PlayCardResult[] resultsMessage)
		{
			List<Result> results = new();
			foreach (var message in resultsMessage)
			{
				// Debug.Log($"attacker: {message.attackerJson}");
				var attacker = GameRoom.State.players[message.attackerId];
				var target = GameRoom.State.players[message.targetId];
				// var attacker = JsonUtility.FromJson<Player>(message.attackerJson);
				// var target = JsonUtility.FromJson<Player>(message.targetJson);
				// var attackerCards = GetCardDatasFromPlayer(attacker);
				// var targetCards = GetCardDatasFromPlayer(target);
				var attackerCards = GetCardDatasFromPIds(message.attackerCardIds);
				var targetCards = GetCardDatasFromPIds(message.targetCardIds);
				results.Add(new Result
				{
					PlayCardResult = message,
					Attacker = attacker,
					Target = target,
					AttackerCards = attackerCards.ToArray(),
					TargetCards = targetCards.ToArray()
				});
			}

			_resultView.ShowResult(results);

			foreach (var message in resultsMessage)
			{
				Debug.Log(
					$"Card Result! {message.attackerId} {message.targetId} {message.damage} {message.targetHpBefore} {message.targetHpAfter}");
			}
		}

		// public void OnCardDealt(PlayCardResultMessage message)
		// {
		// 	Debug.Log(
		// 		$"Card Result! {message.attackerId} {message.targetId} {message.damage} {message.targetHpBefore} {message.targetHpAfter}");
		// }

		// state callback
		private void OnPlayerJoined(string key, Player player)
		{
			var isLocal = GameRoom.SessionId == key;
			var playerDisplayName = player.name;

			// set event
			if (isLocal)
			{
				Debug.Log("You have joined the Game!");
				// id = 0;
				player.hand.OnAdd(OnAddCard);
				player.hand.OnRemove(OnRemoveCard);
				SendPlayerNameAsync().Forget();
			}

			// set event
			var index = PlayerViews.Count;
			var view = _fieldView.AddPlayer();
			player.OnNameChange((current, prev) =>
			{
				Debug.Log($"Player {key} has changed name from {prev} to {current}");
				view.SetPlayerData(key, current, isLocal);
			});

			player.OnIsReadyChange((current, prev) =>
			{
				Debug.Log($"Player {key} has changed ready from {prev} to {current}");
				view.SetIsReady(current);
			});

			// set view
			view.SetActive(true);
			view.SetPlayerData(key, playerDisplayName, isLocal);
			_playerIndex++;

			// other
			Debug.Log($"Player {key} has joined the Game!");
			LoadUserAvatarAsync(index).Forget();
		}

		private void onPlayerLeft(string key)
		{
			Debug.Log($"Player {key} has left the Game!");
			_fieldView.RemovePlayer(key);
		}

		private async UniTask LoadUserAvatarAsync(int id)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			var url = Endpoint.GetAvatarUrl(
				DiscordSDKManager.UserInfo.id,
				DiscordSDKManager.UserInfo.avatar);
#else
			var url = "https://cdn.discordapp.com/avatars/449085425278124039/1d60a50351e03323e37c110dfc0763e8.png";
#endif
			var texture = await WebRequestUtl.GetWebTextureAsync(url);
			Debug.Log($"Loading Avatar... {url}");
			if (texture)
			{
				PlayerViews[id].SetTexture(texture);
			}
		}

		private void OnGamePhaseChangeCallback(byte phase)
		{
			var log = $"Game Phase has changed to {phase}\nPlayers...";
			var players = GameRoom.State.players;
			foreach (var pKey in players.Keys)
			{
				log += $"\n{pKey}";
			}

			if (phase == GameRoom.State.gamePhase.start)
			{
				Debug.Log("Game Phase has changed to start!");
				_gameView.SetReadyButtonActive(true);
				// UpdateCardView();
			}

			if (phase == GameRoom.State.gamePhase.playing)
			{
				Debug.Log("Game Phase has changed to playing!");
				_gameView.SetReadyButtonActive(false);
				_gameView.SetTelopTurnStart(GameRoom.State.turn);
				// UpdateCardView();
			}

			if (phase == GameRoom.State.gamePhase.end)
			{
				Debug.Log("Game Phase has changed to end!");
				var winnerName = GameRoom.State.players[GameRoom.State.winnerId].name;
				_gameView.SetTelopGameSet(winnerName);
			}
		}

		private void SpawnCard(Card card)
		{
			var owner = card.owner;
			Debug.Log($"Card Added! {card.cardId} {owner}");
			var initialPos = new Vector3(0, 1.6f, 0f);
			var cardGameObject = Instantiate(_cardPrefab);
			cardGameObject.cardId = card.cardId;
			cardGameObject.owner = card.owner;
			cardGameObject.transform.position = initialPos;
			_cardGameObjects[card.cardId] = cardGameObject;
			if (card.owner != null)
			{
				if (card.owner == GameRoom.SessionId)
				{
					GameRoom.SendCardPosition(card.cardId, initialPos);
				}
			}
		}

		private void RemoveCard(string key)
		{
			Destroy(_cardGameObjects[key].gameObject);
			_cardGameObjects.Remove(key);
		}

		private void UpdateCardView()
		{
			var myHand = GameRoom.State.players[GameRoom.SessionId].hand;
			foreach (var cardId in myHand.items.Values)
			{
				Debug.Log($"OnCardUpdate in Hand: {cardId}");
			}
		}

		private void OnAddCard(int idx, string cardId)
		{
			Debug.Log($"Card Added! ({idx}){cardId}");
			UpdateCardView();
			var cardData = _gameDataHandler.CardData.FirstOrDefault(c => c.Id == cardId);
			_handView.AddCard(ref cardData, _gameDataHandler.CardViewSettingAsset);
			// SpawnCard(card);
		}

		private void OnRemoveCard(int idx, string cardId)
		{
			_handView.RefreshView();
			foreach (var card in GameRoom.State.players[GameRoom.SessionId].hand.items)
			{
				var cardData = _gameDataHandler.CardData.FirstOrDefault(c => c.Id == card.Value);
				_handView.AddCard(ref cardData, _gameDataHandler.CardViewSettingAsset);
			}
		}
	}
}