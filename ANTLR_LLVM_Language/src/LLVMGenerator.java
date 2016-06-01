import java.util.*;


class LLVMGenerator{
   
	static String header_text = "";
	public static String main_text = "";
	public static String fun_text = "";
	static int str_i = 1;
	static int fun_i = 1;
	public static int label_i = 1;
	public static int for_i = 1;
	
	static void declare(Variable var, String cast){
		

		if(var.global){
			
			//@global = global i32 11
			//@global2 = common global i32 0
			if (var.value != null)
				header_text += "@"+var.name+" = global "+var.type+" " +var.value+"\n";
			else{
				String val = var.type.equals("double")? "0.0": "0";
				header_text += "@"+var.name+" = common global "+var.type+" " +val+"\n";
			}
			return;
		}
		
		
		
		main_text += "\t%var." + var.name + " = alloca " + var.type;
		main_text += "\n";
		
		
		if(var.value != null) {
			
			if(cast == "int"){
			
				main_text += "\t" + "%var."+str_i+" = fptosi double "+var.value+" to i32" + "\n";
				str_i ++;
				main_text += "\t" + "store " + var.type + " %var." + (str_i-1) + ", " + var.type + "* %var." + var.name;
				return;
			}
			
			if(cast == "double"){
				if(var.value.length()==1)
					var.value += ".0";
			
				main_text += "\t" + "%var."+str_i+" = sitofp i32 "+var.value+" to i32" + "\n";
				str_i ++;
				main_text += "\t" + "store " + var.type + " %var." + (str_i-1) + ", " + var.type + "* %var." + var.name;
				return;
			}
			
  //%1 = load double, double* %ww
  //%2 = fptosi double %1 to i32
  //store i32 %2, i32* %a
  
  //%3 = load i32, i32* %a
  //%4 = sitofp i32 %3 to double
  //store double %4, double* %ww
  
			
			main_text += "\t" + "store " + var.type + " " + var.value + ", " + var.type + "* %var." + var.name;
			main_text += "\n";
		}
	}
	
	static void assign(Variable var, Variable sourceVar, Variable value){
		
		//store i32 %2, i32* %b, align 4
		if(value == null){
			//System.out.println("DEBUG");
			
			if(!sourceVar.name.contains("r.")){
				if(sourceVar.global)
					main_text += "\t" +  "%var."+str_i+" = load "+sourceVar.type+", "+sourceVar.type+"* @"+ sourceVar.name + "\n";
				else
					main_text += "\t" +  "%var."+str_i+" = load "+sourceVar.type+", "+sourceVar.type+"* %var."+ sourceVar.name + "\n";
				main_text += "\t" + "store " + sourceVar.type + " %var."+ str_i + ", "+var.type+"* %var."+ var.name + "\n";
			}
			else{
				main_text += "\t" + "store " + sourceVar.type + " %"+ sourceVar.name + ", "+var.type+"* %var."+ var.name + "\n";
			}
		}
		else{
			main_text += "\t" + "store " + value.type + " "+ value.value + ", "+var.type+"* %var."+ var.name + "\n";
		}
		str_i ++;
	}
	
	
	private static String main_text_Copy;
	
	static void funDeclare(String type, String name, List<Variable> parmas){
		
		if(type.equals("int"))
			type = "i32";
		
		fun_text += "define " +type+ " @" +name+"(";
		
		String parDeclare = "";
		for(int i = 0; i < parmas.size();i++){
			Variable v = parmas.get(i);
			
			if(v.type.equals("int"))
				v.type = "i32";
			
			fun_text += " " + v.type+ " %p." +fun_i;
			if(i<parmas.size()-1){
				fun_text += ",";
			}
			
			parDeclare += funParamDec(v, null);
			
			fun_i++;
			
			// i deklaracje zmiennych
		}
		
		fun_text += "){\n";
		fun_text += parDeclare;
		
		main_text_Copy = main_text;
		
		main_text = "";

	}
	
	static String funParamDec(Variable var, String cast){
		String ret = "";
		ret += "\t%var." + var.name + " = alloca " + var.type;
		ret += "\n";
		
		ret += "\t" + "store " + var.type + " %p." + fun_i + ", " + var.type + "* %var." + var.name;
		ret += "\n";
		return ret;
	}
	
	static void funEnd(Variable returnVar){
		fun_text += main_text;
		main_text = main_text_Copy;
		
		fun_text += "\t%r." + str_i+ "= load "+returnVar.type+ ", "+returnVar.type+"* %var."+returnVar.name +"\n";
		fun_text += "\tret " +returnVar.type+ " %r."+str_i+"\n}";
		//%6 = load i32, i32* %C
		//ret i32 %6	
		fun_i++;	
	}
	
	
	static void funCall(Variable var, String funName, List<Variable> params){
		
		//%1 = load i32, i32* %a
		//%2 = load i32, i32* %b
		
		
		
	
		String parDeclare = "";
		for(int i = 0; i < params.size();i++){
			Variable v = params.get(i);
			main_text += "\t%var." + str_i + " = load " + v.type + ", " + v.type+ "* %var."+ v.name + "\n";
			
			parDeclare += " " + v.type+ " %var." +str_i;
			if(i<params.size()-1){
				parDeclare += ",";
			}
			
			str_i++;
		}
		//%3 = call i32 @funkcja(i32 %1, i32 %2)
		//store i32 %3, i32* %c
		main_text += "\t%var." + str_i + " = call " + var.type + " @" +funName+ "("+ parDeclare + ")\n";
		main_text += "\tstore " + var.type + " %var." + str_i + ", " + var.type +  "* %var."+ var.name + "\n";	
		
		str_i++;
	}
	

	static void forDeclare(String times){

		main_text += "\t%for.iter."+for_i+" = alloca i32 \n";
		main_text += "\tstore i32 0, i32* %for.iter." + for_i + "\n";
        main_text += "\tbr label %for."+for_i+".declare" + "\n";

		main_text += "for."+for_i+".declare:\n";                                  
		main_text += "\t%r."+ str_i +  " = load i32, i32* %for.iter." + for_i + "\n";
		main_text += "\t%r."+(str_i+1)+" = icmp slt i32 %r."+str_i+", " + times+"\n";
		main_text += "\tbr i1 %r."+(str_i+1)+", label %for."+for_i+".execute, label %for."+for_i+".continue \n";
		main_text += "for."+for_i+".execute:\n";
		
		str_i+=2;
		for_i++;
	}
	
	static void forExit(){
		main_text += "\tbr label %for."+(for_i-1)+".update\n";
		main_text += "for."+(for_i-1)+".update:\n";
		main_text += "\t%r."+ str_i +  " = load i32, i32* %for.iter." + (for_i-1) + "\n";           
		main_text += "\t%r."+ (str_i+1) +  " = add nsw i32 %r." + str_i + ", 1\n";   
        main_text += "\tstore i32 %r." + (str_i+1) + ", i32* %for.iter." + (for_i-1) + "\n";   		
		main_text += "\tbr label %for."+(for_i-1)+".declare" + "\n";  		
		main_text += "for."+(for_i-1)+".continue:\n";
		str_i+=2;     
	}
	
	static void ifDeclare(Variable var1, Variable var2,String relation){
		
		main_text += "\t%r." +  str_i    + " = load " + var1.type + ", " + var1.type + "* %var." + var1.name + "\n";
		main_text += "\t%r." + (str_i+1) + " = load " + var2.type + ", " + var2.type + "* %var." + var2.name + "\n";
		
		
		String rel = "";
		
		if(relation.equals("=="))
			rel = "eq";
		if(relation.equals("<"))
			rel = "slt";
		if(relation.equals("<="))
			rel = "sle";
		if(relation.equals(">"))
			rel = "sgt";
		if(relation.equals(">="))
			rel = "sge";
		
		if(var1.type.equals("double")){
			rel = rel.replace("s","o");
			if(relation.equals("=="))
				rel = "oeq";
		}
		
		String sick_of_this = "";
		if(var1.type.equals("double")){
			sick_of_this = "fcmp";
		}
		else{
			sick_of_this = "icmp";
		}
		
		main_text += "\t%r." + (str_i+2) + " = "+sick_of_this+" " + rel + " " + var1.type + " %r." +  str_i    + ", %r." +  (str_i+1) + "\n";
		main_text += "\tbr i1 %r."+(str_i+2)+", label %label_"+ (label_i) +", label %label_" +(label_i+1) + "\n";
		
		main_text += "label_" +label_i + ":\n";
		label_i += 2;
		
		str_i += 3;
	}
	
	static Variable math(Variable var1, Variable var2, String operation){
		
		//%1 = load i32, i32* %a, align 4
		//%2 = add nsw i32 %1, 4
		String type = null;
		
		main_text += "\t%r." + str_i     + " = load " + var1.type + ", " + var1.type + "* %var." + var1.name + "\n";
		main_text += "\t%r." + (str_i+1) + " = load " + var2.type + ", " + var2.type + "* %var." + var2.name + "\n";
		
		if(var1.type.equals("i32") && var2.type.equals("i32")){
			main_text += "\t%r." + (str_i+2) + " = "+operation+" nsw " + var1.type + " %r." + str_i + ", %r." + (str_i+1) + "\n";
			type = "i32";
		}
		if(var1.type.equals("double") && var2.type.equals("double")){
			main_text += "\t%r." + (str_i+2) + " = f"+operation+" " + var1.type + " %r." + str_i + ", %r." + (str_i+1) + "\n";
			type = "double";
		}
		
		str_i += 3;
		
		return new Variable("r." + (str_i-1), type);
	}
	
	static void read(Variable var) {
		
		if(var.type.equals("double")){
			header_text += "@.str." + str_i + " = private unnamed_addr constant [4 x i8] c\"%lf\\00\"\n";
			main_text += "\t" + "call i32 (i8*, ...) @__isoc99_scanf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @.str." + str_i + ", i32 0, i32 0), double* %var." + var.name + ")\n";
		}
		else{
			                             
			header_text += "@.str." + str_i + " = private unnamed_addr constant [3 x i8] c\"%d\\00\"\n";
			main_text += "\t" + "call i32 (i8*, ...) @__isoc99_scanf(i8* getelementptr inbounds ([3 x i8], [3 x i8]* @.str." + str_i + ", i32 0, i32 0), i32* %var." + var.name + ")\n";
		}
		str_i++;
	}
	
	static void print(Variable var){
		//System.out.print("DEBUG print"+var.value);
		
		main_text += "\t" + "%print" + str_i + " = load " + var.type + ", " + var.type + "* %var." + var.name + "\n";

		if(var.type.equals("double")){
			header_text += "@.str." + str_i + " = private unnamed_addr constant [5 x i8] c\"%lf\\0A\\00\"\n";
			main_text += "\t" + "call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([5 x i8], [5 x i8]* @.str." + str_i + ", i32 0, i32 0), " + var.type + " %print" + str_i + ")" + "\n";
		}
		else{
			header_text += "@.str." + str_i + " = private unnamed_addr constant [4 x i8] c\"%d\\0A\\00\"\n";		
			main_text += "\t" + "call i32 (i8*, ...) @printf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @.str." + str_i + ", i32 0, i32 0), " + var.type + " %print" + str_i + ")" + "\n";
		}
		
		str_i++;
	}

	static String generate(){
		String text;
		
		text =  "declare i32 @printf(i8*, ...)\n";
		text += "declare i32 @__isoc99_scanf(i8*, ...)\n";
		text += "\n";
		text += header_text;
		text += "\n\n";
		
		text += fun_text;
		text += "\n\n";
		text += "define i32 @main() nounwind{\n\n";
		text += main_text;
		text += "\t" + "ret i32 0 \n}\n";
		return text;
	}

}
