
import java.util.HashMap;

public class LLVMactions extends RBBaseListener {

    HashMap<String, Variable> memory = new HashMap<String, Variable>();
    String value;
	
	

	
    @Override
    public void exitAssign(RBParser.AssignContext ctx) { 
	//System.out.println("DEBUG exitAssign");
		
		if(ctx.expr().add() != null){
			Variable var1 = null;
			Variable var2 = null;
			if(ctx.expr().add().expr1(0).variable_name()!=null){
				var1 = memory.get(ctx.expr().add().expr1(0).variable_name().getText());
			}
			
			if(ctx.expr().add().expr1(1).variable_name()!=null){
				var2 = memory.get(ctx.expr().add().expr1(1).variable_name().getText());
			}
			//System.out.println(var1);
			//System.out.println(var2);
			String resultName = LLVMGenerator.add(var1, var2);
			
			Variable res = memory.get(ctx.variable_name().getText());
			LLVMGenerator.assign(res, resultName,null);
		}
    }
	

    @Override 
    public void exitProg(RBParser.ProgContext ctx) { 
       System.out.println( LLVMGenerator.generate() );
    }


	@Override
	public void exitDeclare_variable(RBParser.Declare_variableContext ctx) {
		//System.out.print("DEBUG exitDeclare\n");
		if(ctx.declare_variable_int() != null){
			Variable var = new Variable(
				ctx.declare_variable_int().variable_name().getText(),
				"i32",
				(ctx.declare_variable_int().value_int() != null) ? ctx.declare_variable_int().value_int().getText() : null
			);
			memory.put(var.name, var);
			LLVMGenerator.declare(var);
			//System.out.print("int " + ctx.declare_variable_int().variable_name().getText() + " = "+ctx.declare_variable_int().value_int().getText()+"\n");
		}
		if(ctx.declare_variable_double() != null){
			Variable var = new Variable(
				ctx.declare_variable_double().variable_name().getText(),
				"double",
				(ctx.declare_variable_double().value_double() != null) ? ctx.declare_variable_double().value_double().getText() : null
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
	
	@Override
    public void exitRead(RBParser.ReadContext ctx) { 
		Variable var = memory.get(ctx.variable_name().getText());
		LLVMGenerator.read(var);
		//System.out.println(ctx.variable_name().getText());
    } 

}
