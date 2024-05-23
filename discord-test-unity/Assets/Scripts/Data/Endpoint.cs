using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork.Poi
{
	public static class Endpoint
	{
		public const string AvatarUrl = "https://cdn.discordapp.com/avatars";

		public static string GetAvatarUrl(string userId, string avatarId)
		{
			return $"{AvatarUrl}/{userId}/{avatarId}.png";
		}

		public const string GameServer = "/colyseus";
		public const string GoogleDocs = "/googledocs";
	}
}