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

        public Lexer(string code)
        {
            _code = code;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            // 1️⃣ Remove multiline block comments: /* ... */
            _code = Regex.Replace(_code, @"/\*.*?\*/", "", RegexOptions.Singleline);

            // 2️⃣ Then split into lines and strip single-line comments
            var lines = _code.Split('\n');

            foreach (var line in lines)
            {
                string codeLine = line.Split("//")[0].Trim(); // Remove inline comments
                if (string.IsNullOrWhiteSpace(codeLine)) continue;

                // Tokenize this cleaned line
                var words = Regex.Matches(codeLine, @"\""[^""]*\""|[\+\-\*/\(\)=]|\n|\r|\t|\s+|[^\s\+\-\*/\(\)=]+");

                foreach (Match word in words)
                {
                    var text = word.Value;
                    if (string.IsNullOrWhiteSpace(text)) continue;

                    switch (text)
                    {
                        case "say":
                            tokens.Add(new Token("SAY", text));
                            break;
                        case "let":
                            tokens.Add(new Token("LET", text));
                            break;
                        case "make":
                            tokens.Add(new Token("MAKE", text));
                            break;
                        case "=":
                            tokens.Add(new Token("EQUAL", text));
                            break;
                        case "+":
                            tokens.Add(new Token("PLUS", text));
                            break;
                        case "-":
                            tokens.Add(new Token("MINUS", text));
                            break;
                        case "*":
                            tokens.Add(new Token("STAR", text));
                            break;
                        case "/":
                            tokens.Add(new Token("SLASH", text));
                            break;
                        case "(":
                            tokens.Add(new Token("LPAREN", text));
                            break;
                        case ")":
                            tokens.Add(new Token("RPAREN", text));
                            break;
                        default:
                            if (text.StartsWith("\"") && text.EndsWith("\""))
                                tokens.Add(new Token("STRING", text.Trim('"')));
                            else if (Regex.IsMatch(text, @"^\d+$"))
                                tokens.Add(new Token("NUMBER", text));

                            else
                                tokens.Add(new Token("IDENT", text));//g
                            break;
                    }

                }
            }

            return tokens;
        }


    }
}
