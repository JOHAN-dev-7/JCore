namespace JCore
{
    public abstract class Node { }

    public class SayNode : Node
    {
        public string Value { get; }
        public bool IsVariable { get; }

        public SayNode(string value, bool isVariable)
        {
            Value = value;
            IsVariable = isVariable;
        }
    }

}