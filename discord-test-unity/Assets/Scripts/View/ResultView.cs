using System;
using System.Collections.Generic;
using System.Threading;
using BYNetwork.Poi;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResultView : MonoBehaviour
{
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private RectTransform _arrowRoot;
	[SerializeField] private RectTransform _damageRoot;
	[SerializeField] private TMP_Text _damageText;
	[SerializeField] private ResultViewPlayer _attacker;
	[SerializeField] private ResultViewPlayer _target;
	private CardViewSettingAsset _cardViewSettingAsset;

	public void SetCardViewSettingAsset(CardViewSettingAsset cardViewSettingAsset)
	{
		_cardViewSettingAsset = cardViewSettingAsset;
	}

	public Action OnShowResultEnd;

	public void ShowResult(IEnumerable<Result> results)
	{
		ShowResultsAsync(results, default).Forget();
	}

	private async UniTask ShowResultsAsync(IEnumerable<Result> results, CancellationToken token)
	{
		_canvasGroup.alpha = 1;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.interactable = true;
		foreach (var result in results)
		{
			await ShowResultAsync(result, token);
			await UniTask.Delay(1500, cancellationToken: token);
		}

		OnShowResultEnd?.Invoke();
		_arrowRoot.gameObject.SetActive(false);

		_canvasGroup.alpha = 0;
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.interactable = false;
	}

	private async UniTask ShowResultAsync(Result result, CancellationToken token)
	{
		_attacker.Hide();
		_target.Hide();
		_damageRoot.gameObject.SetActive(false);

		// set cards
		_attacker.NameText.text = result.Attacker.name;
		_attacker.HpMpText.text = $"HP:{result.PlayCardResult.attackerHpBefore}\nMP:{0}";
		_attacker.PowerText.text = result.PlayCardResult.attackerPower.ToString();
		Debug.Log($"SetCardData: {result.AttackerCards.Length}");
		_attacker.SetCardData(result.AttackerCards, true);
		_attacker.TypeImage.color = _cardViewSettingAsset.GetElementByIndex(result.PlayCardResult.attackerElement).Color;

		// set target info
		_target.NameText.text = result.Target.name;
		_target.HpMpText.text = $"HP:{result.PlayCardResult.targetHpBefore}\nMP:{0}";
		_target.PowerText.text = result.PlayCardResult.targetPower.ToString();
		_target.SetCardData(result.TargetCards, false);
		_target.TypeImage.color = _cardViewSettingAsset.GetElementByIndex(result.PlayCardResult.targetElement).Color;

		// set damage info
		var damageRatio = result.PlayCardResult.damageRatio.ToString("F1");
		var damageRatioText = Math.Abs(result.PlayCardResult.damageRatio - 1) < 0.01 ? "" : $"(x{damageRatio})";
		_damageText.text = result.PlayCardResult.damage.ToString() + $"\nDAMAGE{damageRatioText}";

		// show name
		_attacker.NameText.gameObject.SetActive(true);
		_attacker.HpMpText.gameObject.SetActive(true);
		await UniTask.Delay(500, cancellationToken: token);

		// show attacker cards
		for (var i = 0; i < result.AttackerCards.Length; i++)
		{
			_attacker.GetCardViewAt(i).gameObject.SetActive(true);
			await UniTask.Delay(500, cancellationToken: token);
		}

		// show total power
		_attacker.PowerText.gameObject.SetActive(true);
		_attacker.TypeImage.gameObject.SetActive(true);
		await UniTask.Delay(1000, cancellationToken: token);

		// show target name
		_target.NameText.gameObject.SetActive(true);
		_target.HpMpText.gameObject.SetActive(true);
		await UniTask.Delay(500, cancellationToken: token);


		// show target cards
		for (var i = 0; i < result.TargetCards.Length; i++)
		{
			_target.GetCardViewAt(i).gameObject.SetActive(true);
			await UniTask.Delay(500, cancellationToken: token);
		}

		_target.PowerText.gameObject.SetActive(true);
		_target.TypeImage.gameObject.SetActive(true);
		await UniTask.Delay(1000, cancellationToken: token);

		// Damage 
		_damageRoot.gameObject.SetActive(true);
		// await UniTask.Delay(1000, cancellationToken: token);
		_target.HpMpText.text = $"HP:{result.PlayCardResult.targetHpAfter}\nMP:{0}";

		// if dead
		if (result.PlayCardResult.targetHpAfter <= 0)
		{
			await UniTask.Delay(500, cancellationToken: token);
			_target.FatalityRoot.gameObject.SetActive(true);
		}
	}
}