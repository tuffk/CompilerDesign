/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Collections.Generic;

namespace Buttercup {

    class Parser {

        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                TokenCategory.VAR
            };

        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.PRINT,
                TokenCategory.IF
            };

        static readonly ISet<TokenCategory> firstOfOperator =
            new HashSet<TokenCategory>() {
                TokenCategory.AND,
                TokenCategory.LESS,
                TokenCategory.PLUS,
                TokenCategory.MUL
            };

        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.INT_LITERAL,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.PARENTHESIS_OPEN,
                TokenCategory.NEG
            };

        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }
        public Token CurrentToken2 {
            get { return tokenStream.Current; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public void Program() {

            Comentamela();
            while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
                Comentamela();
            }

            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
                Comentamela();
            }

            Expect(TokenCategory.EOF);
        }
        public void Comentamela()
        {
          if (CurrentToken == TokenCategory.COMMENT) {
            Expect(TokenCategory.COMMENT);
          }
          if (CurrentToken == TokenCategory.COMMENT) {
            Comentamela();
          }
        }

        public void Finisher(){
            Expect(TokenCategory.SEMICOLON);
        }

        public void Declaration() {
          Comentamela();
            switch (CurrentToken) {
              case TokenCategory.VAR:
                Vareamela();
                break;
            case TokenCategory.COMMENT:
              Expect(TokenCategory.COMMENT);
              break;

              default:
                break;
            }
        }

        public void Vareamela(){
          Expect(TokenCategory.VAR);
          Expect(TokenCategory.IDENTIFIER);
          if(CurrentToken == TokenCategory.COMMA)
          {
            Expect(TokenCategory.COMMA);
            DeclarationContinuer();
          }else if(CurrentToken == TokenCategory.IDENTIFIER){
            throw new SyntaxError(TokenCategory.COMMA, tokenStream.Current);
          }
          Finisher();
        }

        public void DeclarationContinuer()
        {

          Expect(TokenCategory.IDENTIFIER);
          if(CurrentToken == TokenCategory.COMMA)
          {
            Expect(TokenCategory.COMMA);
            DeclarationContinuer();
          }
        }

        public void Identificamela()
        {
          Expect(TokenCategory.IDENTIFIER);
          switch(CurrentToken)
          {
              case TokenCategory.PARENTHESIS_OPEN:
                Funcionamela();
                break;
              case TokenCategory.ASSIGN:
                Assignment();
                break;
              default:
                break;
          }
        }

        public void Funcionamela()
        {
          Expect(TokenCategory.PARENTHESIS_OPEN);
          Comentamela();
          if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
          {
            DeclarationContinuer();
          }
          Expect(TokenCategory.PARENTHESIS_CLOSE);
          if (CurrentToken == TokenCategory.SEMICOLON)
          {
            Expect(TokenCategory.SEMICOLON);
            return;
          }
          Expect(TokenCategory.CURLY_OPEN);
          while(CurrentToken != TokenCategory.CURLY_CLOSE)
          {
            Declaration();
            Statement();
          }
          Expect(TokenCategory.CURLY_CLOSE);
        }

        public void Statement() {
          Comentamela();


            switch (CurrentToken) {

              case TokenCategory.IDENTIFIER:
                Identificamela();
                break;

              case TokenCategory.PRINT:
                  Print();
                  break;

              case TokenCategory.IF:
                  If();
                  break;

              case TokenCategory.COMMENT:
                Expect(TokenCategory.COMMENT);
                break;

              default:
                  // throw new SyntaxError(firstOfStatement, tokenStream.Current);
                  break;
            }
            Comentamela();
        }

        public void Type() {
            switch (CurrentToken) {

            case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

            case TokenCategory.BOOL:
                Expect(TokenCategory.BOOL);
                break;

            default:
                throw new SyntaxError(firstOfDeclaration,
                                      tokenStream.Current);
            }
        }

        public void Assignment() {
            Expect(TokenCategory.ASSIGN);
            Expression();
            Finisher();
        }

        public void Print() {
            Expect(TokenCategory.PRINT);
            Expression();
        }

        public void If() {
            Expect(TokenCategory.IF);
            Expression();
            Expect(TokenCategory.CURLY_OPEN);
            Comentamela();
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.CURLY_CLOSE);
            if(CurrentToken == TokenCategory.ELSEIF){
              Comentamela();
              RecursiveameEnElIf();
            }
            if (CurrentToken == TokenCategory.ELSE) {
              Expect(TokenCategory.ELSE);
              Expect(TokenCategory.CURLY_OPEN);
              Comentamela();
              Statement();
              Expect(TokenCategory.CURLY_CLOSE);
            }
        }

        public void RecursiveameEnElIf()
        {
          Expect(TokenCategory.ELSEIF);
          Expect(TokenCategory.PARENTHESIS_OPEN);
          DeclarationContinuer();
          Expect(TokenCategory.PARENTHESIS_CLOSE);
          Expect(TokenCategory.CURLY_OPEN);
          Statement();
          Expect(TokenCategory.CURLY_CLOSE);
          if(CurrentToken == TokenCategory.ELSEIF)
          {
            RecursiveameEnElIf();
          }
        }

        public void Expression() {
          Comentamela();
            SimpleExpression();
            while (firstOfOperator.Contains(CurrentToken)) {
                Operator();
                SimpleExpression();
            }
        }

        public void SimpleExpression() {
            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Expect(TokenCategory.IDENTIFIER);
                break;

            case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

            case TokenCategory.BIN:
                Expect(TokenCategory.BIN);
                break;

            case TokenCategory.CHAR:
                Expect(TokenCategory.CHAR);
                break;

            case TokenCategory.STRING:
                Expect(TokenCategory.STRING);
                break;

            case TokenCategory.OCT:
                Expect(TokenCategory.OCT);
                break;

            case TokenCategory.HEX:
                Expect(TokenCategory.HEX);
                break;

            case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

            case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

            case TokenCategory.PARENTHESIS_OPEN:
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                break;

            case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                SimpleExpression();
                break;

            default:
                throw new SyntaxError(firstOfSimpleExpression,
                                      tokenStream.Current);
            }
        }

        public void Operator() {

            switch (CurrentToken) {

            case TokenCategory.AND:
                Expect(TokenCategory.AND);
                break;

            case TokenCategory.LESS:
                Expect(TokenCategory.LESS);
                break;

            case  TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;

            case TokenCategory.MUL:
                Expect(TokenCategory.MUL);
                break;

            case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                break;

            case TokenCategory.NOMOR:
                Expect(TokenCategory.NOMOR);
                break;

            case TokenCategory.BITOR:
                Expect(TokenCategory.BITOR);
                break;

            case TokenCategory.NOMAND:
                Expect(TokenCategory.NOMAND);
                break;

            case TokenCategory.BITAND:
                Expect(TokenCategory.BITAND);
                break;

            case TokenCategory.EQCOMPARE:
                Expect(TokenCategory.EQCOMPARE);
                break;

            case TokenCategory.NOTEQ:
                Expect(TokenCategory.NOTEQ);
                break;

            case TokenCategory.MORE:
                Expect(TokenCategory.MORE);
                break;

            case TokenCategory.EQMORE:
                Expect(TokenCategory.EQMORE);
                break;

            case TokenCategory.EQLESS:
                Expect(TokenCategory.EQLESS);
                break;

            default:
                throw new SyntaxError(firstOfOperator,
                                      tokenStream.Current);
            }
        }
    }
}
