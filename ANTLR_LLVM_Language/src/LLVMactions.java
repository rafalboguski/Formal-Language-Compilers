
import java.util.HashMap;

public class LLVMactions extends RBBaseListener {

    HashMap<String, Variable> memory = new HashMap<String, Variable>();
    String value;
	
	

	/*
    @Override
    public void exitAssign(RBParser.AssignContext ctx) { 
       String tmp = ctx.STRING().getText(); 
       tmp = tmp.substring(1, tmp.length()-1);
       memory.put(ctx.ID().getText(), tmp);    
    }
	*/

    @Override 
    public void exitProg(RBParser.ProgContext ctx) { 
       System.out.println( LLVMGenerator.generate() );
    }

	/*
    @Override 
    public void exitValue(RBParser.ValueContext ctx) {
       if( ctx.ID() != null ){
          value = memory.get(ctx.ID().getText());
       } 
       if( ctx.STRING() != null ){
          String tmp = ctx.STRING().getText(); 
          value = tmp.substring(1, tmp.length()-1);
       } 
    }
	*/
	@Override
	public void exitDeclare_variable(RBParser.Declare_variableContext ctx) {
		//System.out.print("DEBUG ");
		if(ctx.declare_variable_int() != null){
			Variable var = new Variable(
				ctx.declare_variable_int().variable_name().getText(),
				"i32",
				ctx.declare_variable_int().value_int().getText()
			);
			memory.put(var.name, var);
			LLVMGenerator.declare(var);
			//System.out.print("int " + ctx.declare_variable_int().variable_name().getText() + " = "+ctx.declare_variable_int().value_int().getText()+"\n");
		}
		if(ctx.declare_variable_double() != null){
			Variable var = new Variable(
				ctx.declare_variable_double().variable_name().getText(),
				"double",
				ctx.declare_variable_double().value_double().getText()
			);
			memory.put(var.name, var);
			LLVMGenerator.declare(var);
			//System.out.print("int " + ctx.declare_variable_double().variable_name().getText() + " = "+ctx.declare_variable_double().value_double().getText()+"\n");
		}
		
		
	}

    @Override
    public void exitPrint(RBParser.PrintContext ctx) { 
		
		Variable var = memory.get(ctx.variable_name().getText());
		LLVMGenerator.print(var);
		//System.out.println(ctx.variable_name().getText());
		
		
    } 

}
