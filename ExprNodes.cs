using System.Numerics;

namespace JCore
{
    public class NumberNode : Node
    {
        public BigInteger Value { get; }

        public NumberNode(BigInteger value)
        {
            Value = value;
        }
    }

    public class VariableNode : Node
    {
        public string Name { get; }
        public VariableNode(string name)
        {
            Name = name;
        }
    }

    public class BinaryOpNode : Node
    {
        public Node Left { get; }
        public string Operator { get; }
        public Node Right { get; }

        public BinaryOpNode(Node left, string op, Node right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}
