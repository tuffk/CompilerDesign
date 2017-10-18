/*
   Jaime Margolin A01019332
   Juan carlos Leon A01020200
   Rodrido Solana A01129839
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
        TokenCategory.IF,
        TokenCategory.SWITCH,
        TokenCategory.WHILE,
        TokenCategory.DO,
        TokenCategory.FOR
};

static readonly ISet<TokenCategory> firstOfOperator =
        new HashSet<TokenCategory>() {
        TokenCategory.AND,
        TokenCategory.LESS,
        TokenCategory.PLUS,
        TokenCategory.MUL,
        TokenCategory.NEG,
        TokenCategory.NOMOR,
        TokenCategory.BITOR,
        TokenCategory.NOMAND,
        TokenCategory.BITAND,
        TokenCategory.EQCOMPARE,
        TokenCategory.NOTEQ,
        TokenCategory.EQLESS,
        TokenCategory.MORE,
        TokenCategory.EQMORE
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

/*Lo necesario para SWITCH statement*/



static readonly ISet<TokenCategory> switchSimpleExpression=
        new HashSet<TokenCategory>() {
        TokenCategory.CASE
};


static readonly ISet<TokenCategory> litSimple =
        new HashSet<TokenCategory>() {
        TokenCategory.INT_LITERAL,
        TokenCategory.CHAR,
        TokenCategory.TRUE,
        TokenCategory.FALSE

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
        Console.WriteLine(CurrentToken2);
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
        }else if(CurrentToken == TokenCategory.IDENTIFIER) {
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

public void ArgumentContinuer()
{
        SimpleExpression();
        if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
          Funcionamela();
        }
        if(CurrentToken == TokenCategory.COMMA)
        {
                Expect(TokenCategory.COMMA);

                ArgumentContinuer();
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
        case TokenCategory.AND:
        case TokenCategory.LESS:
        case TokenCategory.PLUS:
        case TokenCategory.MUL:
        case TokenCategory.NEG:
        case TokenCategory.NOMOR:
        case TokenCategory.BITOR:
        case TokenCategory.NOMAND:
        case TokenCategory.BITAND:
        case TokenCategory.EQCOMPARE:
        case TokenCategory.NOTEQ:
        case TokenCategory.EQLESS:
        case TokenCategory.MORE:
        case TokenCategory.EQMORE:
                Operator();
                Expression();
                Finisher();
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
                ArgumentContinuer();
        }
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        if (CurrentToken == TokenCategory.SEMICOLON)
        {
                Expect(TokenCategory.SEMICOLON);
                return;
        }
        if(CurrentToken == TokenCategory.CURLY_OPEN){
          Expect(TokenCategory.CURLY_OPEN);
          while(CurrentToken != TokenCategory.CURLY_CLOSE)
          {
                  Declaration();
                  Statement();
          }
          Expect(TokenCategory.CURLY_CLOSE);
        }else{
          Expression();
        }
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

        case TokenCategory.SWITCH:
                Switcheamela();
                break;


        case TokenCategory.WHILE:
                Whileamela();
                break;


        case TokenCategory.DO:
                DoWhileamela();
                break;


        case TokenCategory.FOR:
                Foreamesto();
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


public void caseList() {
        Comentamela();

        switch (CurrentToken) {

        case TokenCategory.IDENTIFIER:
                Identificamela();
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
        if(CurrentToken == TokenCategory.IDENTIFIER){
          Identificamela();
        }else if (CurrentToken == TokenCategory.SEMICOLON) {
          Finisher();
        }else {

          Expression();

        }

        if (CurrentToken == TokenCategory.SEMICOLON) {
          Finisher();
        }

}

public void Print() {
        Expect(TokenCategory.PRINT);
        Expression();
}

public void If() {
        Expect(TokenCategory.IF);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        Comentamela();
        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }
        Expect(TokenCategory.CURLY_CLOSE);
        if(CurrentToken == TokenCategory.ELSEIF) {
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

public void Switcheamela() {

        Expect(TokenCategory.SWITCH);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        Comentamela();

        while (switchSimpleExpression.Contains(CurrentToken)) {

                Expect(TokenCategory.CASE);
                BEGIN:
                switchExpresion();
                if(CurrentToken == TokenCategory.COMMA)
                {
                        Expect(TokenCategory.COMMA);
                        goto BEGIN;
                }
                Expect(TokenCategory.COLON);
                while (firstOfStatement.Contains(CurrentToken)) {
                        Statement();
                }

        }

        Expect(TokenCategory.DEFAULT);
        Expect(TokenCategory.COLON);

        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }

        Expect(TokenCategory.CURLY_CLOSE);

}



public void Whileamela() {
        Expect(TokenCategory.WHILE);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);

        while (CurrentToken != TokenCategory.CURLY_CLOSE) {
                Comentamela();
                Declaration();
                Statement();
        }
        Expect(TokenCategory.CURLY_CLOSE);
}


public void DoWhileamela() {
        Expect(TokenCategory.DO);
        Expect(TokenCategory.CURLY_OPEN);
        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }

        Expect(TokenCategory.CURLY_CLOSE);
        Expect(TokenCategory.WHILE);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.SEMICOLON);
}


public void Foreamesto() {

        Expect(TokenCategory.FOR);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expect(TokenCategory.IDENTIFIER);
        Expect(TokenCategory.IN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }
        Expect(TokenCategory.CURLY_CLOSE);

}


public void Expression() {
        Comentamela();
        SimpleExpression();

        while (firstOfOperator.Contains(CurrentToken)) {
                Operator();
                SimpleExpression();
        }
}

public void switchExpresion() {
        switch (CurrentToken) {

        case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

        case TokenCategory.CHAR:
                Expect(TokenCategory.CHAR);
                break;


        case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

        case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

        default:
                throw new SyntaxError(litSimple,
                                      tokenStream.Current);
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
                // throw new SyntaxError(firstOfSimpleExpression,
                //                       tokenStream.Current);
                break;
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
