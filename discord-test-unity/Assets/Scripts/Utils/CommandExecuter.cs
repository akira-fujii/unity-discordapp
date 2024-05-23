using System.Collections.Generic;
using System.Text.RegularExpressions;
using BYNetwork.Poi;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace BYNetwork
{
	public class CommandExecuter
	{
		private List<string> _commandHistory = new();

		private int _historyIndex = 0;

		public UnityEvent<string> OnLoadHistory = new();

		private NetworkManager _networkManager;

		public CommandExecuter(NetworkManager networkManager)
		{
			_networkManager = networkManager;
		}

		public async void OnSubmit(string command)
		{
			_commandHistory.Add(command);
			await ExecuteCommandAsync(command);
			_historyIndex = 0;
			OnLoadHistory?.Invoke("");
		}

		public void LoadHistoryPrev()
		{
			if (_commandHistory.Count == 0)
			{
				return;
			}

			var idx = Mathf.Max(0, _commandHistory.Count - _historyIndex - 1);
			idx = Mathf.Min(_commandHistory.Count - 1, idx);
			Debug.Log($"idx: {idx}");
			var history = _commandHistory[idx];
			OnLoadHistory?.Invoke(history);
			_historyIndex = Mathf.Min(_historyIndex + 1, _commandHistory.Count);
		}

		public void LoadHistoryNext()
		{
			if (_commandHistory.Count == 0)
			{
				return;
			}

			_historyIndex = Mathf.Max(_historyIndex - 1, 0);
			var idx = Mathf.Max(0, _commandHistory.Count - _historyIndex - 1);
			idx = Mathf.Min(_commandHistory.Count - 1, idx);
			var history = _commandHistory[idx];
			OnLoadHistory?.Invoke(history);
		}

		private async UniTask ExecuteCommandAsync(string command)
		{
			Debug.Log($"Command: {command}");
			if (TryParseInput(command, out var cmd, out var dict))
			{
				Debug.Log($"Command: {cmd}, Args: {dict.Count}");
				// var obj = FromDictToObject(dict);
				await _networkManager.GameRoom.Send(cmd, dict);
				return;
			}

			Debug.LogError("Command parse failed.");
		}

		public static bool TryParseInput(string input, out string command, out Dictionary<string, object> dict)
		{
			if (string.IsNullOrEmpty(input))
			{
				command = "";
				dict = new Dictionary<string, object>();
				return false;
			}

			command = "";
			dict = new Dictionary<string, object>();

			// コマンドを取得
			var commandMatch = Regex.Match(input, @"^(\w+)");

			if (commandMatch.Success)
			{
				command = commandMatch.Groups[1].Value;
			}

			// キーワード引数を抽出
			var matches = Regex.Matches(input, @"--(\w+)\s+(\S+)");
			foreach (Match match in matches)
			{
				var key = match.Groups[1].Value;
				var value = ParsePayload(match.Groups[2].Value);
				dict[key] = value;
			}

			return true;
		}

		private static object ParsePayload(string payload)
		{
			if (int.TryParse(payload, out var intValue))
			{
				return intValue;
			}
			else if (float.TryParse(payload, out var doubleValue))
			{
				return doubleValue;
			}
			else if (bool.TryParse(payload, out var boolValue))
			{
				return boolValue;
			}

			return payload;
		}
	}
}