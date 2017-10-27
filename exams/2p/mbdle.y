//---------------------------------------------------------
// Jaime margolin a01019332
// juan carlos leon
// rodrigo solana
//---------------------------------------------------------

%{

#include <stdio.h>
#include <stdarg.h>

int yylex(void);
void yyerror(char *s, ...);

%}

%union {
    int ival;
}

/* declare tokens */
%token ATOM ILLEGAL EOL PAR_OPEN PAR_CLOSE SQUARE_OPEN SQUARE_CLOSE ANGLE_OPEN ANGLE_CLOSE CURLY_OPEN CURLY_CLOSE

%%

mbdle:
    /* nothing */ { }                              /* Matches at beginning of input */
    | mbdle prog  EOL { printf("syntax ok\n> "); }
    | atini   EOL { printf("syntax ok\n> "); } /* EOL is end of an expression */
;

prog:
PAR_OPEN prog2 PAR_CLOSE
;

prog2:
atini
;

atini:
 ATOM
;

%%

int main(int argc, char **argv) {
    printf("> ");
    yyparse();
    return 0;
}

void yyerror(char *s, ...) {
    va_list ap;
    va_start(ap, s);
    vfprintf(stderr, s, ap);
    fprintf(stderr, "\n");
}
