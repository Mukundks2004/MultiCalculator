namespace MultiCalculator.Utilities
{
	public class Tree<T>
	{
		public TreeNode<T> Root { get; private set; }

		public Tree(T rootValue)
		{
			Root = new TreeNode<T>(rootValue);
		}

		public void Traverse(TreeNode<T> node, Action<TreeNode<T>> visit)
		{
			visit(node);
			foreach (var child in node.Children)
			{
				Traverse(child, visit);
			}
		}
	}

}
