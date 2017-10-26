/*---------------------------------------------------------
  jaime margolin a01019332
  juan carlos leon
  rodrigo solana
-----------------------------------------------------------*/

%{

#include "mbdle_tokens.h"

%}

%%

[a-zA-Z0-9_]+  { return ATOM; }
[ \t]          { /* ignore whitespace */ }
\n             { return EOL; }
.              { return ILLEGAL; }

%%
