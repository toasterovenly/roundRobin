using System.Text;

namespace RoundRobinConsole
{
	public class Bye : IGame
	{
		public List<Team> Teams = new List<Team>();

		public string GetGameString()
		{
			var sb = new StringBuilder();

			sb.Append("Bye: ");

			var isFirstTeam = true;
			foreach (var team in Teams)
			{
				if (!isFirstTeam)
				{
					sb.Append(", ");
				}
				isFirstTeam = false;
				sb.Append(team.Name);
			}

			return sb.ToString();
		}
	}
}
