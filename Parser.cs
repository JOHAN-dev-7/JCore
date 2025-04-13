using System.Collections.Generic;
using System.Numerics;

namespace JCore
{
    public class LetNode : Node
    {
        public string Name { get; }
        public Node Value { get; }

        public LetNode(string name, Node value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ConstNode : Node
    {
        public string Name { get; }
        public Node Value { get; }

        public ConstNode(string name, Node value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Node> Parse()
        {
            var nodes = new List<Node>();
            while (!IsAtEnd())
            {
                nodes.Add(ParseStatement());
            }
            return nodes;
        }

        private Node ParseStatement()
        {
            if (Match("SAY"))
            {
                if (IsAtEnd())
                    throw new System.Exception("Expected something after 'say'");

                var expr = ParseExpression();
                return new SayNode(expr);
            }

            if (Match("LET"))
            {
                if (IsAtEnd())
                    throw new System.Exception("Expected variable name after 'let'");

                var name = Consume("IDENT").Value;

                if (IsAtEnd())
                    throw new System.Exception("Expected '=' after variable name");

                Consume("EQUAL");

                if (IsAtEnd())
                    throw new System.Exception("Expected value after '='");

                var expr = ParseExpression();
                return new LetNode(name, expr);
            }

            if (Match("MAKE"))
            {
                if (IsAtEnd())
                    throw new System.Exception("Expected constant name after 'make'");

                var name = Consume("IDENT").Value;

                if (IsAtEnd())
                    throw new System.Exception("Expected '=' after constant name");

                Consume("EQUAL");

                if (IsAtEnd())
                    throw new System.Exception("Expected value after '='");

                var expr = ParseExpression();
                return new ConstNode(name, expr);
            }

            if (IsAtEnd())
                throw new System.Exception("Unexpected end of input");

            var unknown = _tokens[_pos++];
            return new UnknownNode(unknown.Value);
        }

        private Node ParseExpression(int precedence = 0)
        {
            if (IsAtEnd())
                throw new System.Exception("Unexpected end of expression");

            Node left;

            var token = _tokens[_pos++];

            if (token.Type == "NUMBER")
            {
                left = new NumberNode(BigInteger.Parse(token.Value));
            }
            else if (token.Type == "STRING")
            {
                left = new StringNode(token.Value);
            }
            else if (token.Type == "IDENT")
            {
                left = new VariableNode(token.Value);
            }
            else if (token.Type == "LPAREN")
            {
                left = ParseExpression();
                Consume("RPAREN");
            }
            else
            {
                throw new System.Exception($"Unexpected token '{token.Value}'");
            }

            while (!IsAtEnd() && GetPrecedence(Current()) > precedence)
            {
                var opToken = _tokens[_pos++];
                var right = ParseExpression(GetPrecedence(opToken));
                left = new BinaryOpNode(left, opToken.Type, right);
            }

            return left;
        }

        private Token Current() => _tokens[_pos];

        private int GetPrecedence(Token token)
        {
            return token.Type switch
            {
                "PLUS" => 1,
                "MINUS" => 1,
                "STAR" => 2,
                "SLASH" => 2,
                _ => 0,
            };
        }

        private Token Consume(params string[] expectedTypes)
        {
            if (IsAtEnd())
                throw new System.Exception($"Expected {string.Join(" or ", expectedTypes)} but found end of input");

            var token = _tokens[_pos];
            if (System.Array.IndexOf(expectedTypes, token.Type) == -1)
                throw new System.Exception($"Expected token of type: {string.Join(" or ", expectedTypes)}, but got '{token.Value}'");

            _pos++;
            return token;
        }

        private bool Match(string type)
        {
            if (IsAtEnd()) return false;
            if (_tokens[_pos].Type == type)
            {
                _pos++;
                return true;
            }
            return false;
        }

        private bool IsAtEnd()
        {
            return _pos >= _tokens.Count;
        }
    }
}
