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
	| print_statement
	| read_statement
	| assign_statement
    ;

declare_variable
    :   declare_variable_int
    |   declare_variable_double
    ;
	
declare_variable_int
	: 	TYPE_INT variable_name END
	| 	TYPE_INT variable_name EQUALS value_int END
	;
declare_variable_double
	:	TYPE_double variable_name END
	|	TYPE_double variable_name EQUALS value_double END
	;
	
assign_statement
	: variable_name '=' (CAST_INT|CAST_DOUBLE)? expr END	#assign
	;

CAST_INT:'(int)';
CAST_DOUBLE:'(double)';	
	
	
expr  
	: add
	| mul
	| sub
	| div
	| value_double
	| value_int
	| variable_name
	;
	 
add : expr1 '+' expr1 
	;
	
mul : expr1 '*' expr1 
	; 
sub : expr1 '-' expr1 
	; 
	
div : expr1 '/' expr1 
	; 
	 
expr1 : variable_name
	| value_double
	| value_int
	;
	
print_statement
	: 'print(' variable_name ')' END #print
	;

read_statement
	: 'read(' variable_name ')' END #read
	;
	
	
variable_name:  NAME;

value_int: DIGIT| '-' DIGIT;
value_double: DIGIT '.' DIGIT | '-' + DIGIT '.' DIGIT;


TYPE_INT:       'int';
TYPE_double:     'double';
NAME  :         ('a'..'z'|'A'..'Z')+ ;
DIGIT :         '0'..'9'+ ;
EQUALS:         '=';
COMMA:          ',';
END:            ';';
WS :            [ \t\r\n]+ -> skip ;