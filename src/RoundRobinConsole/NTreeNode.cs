namespace RoundRobinConsole
{
	internal class NTreeNode
	{
		public int Value;
		public int Depth;
		public List<NTreeNode> Children;
		public int ChildCount => Children.Count;

		public NTreeNode(int data, int depth)
		{
			Value = data;
			Depth = depth;
			Children = new List<NTreeNode>();
		}

		public NTreeNode(int data, int depth, List<NTreeNode>? children)
		{
			Value = data;
			Depth = depth;
			Children = children ?? new List<NTreeNode>();
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
