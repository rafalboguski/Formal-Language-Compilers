grammar RB;
options {
    output=AST;
}

prog
    : statements
    ;

statements
    : stat+
    ;

stat
    : declare_variable
    ;

declare_variable
    :   TYPE_INT variable_name EQUALS value_int END
    |   TYPE_FLOAT variable_name EQUALS value_float END
    ;


variable_name:  NAME;

value_int: DIGIT;
value_float: DIGIT COMMA DIGIT;


TYPE_INT:       'int';
TYPE_FLOAT:     'float';
NAME  :         ('a'..'z'|'A'..'Z')+ ;
DIGIT :         '0'..'9'+ ;
EQUALS:         '=';
COMMA:          ',';
END:            ';';
WS :            [ \t\r\n]+ -> skip ;