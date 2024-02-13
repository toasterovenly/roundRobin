using System.Text.Json.Serialization;

namespace RoundRobinConsole
{
	public class Team : IEquatable<Team>, IComparable<Team>
	{
		[JsonIgnore]
		private static uint _teamNumberMonotonic = 1;

		public uint TeamNumber;

		public string Name;

		// public List<Player> Players = new List<Player>();

		public Team()
		{
			TeamNumber = _teamNumberMonotonic++;
			Name = TeamNumber.ToString();
		}

		public Team(string name)
		{
			TeamNumber = _teamNumberMonotonic++;
			Name = name;
		}

		public bool Equals(Team? other)
		{
			if (other == null)
			{
				return false;
			}
			if (TeamNumber != other.TeamNumber)
			{
				return false;
			}
			//if (Name != other.Name)
			//{
			//	return false;
			//}
			return true;
		}

		public int CompareTo(Team? other)
		{
			if (other == null)
			{
				return -1;
			}
			return TeamNumber.CompareTo(other.TeamNumber);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Team);
		}

		public static bool operator ==(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				return rhs != null;
			}
			return !lhs.Equals(rhs);
		}

		public static bool operator <(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				// Null is always greater than everything except another null.
				return false;
			}
			return lhs.CompareTo(rhs) < 0;
		}

		public static bool operator <=(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return lhs.CompareTo(rhs) <= 0;
		}

		public static bool operator >(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				return rhs != null;
			}
			return lhs.CompareTo(rhs) > 0;
		}

		public static bool operator >=(Team? lhs, Team? rhs)
		{
			if (lhs == null)
			{
				// Null is always greater than everything except another null.
				return true;
			}
			return lhs.CompareTo(rhs) >= 0;
		}

		public override int GetHashCode()
		{
			return TeamNumber.GetHashCode();
		}
	}
}
