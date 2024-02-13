namespace RoundRobinConsole
{
	public interface IGame
	{
		public List<Team> Teams { get; }

		public string GetGameString();
	}
}
