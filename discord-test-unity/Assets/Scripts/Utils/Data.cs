using System;
using UnityEngine.Serialization;

[Serializable]
public struct UserInfo
{
	public string id;
	public string username;
	public string avatar;
}

[Serializable]
public struct GameInfo
{
	public string gameName;
	public string channelName;
	public string channelId;
	public string guildId;
}