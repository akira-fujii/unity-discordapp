using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BYNetwork.Poi
{
	public class UiCardView : MonoBehaviour
	{
		[SerializeField] private RawImage _thumbnail;
		[SerializeField] private TMP_Text _cardLabel;
		[SerializeField] private TMP_Text _cardDescription;

		private const string CardLabelFormat = "[{0}] {1}";
		private const string CardDescriptionFormat = "ATK:{0}  DEF:{1}";
		private const string CardDescriptionFormatAtk = "ATK:{0}";
		private const string CardDescriptionFormatDef = "DEF:{0}";

		private CardData _cardData;

		private static string GetCardLabel(int cost, string name)
		{
			return string.Format(CardLabelFormat, cost, name);
		}

		private static string GetCardDescription(int attack, int defense)
		{
			return string.Format(CardDescriptionFormat, attack, defense);
		}


		public void SetCardDataAtk(CardData cardData)
		{
			SetCardData(cardData, string.Format(CardDescriptionFormatAtk, cardData.Attack));
		}

		public void SetCardDataDef(CardData cardData)
		{
			SetCardData(cardData, string.Format(CardDescriptionFormatDef, cardData.Defense));
		}

		public void SetCardData(CardData cardData, string description)
		{
			Debug.Log($"SetCardData: {cardData.Title}");
			_cardData = cardData;
			_cardLabel.text = GetCardLabel(cardData.Cost, cardData.Title);
			_cardDescription.text = description;
			_thumbnail.texture = cardData.Texture;
			// TODO: set size
		}
	}
}