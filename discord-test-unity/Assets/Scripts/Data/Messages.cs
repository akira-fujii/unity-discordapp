namespace BYNetwork.Poi
{
	public class PlayCardResult
	{
		public int index;
		public string attackerId;
		public string targetId;
		public int damage;
		public int attackerPower;
		public byte attackerElement;
		public int targetPower;
		public byte targetElement;
		public int attackerHpBefore;
		public int attackerHpAfter;
		public int targetHpBefore;
		public int targetHpAfter;
		public float damageRatio;
		public string[] attackerCardIds;
		public string[] targetCardIds;
	}

	public struct Result
	{
		public PlayCardResult PlayCardResult;
		public Player Attacker;
		public Player Target;
		public CardData[] AttackerCards;
		public CardData[] TargetCards;
	}
}