using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colyseus;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// game message type
namespace BYNetwork.Poi
{
	public class PlayerMoveInfo
	{
		public string id;

		public Vector2 position;
	}

	public class PlayerMoveInfo3D
	{
		public string id;

		public Vector3 position;
	}

	// common message type
	public static class LocalEvent
	{
		public static Action<PlayerMoveInfo> OnPlayerMove;
		public static Action<Card> OnAddCard;
		public static Action<PlayerMoveInfo3D> OnChangeCard;

		public static Action OnJoin;
		public static Action<int> OnLeave;
	}

	public static class MessageToSend
	{
		// message base sync 
		public static async Task SendReady(this ColyseusRoom<MyRoomState> room, bool value)
		{
			// await room.Send(room.State.type.ready, new Dictionary<string, string>() { { "isReady", value.ToString() } });
			await room.Send(room.State.ready, new { isReady = value });
		}

		public static async Task SendPlayerPosition(this ColyseusRoom<MyRoomState> room, string id, Vector2 position)
		{
			await room.Send(room.State.playerMove, new { id = id, position = position });
		}

		public static async Task SendCardPosition(this ColyseusRoom<MyRoomState> room, string id, Vector3 position)
		{
			await room.Send(room.State.cardPos, new { id = id, x = position.x, y = position.y, z = position.z });
		}
	}

	public static class MessageToReceive
	{
		// init 処理の集約
		public static void SubscribeRoomSyncEvent(this ColyseusRoom<MyRoomState> room)
		{
			room.SetOnJoin();
			room.SetOnLeave();
			room.ReceiveOnPlayerMove();
			room.ReceiveOnAddCard();
			room.ReceiveOnCard();
		}

		private static void SetOnJoin(this ColyseusRoom<MyRoomState> room)
		{
			room.OnJoin += () => LocalEvent.OnJoin?.Invoke();
		}

		private static void SetOnLeave(this ColyseusRoom<MyRoomState> room)
		{
			room.OnLeave += (message) => LocalEvent.OnLeave?.Invoke(message);
		}

		// state base sync

		private static void ReceiveOnPlayerMove(this ColyseusRoom<MyRoomState> room)
		{
			room.OnMessage<PlayerMoveInfo>(room.State.playerMove,
				(message) => LocalEvent.OnPlayerMove?.Invoke(message));
		}

		public static void ReceiveOnAddCard(this ColyseusRoom<MyRoomState> room)
		{
			room.State.cards.OnAdd((index, item) =>
			{
				Debug.Log($"Card Added! {item}");
				LocalEvent.OnAddCard?.Invoke(item);
				room.ReceiveOnChangeCard(item.cardId);
				item.OnChange(() =>
				{
					Debug.Log($"Card Changed! (ONADD){item.cardId}");
					// var card = room.State.cards.items.First(pair => pair.Value.cardId == item.).Value;
					// Debug.Log($"Card Changed! {card.cardId}");
					// Event.OnChangeCard?.Invoke(card);
				});
			});
		}

		public static void ReceiveOnChangeCard(this ColyseusRoom<MyRoomState> room, string cardId)
		{
			var card = room.State.cards.items.First(pair => pair.Value.cardId == cardId).Value;
			Debug.Log($"ReceiveOnChangeCard! {cardId}=={card.cardId}");
			card.OnChange(() =>
			{
				var card = room.State.cards.items.First(pair => pair.Value.cardId == cardId).Value;
				Debug.Log($"Card Changed! {card.cardId}");
				// Event.OnChangeCard?.Invoke(card);
			});
		}

		public static void ReceiveOnCard(this ColyseusRoom<MyRoomState> room)
		{
			room.OnMessage<PlayerMoveInfo3D>("onChangeCardPos", (message) => LocalEvent.OnChangeCard?.Invoke(message));
			// room.OnMessage<PlayerMoveInfo3D>(room.State.type.onChangeCardPos, (message) => Event.OnChangeCard?.Invoke(message));
		}
	}

	public class NetworkManager : MonoBehaviour
	{
		private ColyseusClient _client = null;
		private MenuManager _menuManager = null;
		private ColyseusRoom<MyRoomState> _room = null;
		public Action<ColyseusRoom<MyRoomState>> OnCreateRoom;

		private void Awake()
		{
			EditorApplication.playModeStateChanged += async state => OnExitAsync(state);
		}

		private async void OnExitAsync(PlayModeStateChange state)
		{
			// This method is run whenever the playmode state is changed.
			if (state == PlayModeStateChange.ExitingPlayMode)
			{
				Debug.Log("Exiting Play Mode!");
				await GameRoom.Leave();
			}
		}

		private async void OnDestroy()
		{
			await GameRoom.Leave();
		}

		public void Initialize()
		{
			if (_menuManager == null)
			{
				_menuManager = gameObject.AddComponent<MenuManager>();
			}

			_client = new ColyseusClient(_menuManager.HostAddress);
		}

		public async Task JoinOrCreateAsync(string roomId)
		{
			Dictionary<string, object> options = new() { { "id", roomId } };

			try
			{
				_room = await Client.JoinById<MyRoomState>(roomId, options);
			}
			catch (MatchMakeException e)
			{
				Debug.Log("Failed to join room. Creating a new room...");
				_room = await Client.Create<MyRoomState>("card", options);
				OnCreateRoom?.Invoke(_room);
			}
		}

		//
		// public async Task CreateGame()
		// {
		//     // Will create a new game room if there is no available game rooms in the server.
		//     _room = await Client.Create<MyRoomState>(MenuManager.ROOMNAME);
		//     Debug.Log($"Room Created! {_room.RoomId}");
		//     // _room = await Client.JoinOrCreate<MyRoomState>(MenuManager.ROOMNAME);
		// }
		//
		// public async Task<ColyseusRoomAvailable[]> GetAllRoomsAsync()
		// {
		//     return await Client.GetAvailableRooms(MenuManager.ROOMNAME);
		// }

		public ColyseusClient Client
		{
			get
			{
				// Initialize Colyseus client, if the client has not been initiated yet or input values from the Menu have been changed.
				if (_client == null) // || !_client.Endpoint.Uri.ToString().Contains(_menuManager.HostAddress))
				{
					Initialize();
				}

				return _client;
			}
		}

		public ColyseusRoom<MyRoomState> GameRoom
		{
			get
			{
				if (_room == null)
				{
					Debug.LogError("Room hasn't been initialized yet!");
				}

				return _room;
			}
		}
	}
}