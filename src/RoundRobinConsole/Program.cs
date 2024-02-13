using System.Diagnostics;
using System.Text;

namespace RoundRobinConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Round Robin Output:");
			var bracket = new RoundRobin();
			bracket.Generate(2, 4, 2, 0);

			//var tree = new Tree(3);
			//Console.Write(tree.ToString());
		}
	}

	public class RoundRobin
	{
		public List<Team> Teams = new List<Team>();
		public List<Location> Locations = new List<Location>();
		public Stack<Round> Rounds = new Stack<Round>();

		private int _teamCount => Teams.Count;
		private int _locationCount => Locations.Count;
		private int _roundCount => Rounds.Count;
		private int _maxRoundCount = 0;
		public int GamesRequired = 0;

		/// <summary>
		/// Make a round robin tournament bracket.
		/// </summary>
		/// <param name="teamCount"></param>
		/// <param name="maxRoundCount">
		/// Set this to 0 to allow as many rounds as needed.
		/// If this is greater than 0, then it will produce sub-rounds where the same location needs to be used multiple times in one round.
		/// </param>
		/// <returns><see langword="true"/> if the bracket was generated successfully.</returns>
		public bool Generate(int teamsPerGame, int teamCount, int locationCount, int maxRoundCount = 0)
		{
			Game.TeamsPerGame = teamsPerGame;
			Teams.Clear();
			Locations.Clear();
			Rounds.Clear();
			_maxRoundCount = maxRoundCount;
			GamesRequired = 0;

			for (int i = 0; i < teamCount; ++i)
			{
				GamesRequired += i;
				Teams.Add(new Team(i.ToString()));
			}

			char letterA = 'A';
			for (byte b = 0; b < locationCount; ++b)
			{
				var locationLetter = (char)(letterA + b);
				Locations.Add(new Location(locationLetter.ToString()));
			}

			//GenerateRecursive();
			var games = EnumerateGames();
			var rounds = EnumerateRounds(games);

			return true;
		}

		public List<uint> EnumerateGames()
		{
			List<uint> games = new List<uint>();
			for (int i = 0; i < _teamCount; ++i)
			{
				for (int j = i + 1; j < _teamCount; ++j)
				{
					uint game = 1u << i | 1u << j;
					games.Add(game);
					Console.WriteLine($"Game {games.Count}: {game.ToString($"B{_teamCount}")}");
				}
			}
			return games;
		}

		public List<uint> EnumerateRounds(List<uint> games)
		{
			var possibleGameCount = games.Count;
			List<uint> rounds = new List<uint>();
			for (int i = 0; i < possibleGameCount; ++i)
			{
				for (int j = i + 1; j < possibleGameCount; ++j)
				{
					if ((games[i] & games[j]) != 0)
					{
						continue;
					}
					for (int k = j + 1; k < possibleGameCount; ++k)
					{
						if ((games[i] & games[k] & games[k]) != 0)
						{
							continue;
						}
						uint round = 1u << i | 1u << j | 1u << k;
						rounds.Add(round);
						Console.WriteLine($"Round {rounds.Count}: {round.ToString($"B{possibleGameCount}")}");
					}
				}
			}

			var resultRounds = new List<uint>();
			EnumerateRoundsRecursive(resultRounds, rounds, 0u, 0, 0, games.Count - 1);
			for (int i = 0; i < resultRounds.Count; ++i)
			{
				Console.WriteLine($"Rounds {i}: {resultRounds[i].ToString($"B{rounds.Count}")}");
			}

			return rounds;
		}

		public bool EnumerateRoundsRecursive(
			List<uint> resultRounds,
			List<uint> possibleRounds,
			uint currentRound,
			int roundIndex,
			int roundCount,
			int maxRoundCount
		)
		{
			if (roundCount >= maxRoundCount)
			{
				resultRounds.Add(currentRound);
				return true;
			}

			for (int i = roundIndex; i < possibleRounds.Count; ++i)
			{
				if ((currentRound & possibleRounds[i]) != 0)
				{
					continue;
				}

				var roundsBits = currentRound | possibleRounds[i];
				EnumerateRoundsRecursive(
					resultRounds,
					possibleRounds,
					roundsBits,
					i + 1,
					roundCount + 1,
					maxRoundCount
				);
			}

			return false;
		}

		//public List<uint> EnumerateRounds2(uint currentRounds, int depth, int maxDepth)
		//{
		//	var possibleRounds = EnumerateRounds();
		//	var possibleRoundCount = possibleRounds.Count;
		//	List<uint> rounds = new List<uint>();
		//	for (int i = 0; i < possibleRoundCount; ++i)
		//	{
		//		for (int j = i + 1; j < possibleRoundCount; ++j)
		//		{
		//			if ((possibleRounds[i] & possibleRounds[j]) != 0)
		//			{
		//				continue;
		//			}
		//			for (int k = j + 1; k < possibleRoundCount; ++k)
		//			{
		//				if ((possibleRounds[i] & possibleRounds[k]) != 0)
		//				{
		//					continue;
		//				}
		//				if ((possibleRounds[j] & possibleRounds[k]) != 0)
		//				{
		//					continue;
		//				}
		//				uint round = 1u << i | 1u << j | 1u << k;
		//				possibleRounds.Add(round);
		//				Console.WriteLine($"Round {possibleRounds.Count}: {round.ToString($"B{3}")}");
		//			}
		//		}
		//	}

		//	return possibleRounds;
		//}

		//public void EnumerateGames2(List<uint> games)
		//{
		//	var gamesCopy = new List<uint>(games);
		//	var game = 0u;
		//	EnumerateGamesRecursive2(ref game, gamesCopy, 3, 0);
		//}

		//public void EnumerateGamesRecursive2(ref uint game, List<uint> games, int maxDepth, int depth)
		//{
		//	if (depth >= maxDepth)
		//	{
		//		return;
		//	}
		//	//List<uint> games = new List<uint>();
		//	for (int i = depth; i < maxDepth; ++i)
		//	{
		//		uint game = 1u << i;
		//		games.Add(game);
		//	}
		//}

		//public bool GenerateRecursive()
		//{
		//	if (GenerateRecursive())
		//	{
		//		return true;
		//	}

		//	/*
		//	if number of games for each team is the required number and the total is the required number -> algorithm done, return true.
		//	have a list of teams for this round.
		//	have a list of all byes for all rounds.
		//	when the bye list contains all teams, start a new bye list.
		//	make the first teams on the list be the team that had a bye in the previous round
		//	shuffle the list except the previous byes at the front.
		//	make games in order
		//	    pull two teams out, put them in a game, pull the next two out, etc. until all locations have a game.
		//	verify that conditions are met
		//	put the new round in the round list.
		//	generate another round
		//	//*/

		//	return false;
		//}
	}

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

	public interface IGame
	{
		public string GetGameString();
	}

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

	public class Location
	{
		public string Name;

		public Location(string name)
		{
			Name = name;
		}
	}

	public class Team
	{
		public string Name;

		public Team(string name)
		{
			Name = name;
		}
	}

	//public class Player
	//{
	//	public string Name;

	//	public Player(string name)
	//	{
	//		Name = name;
	//	}
	//}
}
