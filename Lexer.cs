using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JCore
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public Token(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public class Lexer
    {
        private string _code;

        private static readonly Dictionary<string, string> Keywords = new()
        {
            { "say", "SAY" },
            { "let", "LET" },
            { "make", "MAKE" }
        };

        private static readonly Dictionary<string, string> Operators = new()
        {
            { "=", "EQUAL" },
            { "+", "PLUS" },
            { "-", "MINUS" },
            { "*", "STAR" },
            { "/", "SLASH" },
            { "(", "LPAREN" },
            { ")", "RPAREN" }
        };

        private static readonly Regex TokenPattern = new(
            @"(?<STRING>""[^""\\]*(?:\\.[^""\\]*)*"")|(?<OP>[=+\-*/()])|(?<NUMBER>\d+)|(?<IDENT>[a-zA-Z_][a-zA-Z0-9_]*)",
            RegexOptions.Compiled);

        public Lexer(string code)
        {
            _code = code;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            _code = Regex.Replace(_code, @"/\*.*?\*/", "", RegexOptions.Singleline);

            var lines = _code.Split('\n');
            foreach (var rawLine in lines)
            {
                var line = StripInlineComment(rawLine.Trim());
                if (string.IsNullOrWhiteSpace(line)) continue;

                var matches = TokenPattern.Matches(line);
                foreach (Match match in matches)
                {
                    if (match.Groups["STRING"].Success)
                    {
                        var raw = match.Groups["STRING"].Value;
                        var str = raw.Substring(1, raw.Length - 2);
                        tokens.Add(new Token("STRING", str));
                    }
                    else if (match.Groups["NUMBER"].Success)
                    {
                        tokens.Add(new Token("NUMBER", match.Groups["NUMBER"].Value));
                    }
                    else if (match.Groups["IDENT"].Success)
                    {
                        var value = match.Groups["IDENT"].Value;
                        if (Keywords.TryGetValue(value, out var keywordType))
                            tokens.Add(new Token(keywordType, value));
                        else
                            tokens.Add(new Token("IDENT", value));
                    }
                    else if (match.Groups["OP"].Success)
                    {
                        var op = match.Groups["OP"].Value;
                        if (Operators.TryGetValue(op, out var opType))
                            tokens.Add(new Token(opType, op));
                        else
                            throw new Exception($"Unknown operator: {op}");
                    }
                }
            }

            return tokens;
        }

        private string StripInlineComment(string line)
        {
            bool inString = false;
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (line[i] == '"') inString = !inString;

                if (!inString && line[i] == '/' && line[i + 1] == '/')
                    return line.Substring(0, i).Trim();
            }
            return line;
        }
    }
}
