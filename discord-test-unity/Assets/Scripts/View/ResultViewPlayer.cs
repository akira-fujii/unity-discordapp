using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYNetwork.Poi
{
	public class ResultViewPlayer : MonoBehaviour
	{
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _hpMpText;
		[SerializeField] private TMP_Text _powerText;
		[SerializeField] private Image _typeImage;
		[SerializeField] private RectTransform _fatalityRoot;
		[SerializeField] private UiCardView _cardViewPrefab;
		private List<UiCardView> _cardViews = new();
		public TMP_Text NameText => _nameText;
		public TMP_Text HpMpText => _hpMpText;
		public TMP_Text PowerText => _powerText;
		public Image TypeImage => _typeImage;
		public RectTransform FatalityRoot => _fatalityRoot;

		public UiCardView GetCardViewAt(int index)
		{
			return _cardViews[index];
		}

		[ContextMenu("Hide")]
		public void Hide()
		{
			_nameText.gameObject.SetActive(false);
			_hpMpText.gameObject.SetActive(false);
			_powerText.gameObject.SetActive(false);
			_typeImage.gameObject.SetActive(false);
			_fatalityRoot.gameObject.SetActive(false);
			foreach (var cardView in _cardViews)
			{
				cardView.gameObject.SetActive(false);
			}
		}

		public void SetCardData(CardData[] cardData, bool isAttacker)
		{
			for (var i = 0; i < cardData.Length; i++)
			{
				if (i >= _cardViews.Count)
				{
					var cardView = Instantiate(_cardViewPrefab, _cardViewPrefab.transform.parent);
					_cardViews.Add(cardView);
				}

				if (isAttacker)
				{
					_cardViews[i].SetCardDataAtk(cardData[i]);
				}
				else
				{
					_cardViews[i].SetCardDataDef(cardData[i]);
				}
			}
		}
	}
}