/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

namespace Buttercup {

    enum TokenCategory {
        AND,
        ASSIGN,
        BOOL,
        END,
        EOF,
        FALSE,
        IDENTIFIER,
        IF,
        INT_LITERAL,
        LESS,
        MUL,
        NEG,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        PLUS,
        PRINT,
        THEN,
        TRUE,
        ILLEGAL_CHAR,
        //
        BREAK,
        ELSE,
        RETURN,
        CASE,
        SWITCH,
        CONTINUE,
        FOR,
        DEFAULT,
        WHILE,
        DO,
        IN,
        VAR,
        CURLY_OPEN,
        CURLY_CLOSE,
        SEMICOLON,
        INLINEIF,
        COLON,
        NOMOR,
        BITOR,
        NOMAND,
        BITAND,
        EQCOMPARE,
        NOTEQ,
        MORE,
        EQMORE,
        COMMA,
        // ints
        INT,
        BIN,
        OCT,
        HEX,
        COMMENT
    }
}
