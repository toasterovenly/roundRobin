using System.Collections;
using RoundRobinConsole.Extensions;

namespace RoundRobinConsole
{
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
			Rounds.Clear();
			_maxRoundCount = maxRoundCount;
			GamesRequired = 0;

			CreateTeams(teamCount);
			CreateLocations(locationCount);

			var games = EnumerateGames();
			var rounds = EnumerateRounds(games);

			return true;
		}

		private void CreateTeams(int teamCount)
		{
			Teams.Clear();

			for (int i = 0; i < teamCount; ++i)
			{
				GamesRequired += i;
				var team = new Team();
				Teams.Add(team);
				Console.WriteLine($"Team {Teams.Count}: {team.Name}");
			}
		}

		private void CreateLocations(int locationCount)
		{
			Locations.Clear();

			char letterA = 'A';
			for (int i = 0; i < locationCount; ++i)
			{
				var locationLetter = (char)(letterA + i);
				var location = new Location(locationLetter.ToString());
				Locations.Add(location);
				Console.WriteLine($"Location {Locations.Count}: {location.Name}");
			}
		}

		public List<BitArray> EnumerateGames()
		{
			var gameBitArrays = new List<BitArray>();
			for (int i = 0; i < _teamCount; ++i)
			{
				for (int j = i + 1; j < _teamCount; ++j)
				{
					// for logging
					var teamIndices = new List<int>() { i, j };
					var game = new Game(teamIndices, Teams, Locations[0]);
					Console.WriteLine($"Game {gameBitArrays.Count}: {game.GetGameString()}");
					// for logging

					var gameBitArray = new BitArray(_teamCount);
					gameBitArray.Set(i, true);
					gameBitArray.Set(j, true);
					gameBitArrays.Add(gameBitArray);

					//Console.WriteLine($"Game {gameBitArrays.Count}: {gameBitArray.ToStringTeams(Teams)}");
					//Console.WriteLine($"Game {gameBitArrays.Count}: {gameBitArray.ToStringBools()}");
				}
			}
			return gameBitArrays;
		}

		public List<BitArray> EnumerateRounds(List<BitArray> gameBitArrays)
		{
			var possibleGameCount = gameBitArrays.Count;
			var roundBitArrays = new List<BitArray>();
			for (int i = 0; i < possibleGameCount; ++i)
			{
				for (int j = i + 1; j < possibleGameCount; ++j)
				{
					if (gameBitArrays[i].And(gameBitArrays[j]).HasAnySet())
					{
						continue;
					}
					for (int k = j + 1; k < possibleGameCount; ++k)
					{
						if (
							gameBitArrays[i].And(gameBitArrays[k]).HasAnySet()
							|| gameBitArrays[j].And(gameBitArrays[k]).HasAnySet()
						)
						{
							continue;
						}

						var roundBitArray = new BitArray(possibleGameCount);
						roundBitArray.Set(i, true);
						roundBitArray.Set(j, true);
						roundBitArray.Set(k, true);
						roundBitArrays.Add(roundBitArray);
						Console.WriteLine($"Round {roundBitArrays.Count}: {roundBitArray.ToIndices().ToStringNice()}");
					}
				}
			}

			var resultRounds = new List<BitArray>();
			var currentRound = new BitArray(possibleGameCount);
			EnumerateRoundsRecursive(resultRounds, roundBitArrays, currentRound, 0, 0, gameBitArrays.Count - 1);
			for (int i = 0; i < resultRounds.Count; ++i)
			{
				Console.WriteLine($"Rounds {i}: {resultRounds[i].ToIndices().ToStringNice()}");
			}

			return roundBitArrays;
		}

		public bool EnumerateRoundsRecursive(
			List<BitArray> resultRounds,
			List<BitArray> possibleRounds,
			BitArray currentRound,
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
				if (currentRound.And(possibleRounds[i]).HasAnySet())
				{
					continue;
				}

				var roundsBits = currentRound.Or(possibleRounds[i]);
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
}
