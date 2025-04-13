using System;
using System.IO;
using System.Diagnostics;

namespace JCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter();

            if (args.Length == 0)
            {
                RunRepl(interpreter);
            }
            else
            {
                var path = string.Join(" ", args);  // Handles spaces
                RunFile(interpreter, path);
            }
        }

        static void RunRepl(Interpreter interpreter)
        {
            Console.WriteLine("💡 JCore Beta 1.0.0 REPL mode. Type 'exit' to quit.\n");

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.Trim().ToLower() == "exit")
                    break;

                try
                {
                    var lexer = new Lexer(line);
                    var tokens = lexer.Tokenize();
                    var parser = new Parser(tokens);
                    var statements = parser.Parse();
                    interpreter.Run(statements);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Syntax error: {ex.Message}");
                }
            }
        }

        static void RunFile(Interpreter interpreter, string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"❌ File not found: {path}");
                return;
            }

            try
            {
                var code = File.ReadAllText(path);

                var stopwatch = Stopwatch.StartNew();

                var lexer = new Lexer(code);
                var tokens = lexer.Tokenize();
                var parser = new Parser(tokens);
                var statements = parser.Parse();
                interpreter.Run(statements);

                stopwatch.Stop();
                Console.WriteLine($"\n⏱ Execution Time: {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during file execution: {ex.Message}");
            }
        }
    }
}
