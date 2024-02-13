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
}
