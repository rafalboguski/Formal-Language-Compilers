grammar RB;
options {
    output=AST;
}

prog
	: function* statements
    ;

function: 
	fundef funbody
;

fundef : (TYPE_INT|TYPE_double) variable_name '(' (variable)* ')'  #fdef
	;
	
funbody: '{' statements  'return' variable_name ';' '}' 		#fbody
;

variable: (TYPE_INT|TYPE_double) variable_name ;
	
statements
    : stat*
    ;

stat
    : declare_variable 
	| print_statement
	| read_statement
	| assign_statement
	| if_statement
	| for_statement
	| fun_call_statement
    ;
	
fun_call_statement: variable_name '=' variable_name '(' (variable_names) ')' ';' #fcall
	;

variable_names: (variable_name)*;
	
for_statement: for_declare for_body;
	
for_declare: 'for' '(' DIGIT ')'	                            #fordeclare
		;
for_body : '{' statements '}'                                   #forbody
	   ;


if_statement: if_declare if_body                           
			;
if_declare: 'if' '(' variable_name relation variable_name ')' #ifdeclare
		  ;
if_body: '{' statements '}'                                   #ifbody
	   ;
relation: ('=='|'<'|'<='|'>'|'>=');

declare_variable
    :   declare_variable_int
    |   declare_variable_double
	|	declare_global_int
	|	declare_global_double
    ;
declare_global_int:
	 	'global' TYPE_INT variable_name END
	| 	'global' TYPE_INT variable_name EQUALS value_int END
;	

declare_global_double:
	 	'global' TYPE_double variable_name END
	| 	'global' TYPE_double variable_name EQUALS value_double END
;	

declare_variable_int
	: 	TYPE_INT variable_name END
	| 	TYPE_INT variable_name EQUALS (CAST_INT|CAST_DOUBLE)? (value_int|value_double) END
	;
declare_variable_double
	:	TYPE_double variable_name END
	|	TYPE_double variable_name EQUALS (CAST_INT|CAST_DOUBLE)? (value_int|value_double) END
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