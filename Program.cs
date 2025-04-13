// === Program.cs (updated) ===
using System;
using System.IO;
using System.Diagnostics; // Add this at the top


namespace JCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter();

            if (args.Length == 0)
            {
                Console.WriteLine("💡 JCore REPL mode. Type 'exit' to quit.\n");

                while (true)
                {
                    Console.Write("> ");
                    var line = Console.ReadLine();
                    if (line == null || line.Trim().ToLower() == "exit")
                        break;

                    var lexer = new Lexer(line);
                    var tokens = lexer.Tokenize();
                    var parser = new Parser(tokens);
                    var statements = parser.Parse();
                    // Inside Main method, before interpreter.Run(statements);
                    var stopwatch = Stopwatch.StartNew();
                   /* interpreter.Run(statements);
                    stopwatch.Stop();

                    Console.WriteLine($"\n⏱ Execution Time: {stopwatch.ElapsedMilliseconds} ms");*/
                    interpreter.Run(statements);
                }
            }
            else
            {
                var path = string.Join(" ", args);  // handles drag-and-drop with spaces
                if (!File.Exists(path))
                {
                    Console.WriteLine($"❌ File not found: {path}");
                    return;
                }

                var code = File.ReadAllText(path);

                var lexer = new Lexer(code);
                var tokens = lexer.Tokenize();
                var parser = new Parser(tokens);
                var statements = parser.Parse();
                interpreter.Run(statements);
            }
        }
    }
}
