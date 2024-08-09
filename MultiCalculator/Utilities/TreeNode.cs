namespace MultiCalculator.Utilities
{
	public class TreeNode<T>
	{
		public T Value { get; set; }

		public List<TreeNode<T>> Children { get; private set; }

		public TreeNode(T value)
		{
			Value = value;
			Children = [];
		}

		public void AddChild(T value)
		{
			Children.Add(new TreeNode<T>(value));
		}

		public TreeNode<T> GetChild(int index)
		{
			return Children[index];
		}
	}
}
