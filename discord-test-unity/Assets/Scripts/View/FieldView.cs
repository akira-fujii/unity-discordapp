using System;
using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork.Poi
{
	[Serializable]
	public struct PlayerLayoutSetting
	{
		public int PlayerPerRow;
		public int LoopCount;
		public float Radius;
		public float MaxPi;
		public float a;
		public float b;
		public float Offset;
		public Vector3 Center;
	}

	public class FieldView : MonoBehaviour
	{
		[SerializeField] private PlayerLayoutSetting _playerLayoutSetting;
		[SerializeField] private PlayerView _playerViewPrefab;
		private List<PlayerView> _playerViews = new();
		public List<PlayerView> PlayerViews => _playerViews;

		public PlayerView AddPlayer()
		{
			var playerView = Instantiate(_playerViewPrefab, transform);
			playerView.gameObject.SetActive(true);
			_playerViews.Add(playerView);
			SetPlayerPositions();
			return playerView;
		}

		public void RemovePlayer(string id)
		{
			var index = _playerViews.FindIndex(playerView => playerView.ClientId == id);
			Destroy(_playerViews[index].gameObject);
			_playerViews.RemoveAt(index);
		}

		[ContextMenu("SetPlayerPositions")]
		public void SetPlayerPositions()
		{
			var childActiveCount = 0;
			for (var i = 0; i < transform.childCount; i++)
			{
				var child = transform.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					childActiveCount++;
				}
			}

			var cnt = 0;
			for (var i = 0; i < transform.childCount; i++)
			{
				if (!transform.GetChild(i).gameObject.activeSelf)
				{
					continue;
				}

				var playerView = transform.GetChild(i).GetComponent<PlayerView>();
				var t = GetPlayerTransform(cnt, _playerLayoutSetting);
				var selectable = playerView.SelectableView.transform;
				transform.GetChild(i).localPosition = t.Item1;
				selectable.localRotation = t.Item2;
				cnt++;
			}
		}


		public (Vector3, Quaternion) GetPlayerTransform(int i,
			PlayerLayoutSetting setting)
		{
			// 円周上にプレイヤーを配置する
			var idx = i % setting.PlayerPerRow + i / setting.PlayerPerRow * 1f / setting.LoopCount;
			var maxPi = setting.MaxPi * Mathf.PI;
			var angle = maxPi / setting.PlayerPerRow * (idx + 0.4f) - (maxPi - Mathf.PI) / 2f - setting.Offset;
			var a = 1 / setting.a;
			var b = 1 / setting.b;
			var x = setting.Center.x + setting.a * setting.Radius * Mathf.Cos(angle);
			var z = setting.Center.z + setting.b * setting.Radius * Mathf.Sin(angle);
			var y = setting.Center.y;
			var position = new Vector3(x, y, z);

			// 法線ベクトル
			var fx = 2 * a * x;
			var fz = 2 * b * z;
			var normal = -new Vector3(fx, 0f, fz).normalized;
			var rotation = Quaternion.FromToRotation(Vector3.forward, normal);
			// // 接線の方向ベクトルを計算する
			// var tangentDirection = new Vector3(-b * x / (a * a * Mathf.Sqrt(1 - x * x / (a * a))),
			// 	a * y / (b * b * Mathf.Sqrt(1 - y * y / (b * b))),
			// 	0f).normalized;
			// //クターを接線に向かせる
			// var angleToRotate = Mathf.Atan2(tangentDirection.y, tangentDirection.x) * Mathf.Rad2Deg;
			// transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, angleToRotate);

			// var rotation = Quaternion.Euler(0f, angleToRotate, 0f);
			// var rotation = Quaternion.LookRotation(setting.Center - position);
			return (position, rotation);
		}
	}
}