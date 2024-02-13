using System.Collections;
using System.Text;

namespace RoundRobinConsole.Extensions
{
	public static class BitArrayExtensions
	{
		public static string ToStringBools(this BitArray bitArray)
		{
			var sb = new StringBuilder();
			var entriesPerRowMax = 8;
			int entriesPerRow = 0;

			foreach (var bitValue in bitArray)
			{
				if (entriesPerRow > entriesPerRowMax)
				{
					entriesPerRow = 0;
					sb.AppendLine();
				}

				if (entriesPerRow > 0)
				{
					sb.Append($", ");
				}

				sb.Append($"{bitValue, 5}");

				++entriesPerRow;
			}

			return sb.ToString();
		}

		public static string ToStringTeams(this BitArray bitArray, List<Team> teams)
		{
			var sb = new StringBuilder();
			var entriesPerRowMax = 8;
			int entriesPerRow = 0;

			var indices = bitArray.ToIndices();
			foreach (var index in indices)
			{
				if (entriesPerRow > entriesPerRowMax)
				{
					entriesPerRow = 0;
					sb.AppendLine();
				}

				if (entriesPerRow > 0)
				{
					sb.Append($", ");
				}

				var team = teams[index];
				sb.Append($"{team.Name}");

				++entriesPerRow;
			}

			return sb.ToString();
		}

		public static List<int> ToIndices(this BitArray bitArray)
		{
			var indices = new List<int>();
			for (int i = 0; i < bitArray.Count; ++i)
			{
				if (bitArray[i])
				{
					indices.Add(i);
				}
			}
			return indices;
		}

		public static BitArray FromIndices(this List<int> indices, int bitCount)
		{
			var bitArray = new BitArray(bitCount);
			for (int i = 0; i < indices.Count; ++i)
			{
				bitArray.Set(indices[i], true);
			}
			return bitArray;
		}
	}
}
