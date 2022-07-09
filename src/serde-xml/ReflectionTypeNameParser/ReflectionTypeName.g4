grammar ReflectionTypeName;

topTypeName : fullType EOF ;
fullType : typeName generics? ;
typeName : qualifiedName BRACKETS* ;
qualifiedName : ( IDENT '.' )* IDENT ;
generics : '`' NUM* '[' fullType ']' ;

BRACKETS : '[' ']' ;
IDENT : [_a-zA-Z]+ NUM* ;
NUM : [0-9] ;