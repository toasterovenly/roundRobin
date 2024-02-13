using System.Text;

namespace RoundRobinConsole
{
	public class Round
	{
		public int RoundNumber;
		public List<Game> Games = new List<Game>();

		public Round(int roundNumber)
		{
			RoundNumber = roundNumber;
		}

		public string GetRoundString()
		{
			var sb = new StringBuilder();
			sb.Append($"Round {RoundNumber}:");
			foreach (var game in Games)
			{
				sb.Append($"\t");
				sb.Append(game.GetGameString());
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
