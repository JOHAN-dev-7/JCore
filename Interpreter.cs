using System;
using System.Collections.Generic;
using System.Numerics;

namespace JCore
{
    public class Interpreter
    {
        private readonly Dictionary<string, string> variables = new();
        private readonly HashSet<string> constants = new();

        public void Run(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                switch (node)
                {
                    case SayNode say:
                        var result = Evaluate(say.Expression);
                        Console.WriteLine(result);
                        break;

                    case LetNode let:
                        if (constants.Contains(let.Name))
                        {
                            Console.WriteLine($"❌ Cannot overwrite constant '{let.Name}' with let.");
                        }
                        else
                        {
                            var value = Evaluate(let.Value);
                            variables[let.Name] = value.ToString();
                        }
                        break;

                    case ConstNode constant:
                        if (constants.Contains(constant.Name))
                        {
                            Console.WriteLine($"❌ Constant '{constant.Name}' already defined.");
                        }
                        else
                        {
                            var value = Evaluate(constant.Value);
                            variables[constant.Name] = value.ToString();
                            constants.Add(constant.Name);
                        }
                        break;

                    case UnknownNode unknown:
                        Console.WriteLine($"❌ Unknown command: {unknown.Command}");
                        break;

                    default:
                        Console.WriteLine("⚠️ Unhandled node type.");
                        break;
                }
            }
        }

        private object Evaluate(Node node)
        {
            return node switch
            {
                NumberNode n => n.Value,
                StringNode s => s.Value,
                VariableNode v => variables.TryGetValue(v.Name, out var val)
                    ? val
                    : throw new Exception($"❌ Undefined variable '{v.Name}'"),
                BinaryOpNode b => EvaluateOp(Evaluate(b.Left), b.Operator, Evaluate(b.Right)),
                _ => throw new Exception("❌ Invalid expression")
            };
        }

        private BigInteger EvaluateOp(object left, string op, object right)
        {
            if (!BigInteger.TryParse(left.ToString(), out var l))
                throw new Exception($"❌ Cannot perform math on '{left}'");

            if (!BigInteger.TryParse(right.ToString(), out var r))
                throw new Exception($"❌ Cannot perform math on '{right}'");

            return op switch
            {
                "PLUS" => l + r,
                "MINUS" => l - r,
                "STAR" => l * r,
                "SLASH" => r != 0 ? l / r : throw new Exception("❌ Division by zero"),
                _ => throw new Exception($"❌ Unknown operator '{op}'")
            };
        }
    }
}
