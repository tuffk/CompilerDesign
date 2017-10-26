//---------------------------------------------------------
// Jaime margolin a01019332
// juan carlos leon
// rodrigo solana
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public enum TokenCategory {
    ATOM, EOL, ILLEGAL,
    PAR_OPEN, PAR_CLOSE,
    SQUARE_OPEN, SQUARE_CLOSE,
    ANGLE_OPEN, ANGLE_CLOSE,
    CURLY_OPEN, CURLY_CLOSE
}

public class Token {

    TokenCategory category;

    public TokenCategory Category {
        get { return category; }
    }

    public Token(TokenCategory category) {
        this.category = category;
    }
}

public class Scanner {

    readonly String input;

    static readonly Regex regex =
        new Regex(
            @"(\w+)|(\s)
            |(\() | (\))
            |(\[) | (\])
            |(\<) | (\>)
            |(\{) | (\})
            |(.)",
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
        );

    public Scanner(String input) {
        this.input = input;
    }

    public IEnumerable<Token> Start() {
        foreach (Match m in regex.Matches(input)) {
            if (m.Groups[1].Success) {
                yield return new Token(TokenCategory.ATOM);
            } else if (m.Groups[2].Success) {
                continue;
            } else if(m.Groups[3].Success) {
                yield return new Token(TokenCategory.PAR_OPEN);
            } else if(m.Groups[4].Success) {
                yield return new Token(TokenCategory.PAR_CLOSE);
            } else if(m.Groups[5].Success) {
                yield return new Token(TokenCategory.SQUARE_OPEN);
            } else if(m.Groups[6].Success) {
                yield return new Token(TokenCategory.SQUARE_CLOSE);
            } else if(m.Groups[7].Success) {
                yield return new Token(TokenCategory.ANGLE_OPEN);
            } else if(m.Groups[8].Success) {
                yield return new Token(TokenCategory.ANGLE_CLOSE);
            } else if(m.Groups[9].Success) {
                yield return new Token(TokenCategory.CURLY_OPEN);
            } else if(m.Groups[10].Success) {
                yield return new Token(TokenCategory.CURLY_CLOSE);
            } else if (m.Groups[11].Success) {
                yield return new Token(TokenCategory.ILLEGAL);
            }
        }
        yield return new Token(TokenCategory.EOL);
    }
}

class SyntaxError: Exception {
}

public class Parser {

    IEnumerator<Token> tokenStream;

    public Parser(IEnumerator<Token> tokenStream) {
        this.tokenStream = tokenStream;
        this.tokenStream.MoveNext();
    }

    public TokenCategory Current {
        get { return tokenStream.Current.Category; }
    }

    public Token Expect(TokenCategory category) {
        if (Current == category) {
            Token current = tokenStream.Current;
            tokenStream.MoveNext();
            return current;
        } else {
            throw new SyntaxError();
        }
    }

    public void Mbdle() {
        switch (Current) {
          case TokenCategory.ATOM:
                Expect(TokenCategory.ATOM);
                break;
          case TokenCategory.PAR_OPEN:
            Expect(TokenCategory.PAR_OPEN);
            break;
          case TokenCategory.PAR_CLOSE:
            Expect(TokenCategory.PAR_CLOSE);
            break;
          default:
          Console.WriteLine($"pito: {Current}");
          break;
        }
        Expect(TokenCategory.EOL);
    }
}

public class MBDLE {

    public static void Main(String[] args) {
        try {
            while (true) {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) {
                    break;
                }
                var parser = new Parser(new Scanner(line).Start().GetEnumerator());
                parser.Mbdle();
                Console.WriteLine("syntax ok");
            }
        } catch (SyntaxError) {
            Console.WriteLine("syntax error");
        }
    }
}
