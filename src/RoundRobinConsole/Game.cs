using System.Diagnostics;
using System.Text;

namespace RoundRobinConsole
{
	public class Game : IGame
	{
		public static int TeamsPerGame = 2;
		public List<Team> Teams = new List<Team>();
		public Location Location;

		public Game(List<Team> teams, Location location)
		{
			Debug.Assert(teams != null);
			Debug.Assert(teams.Count == TeamsPerGame);
			Teams = teams;
			Location = location;
		}

		public string GetGameString()
		{
			var sb = new StringBuilder();

			sb.Append($"{Location.Name}: ");

			var isFirstTeam = true;
			foreach (var team in Teams)
			{
				if (!isFirstTeam)
				{
					sb.Append(" vs ");
				}
				isFirstTeam = false;
				sb.Append(team.Name);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Do the games have the same teams?
		/// Not exactly an equals check becuase it ignores the team order.
		/// So team 1 playing team 2 is the same as team 2 playing team 1.
		/// </summary>
		/// <param name="other"></param>
		/// <returns><see langword="true"/> if the two games contain the same teams. <see langword="false"/> otherwise.</returns>
		public bool Matches(Game other)
		{
			if (Teams.Count != other.Teams.Count)
			{
				return false;
			}

			foreach (var team in Teams)
			{
				if (!other.Teams.Contains(team))
				{
					return false;
				}
			}

			return true;
		}
	}
}
