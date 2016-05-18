
class LLVMGenerator{
   
	static String header_text = "";
	static String main_text = "";
	static int str_i = 0;

	static void declare(Variable var){
		main_text += "\t%" + var.name + " = alloca " + var.type;
		main_text += "\n";
		
		if(var.value != null) {
			main_text += "\t" + "store " + var.type + " " + var.value + ", " + var.type + "* %" + var.name;
			main_text += "\n";
		}
	}
	
	static void assign(){
		
		
	}
	
	static void read(Variable var) {
		
		if(var.type == "double"){
			header_text += "@.str." + str_i + " = private unnamed_addr constant [4 x i8] c\"%lf\\00\"\n";
			main_text += "\t" + "call i32 (i8*, ...) @__isoc99_scanf(i8* getelementptr inbounds ([4 x i8], [4 x i8]* @.str." + str_i + ", i32 0, i32 0), double* %" + var.name + ")\n";
		}
		else{
			                             
			header_text += "@.str." + str_i + " = private unnamed_addr constant [3 x i8] c\"%d\\00\"\n";
			main_text += "\t" + "call i32 (i8*, ...) @__isoc99_scanf(i8* getelementptr inbounds ([3 x i8], [3 x i8]* @.str." + str_i + ", i32 0, i32 0), i32* %" + var.name + ")\n";
		}
		str_i++;
	}
	
	static void print(Variable var){
		//System.out.print("DEBUG print"+var.value);
		
		main_text += "\t" + "%print" + str_i + " = load " + var.type + ", " + var.type + "* %" + var.name + "\n";

		if(var.type == "double"){
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
		text += "define i32 @main() nounwind{\n\n";
		text += main_text;
		text += "\t" + "ret i32 0 \n}\n";
		return text;
	}

}
