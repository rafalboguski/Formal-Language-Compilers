
import java.util.HashMap;

public class LLVMactions extends RBBaseListener {

    HashMap<String, Variable> memory = new HashMap<String, Variable>();
    String value;
	
	
	@Override
	public void exitIfdeclare(RBParser.IfdeclareContext ctx){
		LLVMGenerator.ifDeclare(memory.get(ctx.variable_name(0).getText()),memory.get(ctx.variable_name(1).getText()), ctx.relation().getText());		
	}

	
	@Override
	public void exitIfbody(RBParser.IfbodyContext ctx){
		LLVMGenerator.main_text += "\tbr label %label_" +(LLVMGenerator.label_i-1)+ "\n";
		LLVMGenerator.main_text +=   "label_" +(LLVMGenerator.label_i-1)+ ":\n";
	}
	
	@Override
	public void exitFordeclare(RBParser.FordeclareContext ctx){
		LLVMGenerator.forDeclare(ctx.DIGIT().getText());		
	}
	
	@Override
	public void exitForbody(RBParser.ForbodyContext ctx){
		LLVMGenerator.forExit();
		
		//LLVMGenerator.main_text += "\tbr label %label_" +(LLVMGenerator.label_i-1)+ "\n";
		//LLVMGenerator.main_text +=   "label_" +(LLVMGenerator.label_i-1)+ ":\n";
	}
	
    @Override
    public void exitAssign(RBParser.AssignContext ctx) { 
	//System.out.println("DEBUG exitAssign");
		
		
		Variable var1 = null;
		Variable var2 = null;
		Variable resultName = null;
		Variable value = null;
		
		if(ctx.expr().add() != null){
			
			if(ctx.expr().add().expr1(0).variable_name()!=null){
				var1 = memory.get(ctx.expr().add().expr1(0).variable_name().getText());
			}
			if(ctx.expr().add().expr1(1).variable_name()!=null){
				var2 = memory.get(ctx.expr().add().expr1(1).variable_name().getText());
			}
			resultName = LLVMGenerator.math(var1, var2,"add");
		}
		
		if(ctx.expr().mul() != null){
			
			if(ctx.expr().mul().expr1(0).variable_name()!=null){
				var1 = memory.get(ctx.expr().mul().expr1(0).variable_name().getText());
			}
			if(ctx.expr().mul().expr1(1).variable_name()!=null){
				var2 = memory.get(ctx.expr().mul().expr1(1).variable_name().getText());
			}
			resultName = LLVMGenerator.math(var1, var2,"mul");
		}	

		if(ctx.expr().sub() != null){
			
			if(ctx.expr().sub().expr1(0).variable_name()!=null){
				var1 = memory.get(ctx.expr().sub().expr1(0).variable_name().getText());
			}
			if(ctx.expr().sub().expr1(1).variable_name()!=null){
				var2 = memory.get(ctx.expr().sub().expr1(1).variable_name().getText());
			}
			resultName = LLVMGenerator.math(var1, var2,"sub");
		}	

		if(ctx.expr().div() != null){
			
			if(ctx.expr().div().expr1(0).variable_name()!=null){
				var1 = memory.get(ctx.expr().div().expr1(0).variable_name().getText());
			}
			if(ctx.expr().div().expr1(1).variable_name()!=null){
				var2 = memory.get(ctx.expr().div().expr1(1).variable_name().getText());
			}
			resultName = LLVMGenerator.math(var1, var2,"div");
		}	
		//System.out.println(var1);
		//System.out.println(var2);
		
		if(ctx.expr().variable_name()!= null){
			resultName = memory.get(ctx.expr().variable_name().getText());
		}
		
		if(ctx.expr().value_double()!= null){
			value = new Variable (null,"double",ctx.expr().value_double().getText());
		}
		
		if(ctx.expr().value_int()!= null){
			value = new Variable (null,"i32",ctx.expr().value_int().getText());
		}
		
		Variable res = memory.get(ctx.variable_name().getText());
		LLVMGenerator.assign(res, resultName,value);
	
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
			if(ctx.declare_variable_int().value_double() != null)
				var.value = ctx.declare_variable_int().value_double().getText();
			memory.put(var.name, var);
			String cast = null;
			if(ctx.declare_variable_int().CAST_INT() != null)
				cast = "int";
			LLVMGenerator.declare(var,cast);
			//System.out.print("int " + ctx.declare_variable_int().variable_name().getText() + " = "+ctx.declare_variable_int().value_int().getText()+"\n");
		}
		if(ctx.declare_variable_double() != null){
			Variable var = new Variable(
				ctx.declare_variable_double().variable_name().getText(),
				"double",
				(ctx.declare_variable_double().value_double() != null) ? ctx.declare_variable_double().value_double().getText() : null
			);
			if(ctx.declare_variable_double().value_int() != null)
				var.value = ctx.declare_variable_double().value_int().getText();
			memory.put(var.name, var);
			String cast = null;
			if(ctx.declare_variable_double().CAST_INT() != null)
				cast = "double";
			LLVMGenerator.declare(var, cast);
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
