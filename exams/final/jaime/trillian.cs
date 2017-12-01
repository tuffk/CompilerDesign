// Jaime Margolin A01019332
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Trillian {

    //---------------------------------------------------------------
    class SyntaxError: Exception {}

    //---------------------------------------------------------------
    enum Token {
        BANG, COMMA, STAR, SQ_OPEN, SQ_CLOSE, FLOAT,
        ILLEGAL_CHAR, EOF
    }

    class Kuz{
      public Token tok;
      public string val;
    }

    //---------------------------------------------------------------
    class Scanner {
        readonly string input;
        public string val;
        static readonly Regex regex = new Regex(
            @"
                (?<NMax>       [!] )
              | (?<Comma>         [,] )
              | (?<Dup>        [*] )
              | (?<SqOpen>    \[ )
              | (?<SqClose>   \] )
              | (?<Float>       [-]* \d+ [.]* \d* )
              | (?<WhiteSpace> \s  )
              | (?<Other>      .   )
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
            );
        static readonly IDictionary<string, Token> regexLabels =
            new Dictionary<string, Token>() {
                {"NMax", Token.BANG},
                {"Comma", Token.COMMA},
                {"Dup", Token.STAR},
                {"SqOpen", Token.SQ_OPEN},
                {"SqClose", Token.SQ_CLOSE},
                {"Float", Token.FLOAT}
            };
        public Scanner(string input) {
            this.input = input;
        }
        public IEnumerable<Kuz> Start() {
            foreach (Match m in regex.Matches(input)) {
                if (m.Groups["WhiteSpace"].Success) {
                    // Skip spaces.
                } else if (m.Groups["Other"].Success) {
                    var k =new Kuz();
                    k.val = "";
                    k.tok = Token.ILLEGAL_CHAR;
                    yield return k;
                }else {
                    foreach (var name in regexLabels.Keys) {
                        if (m.Groups[name].Success) {
                            Console.WriteLine($"NAME: {name}, val: {m}");

                            var zain = new Kuz();
                            zain.tok = regexLabels[name];
                            zain.val = $"{m}";
                            yield return zain;
                            break;
                        }
                    }
                }
            }
            var k2 =new Kuz();
            k2.val = "";
            k2.tok = Token.EOF;
            yield return k2;
        }
    }

    //---------------------------------------------------------------
    class Node: IEnumerable<Node> {
        public IList<Node> children = new List<Node>();
        public Node this[int index] {
            get {
                return children[index];
            }
        }
        public string Lexeme;
        public void Add(Node node) {
            children.Add(node);
        }
        public IEnumerator<Node> GetEnumerator() {
            return children.GetEnumerator();
        }
        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
        public override string ToString() {
            return GetType().Name;
        }
        public string ToStringTree() {
            var sb = new StringBuilder();
            TreeTraversal(this, "", sb);
            return sb.ToString();
        }
        static void TreeTraversal(
                Node node,
                string indent,
                StringBuilder sb) {
            sb.Append(indent);
            sb.Append(node);
            sb.Append('\n');
            foreach (var child in node.children) {
                TreeTraversal(child, indent + "  ", sb);
            }
        }
    }

    //---------------------------------------------------------------
    class Program:   Node {}
    class NMax:       Node {}
    class Dup:        Node {}
    class Sum:       Node {}
    class Float: Node {
      public Kuz algo;
      public Float(Kuz x)
      {
        this.algo = x;
      }

      public override string ToString() {
          return algo.val;
      }

    }

    //---------------------------------------------------------------
    class Parser {
        IEnumerator<Kuz> tokenStream;
        public Parser(IEnumerator<Kuz> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }
        public Kuz CurrentToken {
            get { return tokenStream.Current; }
        }
        public Token Expect(Token category) {
            if (CurrentToken.tok == category) {
                Token current = tokenStream.Current.tok;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError();
            }
        }
        public Node Start() {
            var e = new Program() { Max() };
            Expect(Token.EOF);
            return e;
        }

        public Node Max() {
          Console.WriteLine($"++ MAX ++");
            var exp1 = SimpleExp();
            Console.WriteLine($"expr1 {exp1}");
            while (CurrentToken.tok == Token.BANG) {
                Expect(Token.BANG);
                var exp2 = new NMax() { exp1, SimpleExp() };
                exp1 = exp2;
            }
            return exp1;
        }
        public Node MaxList() {
          Console.WriteLine($"++ MaxList ++");
            var exp1 = Max();
            while (CurrentToken.tok == Token.COMMA) {
                Expect(Token.COMMA);
                 var exp2 = new Sum() { exp1, Max() };
                 exp1 = exp2;
            }
            return exp1;
        }
        public Node SimpleExp() {
          Console.WriteLine($"++ SimpleExp con {CurrentToken.tok} ++");
            switch (CurrentToken.tok) {
            case Token.SQ_OPEN:
                Console.WriteLine("-- SUM --");
                Expect(Token.SQ_OPEN);
                var exp = new Sum();
                exp.Add(Max());
                while (CurrentToken.tok == Token.COMMA) {
                    Expect(Token.COMMA);
                     exp.Add(Max());
                }
                Expect(Token.SQ_CLOSE);
                return exp;
            case Token.STAR:
                Console.WriteLine("-- STAR -- ");
                Expect(Token.STAR);
                return new Dup() { SimpleExp() };
            case Token.FLOAT:
                Console.WriteLine("-- FLOAT --");
                var y = CurrentToken;
                //Console.WriteLine($"********************** {y.tok} *************");
                Expect(Token.FLOAT);
                return new Float(y);
            default:
                throw new SyntaxError();
            }
        }


    }

    //---------------------------------------------------------------
    class CILGenerator {
        public string Visit(Program node) {
          Console.WriteLine($"visitamela {node} con hijos: {node[0]} ");
            return ".assembly 'Trillian' {}\n\n"
                + ".class public 'final_exam' extends ['mscorlib']'System'.'Object' {\n"
                + "\t.method public static void 'start'() {\n"
                + "\t\t.entrypoint\n"
                + Visit((dynamic) node[0])
                + "\t\tcall void ['mscorlib']'System'.'Console'::'WriteLine'(float64)\n"
                + "\t\tret\n"
                + "\t}\n"
                + "}\n";
        }
        public string Visit(NMax node) {
          Console.WriteLine($"visit bang ");
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\tcall float64 ['mscorlib']'System'.'Math'::'Max'(float64, float64)\n";
        }
        public string Visit(Dup node) {
          Console.WriteLine($"visit node");
            return Visit((dynamic) node[0])
                + "\t\tdup\n\t\tadd\n";
        }
        public string Visit(Sum node) {
          Console.WriteLine($"visit sum");
            // return Visit((dynamic) node[0])
            //     + Visit((dynamic) node[1])
            //     + "\t\tadd\n";
            var cont =0;
            var sb = new StringBuilder();
            foreach(var c in node.children)
            {
              if (cont == 0) {
                sb.Append(Visit((dynamic) c));
                sb.Append("\n");
                cont++;
                continue;
              }
              sb.Append(Visit((dynamic) c));
              sb.Append("\n");
              sb.Append("\t\tadd \n");
            }
            return sb.ToString();
        }
        public string Visit(Float node) {
          Console.WriteLine("visit float");
            return $"\t\tldc.r8 {node.algo.val}\n";
        }
    }

    //---------------------------------------------------------------
    class Driver {
        public static void Main(string[] args) {
            try {
                var p = new Parser(
                    new Scanner(args[0]).Start().GetEnumerator());
                var ast = p.Start();
                Console.Write(ast.ToStringTree());
                File.WriteAllText(
                    "output.il",
                    new CILGenerator().Visit((dynamic) ast));
            } catch (SyntaxError) {
                Console.Error.WriteLine("parse error");
                Environment.Exit(1);
            }
        }
    }
}
