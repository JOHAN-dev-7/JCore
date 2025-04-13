namespace JCore
{
    public class UnknownNode : Node
    {
        public string Command { get; }

        public UnknownNode(string command)
        {
            Command = command;
        }
    }
}
