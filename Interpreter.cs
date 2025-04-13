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
                        if (say.IsVariable)
                        {
                            if (variables.TryGetValue(say.Value, out var val))
                                Console.WriteLine(val);
                            else
                                Console.WriteLine($"❌ Undefined variable '{say.Value}'");
                        }
                        else
                        {
                            Console.WriteLine(say.Value);
                        }
                        break;


                        break;

                    case LetNode let:
                        if (constants.Contains(let.Name))
                            Console.WriteLine($"❌ Cannot overwrite constant '{let.Name}' with let.");
                        else
                            variables[let.Name] = Evaluate(let.Value).ToString();
                        break;

                    case ConstNode constant:
                        if (constants.Contains(constant.Name))
                            Console.WriteLine($"❌ Constant '{constant.Name}' already defined.");
                        else
                        {
                            variables[constant.Name] = Evaluate(constant.Value).ToString();
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



        private BigInteger Evaluate(Node node)
        {
            return node switch
            {
                NumberNode n => n.Value,
                VariableNode v => BigInteger.TryParse(variables.GetValueOrDefault(v.Name), out var val)
                    ? val
                    : throw new Exception($"❌ Undefined variable '{v.Name}'"),
                BinaryOpNode b => EvaluateOp(Evaluate(b.Left), b.Operator, Evaluate(b.Right)),
                _ => throw new Exception("❌ Invalid expression")
            };
        }


        private BigInteger EvaluateOp(BigInteger left, string op, BigInteger right)
        {
            return op switch
            {
                "PLUS" => left + right,
                "MINUS" => left - right,
                "STAR" => left * right,
                "SLASH" => right != 0 ? left / right : throw new Exception("❌ Division by zero"),
                _ => throw new Exception($"❌ Unknown operator '{op}'")
            };
        }




    }






}
