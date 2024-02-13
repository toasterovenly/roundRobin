using System.Text;

namespace RoundRobinConsole
{
	internal class NTree
	{
		public readonly int N;
		public List<NTreeNode> Roots;
		public int RootCount => Roots.Count;
		private readonly int _numWidth;
		private readonly string _spacing;
		private readonly string _formatString;

		/// <summary>
		/// true means tall, false means wide.
		/// </summary>
		public bool ToStringModeIsTall = true;

		public NTree(int n)
		{
			N = n;
			_numWidth = N / 10 + 1;
			_spacing = new string(' ', _numWidth);
			_formatString = $"D{_numWidth}";
			//_formatString = new string('0', _numWidth);

			Roots = GenerateTreeRecursive(N, 0);
		}

		public List<NTreeNode> GenerateTreeRecursive(int n, int depth)
		{
			if (depth >= n)
			{
				return new List<NTreeNode>();
			}

			var nodes = new List<NTreeNode>();
			for (int i = 0; i < n; ++i)
			{
				var children = GenerateTreeRecursive(n, depth + 1);
				NTreeNode node = new NTreeNode(i, depth, children);
				nodes.Add(node);
			}
			return nodes;
		}

		public override string ToString()
		{
			if (ToStringModeIsTall)
			{
				return ToStringTall();
			}
			else
			{
				return ToStringWide();
			}
		}

		public string ToStringTall()
		{
			if (RootCount <= 0)
			{
				return $"Tree has no roots.";
			}

			var sb = new StringBuilder();

			LinkedList<NTreeNode> queue = new LinkedList<NTreeNode>();
			foreach (var root in Roots)
			{
				queue.AddLast(root);
			}

			var prevDepth = -1;
			while (queue.Count > 0)
			{
				var listNode = queue.First;
				var treeNode = listNode?.Value;
				queue.RemoveFirst();

				if (listNode == null || treeNode == null)
				{
					Console.Write("Null node found.");
					continue;
				}

				if (prevDepth >= treeNode.Depth)
				{
					sb.AppendLine();

					for (int s = 0; s < treeNode.Depth; ++s)
					{
						sb.Append($"{_spacing} ");
					}
				}
				prevDepth = treeNode.Depth;

				sb.Append($"{treeNode.Value.ToString(_formatString)} ");

				var children = treeNode.Children;
				for (int j = children.Count - 1; j >= 0; --j)
				{
					var child = children[j];
					queue.AddFirst(child);
				}
			}

			sb.AppendLine();

			return sb.ToString();
		}

		public string ToStringWide()
		{
			if (RootCount <= 0)
			{
				return $"Tree has no roots.";
			}

			var sb = new StringBuilder();

			var queue = new Queue<NTreeNode>();
			foreach (var root in Roots)
			{
				queue.Enqueue(root);
			}

			var depth = 0;
			while (queue.Count > 0)
			{
				var currentNodeCount = queue.Count;
				for (int i = 0; i < currentNodeCount; ++i)
				{
					var treeNode = queue.Dequeue();

					if (treeNode == null)
					{
						Console.Write("Null node found.");
						continue;
					}

					sb.Append($"{treeNode.Value.ToString(_formatString)} ");

					var remainingDepth = N - 1 - depth;
					var spacingCount = Math.Pow(N, remainingDepth) - 1;
					for (int s = 0; s < spacingCount; ++s)
					{
						sb.Append($"{_spacing} ");
					}

					foreach (var child in treeNode.Children)
					{
						queue.Enqueue(child);
					}
				}
				sb.AppendLine();
				++depth;
			}

			return sb.ToString();
		}
	}
}
