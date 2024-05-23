using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public static string GameNameStatic = null;

	private static string HostStatic = null;

	public const string ROOMNAME = "card";

	public string GameName
	{
		get => string.IsNullOrEmpty(GameNameStatic) ? "testroom" : GameNameStatic;
		set => GameNameStatic = value;
	}

	public string Host
	{
		get => string.IsNullOrEmpty(HostStatic) ? "ws://localhost:2567" : HostStatic;
		set => HostStatic = value;
	}

	public string HostAddress
	{
		get
		{
			Debug.Log($"{Host}");
			return $"{Host}";
		}
	}

	public void Play()
	{
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
}