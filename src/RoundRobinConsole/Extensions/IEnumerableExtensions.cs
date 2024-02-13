using System.Collections;
using System.Text;

namespace RoundRobinConsole.Extensions
{
	public static class IEnumerableExtensions
	{
		public static string ToStringNice(this IEnumerable enumerable)
		{
			var sb = new StringBuilder();
			var entriesPerRowMax = 8;
			int entriesPerRow = 0;

			foreach (var value in enumerable)
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

				sb.Append(value.ToString());

				++entriesPerRow;
			}

			return sb.ToString();
		}
	}
}
