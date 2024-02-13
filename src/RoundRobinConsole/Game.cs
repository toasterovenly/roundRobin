using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using RoundRobinConsole.Extensions;

namespace RoundRobinConsole
{
	public class Game : IGame
	{
		[JsonIgnore]
		public static int TeamsPerGame = 2;
		public List<Team> Teams { get; set; } = new List<Team>();
		public Location Location;

		public Game(List<Team> teams, Location location)
		{
			Teams = teams;
			ValidateTeams();
			Location = location;
		}

		public Game(List<int> teamIndices, List<Team> allTeams, Location location)
		{
			TeamsFromIndices(teamIndices, allTeams);
			Location = location;
		}

		public Game(BitArray bitArrayTeams, List<Team> allTeams, Location location)
		{
			var teamIndices = bitArrayTeams.ToIndices();
			TeamsFromIndices(teamIndices, allTeams);
			Location = location;
		}

		public void TeamsFromIndices(List<int> teamIndices, List<Team> allTeams)
		{
			Teams.Clear();
			foreach (var index in teamIndices)
			{
				var team = allTeams[index];
				Teams.Add(team);
			}
			ValidateTeams();
		}

		public void ValidateTeams()
		{
			Debug.Assert(Teams != null);
			Debug.Assert(Teams.Count == TeamsPerGame);
		}

		public string GetGameString()
		{
			var sb = new StringBuilder();

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

			sb.Append($" at {Location.Name}");

			return sb.ToString();
		}

		public List<int> ToTeamIndices()
		{
			var indices = new List<int>();
			for (int i = 0; i < Teams.Count; ++i)
			{
				indices.Add((int)Teams[i].TeamNumber);
			}
			return indices;
		}

		/// <summary>
		/// Do the games have the same teams?
		/// <para></para>
		/// Not exactly an equals check becuase it ignores the team order.
		/// <br></br>
		/// E.g. team 1 playing team 2 is the same as team 2 playing team 1.
		/// </summary>
		/// <returns><see langword="true"/> if the two games contain the same teams. <see langword="false"/> otherwise.</returns>
		public bool HasSameTeams(Game other)
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
