namespace JCore
{
    public abstract class Node { }

    public class SayNode : Node
    {
        public Node Expression { get; }

        public SayNode(Node expression)
        {
            Expression = expression;
        }
    }
}
