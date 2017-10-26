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
%token ATOM ILLEGAL EOL

%%

mbdle:
    /* nothing */ { }                              /* Matches at beginning of input */
    | mbdle ATOM EOL { printf("syntax ok\n> "); }  /* EOL is end of an expression */
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
