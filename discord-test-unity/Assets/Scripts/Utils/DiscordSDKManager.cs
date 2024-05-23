using System;
using System.Collections;
using System.Collections.Generic;
using BYNetwork.Poi;
using JaveScriptExecuter;
using UnityEngine;

public class DiscordSDKManager : MonoBehaviour
{
	[Flags]
	private enum InitFlags
	{
		None = 0,
		ServerUrl = 1 << 0,
		GameInfo = 1 << 1,
		UserInfo = 1 << 2
	}

	[SerializeField] private MenuManager _menuManager;
	public static string GameServerUrl;
	public static string DataUrl;
	public static UserInfo UserInfo;
	public static GameInfo GameInfo;
	public static DiscordSDKManager Instance;
	public static string GetChannelGameRoomName => $"{GameInfo.guildId}.{GameInfo.channelId}";

	/// <summary>
	/// Discord SDKの情報を取得できているか
	/// </summary>
	public static bool IsInitialized => _flags.HasFlag(InitFlags.ServerUrl | InitFlags.GameInfo | InitFlags.UserInfo);

	private static InitFlags _flags = 0;

	// Start is called before the first frame update
	private void Awake()
	{
		_flags = 0;
		Instance = null;
		Instance = this;
#if UNITY_WEBGL && !UNITY_EDITOR
		Debug.Log("in WebGL");
		GetGameServerUrl();
		GetUserInfo();
# else
		Debug.Log("Not in WebGL");
#endif
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void GetGameServerUrl()
	{
		var callbackParameter = new CallbackParameter
		{
			callbackGameObjectName = gameObject.name,
			callbackFunctionName = "GetServerUrlCallback"
		};
		var parameterJson = JsonUtility.ToJson(callbackParameter);
		JavascriptExecuter.Execute(JSFunction.GetServerUrl, parameterJson);
	}

	private void GetUserInfo()
	{
		JavascriptExecuter.Execute(JSFunction.GetUserInfo);
	}

	public void InitiateImageUpload()
	{
		JavascriptExecuter.Execute(JSFunction.InitiateImageUpload);
	}

	public void OpenInviteDialog()
	{
		JavascriptExecuter.Execute(JSFunction.OpenInviteDialog);
	}

	// Called From JS Client -----------------------
	/// <summary>
	/// 接続先ゲームサーバーの情報はクライアントjsから取得する
	/// UnityでアクティビティのIDを保持する必要がなくなる
	/// </summary>
	/// <param name="serverUrl"></param>
	public void GetServerUrlCallback(string serverUrl)
	{
		Debug.Log($"GetServerUrlCallback callback from js {serverUrl}");
		GameServerUrl = "wss://" + serverUrl + Endpoint.GameServer;
		DataUrl = "https://" + serverUrl + Endpoint.GoogleDocs;

		// set url
		_menuManager.Host = GameServerUrl;

		_flags |= InitFlags.ServerUrl;
		if (IsInitialized)
		{
			_menuManager.Play();
		}
	}

	public void GetGameInfoCallback(string gameinfo)
	{
		GameInfo = JsonUtility.FromJson<GameInfo>(gameinfo);
		Debug.Log($"GetGameInfoCallback callback from js {GameInfo.gameName} {GameInfo.channelName}");
		_menuManager.GameName = GameInfo.gameName;

		_flags |= InitFlags.GameInfo;
		if (IsInitialized)
		{
			_menuManager.Play();
		}
	}

	public void GetUserInfoCallback(string userInfo)
	{
		Debug.Log($"GetUserInfoCallback callback from js {userInfo}");
		UserInfo = JsonUtility.FromJson<UserInfo>(userInfo);
		var avatarUrl = $"https://cdn.discordapp.com/avatars/{UserInfo.id}/{UserInfo.avatar}.png";
		Debug.Log($"avatarUrl: {avatarUrl}");

		_flags |= InitFlags.UserInfo;
		if (IsInitialized)
		{
			_menuManager.Play();
		}
	}
}