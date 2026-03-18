using System.Text;
using Xui.GPU.IR;

namespace Xui.GPU.Backends.Hlsl;

/// <summary>
/// HLSL code generator for DirectX shaders.
/// Translates IR to HLSL shader code.
/// </summary>
public class HlslCodeGenerator : IShaderBackend
{
    public string Name => "HLSL";
    
    private readonly StringBuilder _output = new();
    private int _indentLevel = 0;
    private const string IndentString = "    ";

    /// <summary>
    /// Generates HLSL shader code from an IR module.
    /// </summary>
    public string GenerateCode(IrShaderModule module)
    {
        _output.Clear();
        _indentLevel = 0;

        // Header comment
        WriteLine("// Generated HLSL shader code");
        WriteLine($"// Module: {module.Name}");
        WriteLine();

        // Generate struct declarations
        foreach (var structDecl in module.Structs)
        {
            GenerateStructDecl(structDecl);
            WriteLine();
        }

        // Generate vertex shader if present
        if (module.VertexStage != null)
        {
            GenerateVertexStage(module.VertexStage);
            WriteLine();
        }

        // Generate fragment (pixel) shader if present
        if (module.FragmentStage != null)
        {
            GenerateFragmentStage(module.FragmentStage);
            WriteLine();
        }

        return _output.ToString();
    }

    private void GenerateStructDecl(IrStructDecl structDecl)
    {
        var structType = structDecl.Type;
        WriteLine($"struct {structType.Name}");
        WriteLine("{");
        _indentLevel++;

        foreach (var field in structType.Fields)
        {
            var typeStr = GetHlslType(field.Type);
            var semantic = GetSemanticString(field.Decorations);
            WriteLine($"{typeStr} {field.Name}{semantic};");
        }

        _indentLevel--;
        WriteLine("};");
    }

    private void GenerateVertexStage(IrVertexStage stage)
    {
        WriteLine($"// Vertex Shader: {stage.Name}");
        var outputType = GetHlslType(stage.OutputType);
        var inputType = GetHlslType(stage.InputType);
        
        WriteLine($"{outputType} VSMain({inputType} input)");
        WriteLine("{");
        _indentLevel++;

        // Generate body statements
        GenerateBlock(stage.Body);

        _indentLevel--;
        WriteLine("}");
    }

    private void GenerateFragmentStage(IrFragmentStage stage)
    {
        WriteLine($"// Pixel Shader: {stage.Name}");
        var outputType = GetHlslType(stage.OutputType);
        var inputType = GetHlslType(stage.InputType);
        
        WriteLine($"{outputType} PSMain({inputType} input)");
        WriteLine("{");
        _indentLevel++;

        // Generate body statements
        GenerateBlock(stage.Body);

        _indentLevel--;
        WriteLine("}");
    }

    private void GenerateBlock(IrBlock block)
    {
        foreach (var statement in block.Statements)
        {
            GenerateStatement(statement);
        }
    }

    private void GenerateStatement(IrStatement statement)
    {
        switch (statement)
        {
            case IrVarDecl varDecl:
                GenerateVarDecl(varDecl);
                break;
            case IrAssignment assignment:
                GenerateAssignment(assignment);
                break;
            case IrReturn returnStmt:
                GenerateReturn(returnStmt);
                break;
            case IrIf ifStmt:
                GenerateIf(ifStmt);
                break;
            case IrBlock block:
                GenerateBlock(block);
                break;
            default:
                WriteLine($"// TODO: Unsupported statement type {statement.GetType().Name}");
                break;
        }
    }

    private void GenerateVarDecl(IrVarDecl varDecl)
    {
        var typeStr = GetHlslType(varDecl.Type);
        if (varDecl.Initializer != null)
        {
            var initExpr = GenerateExpression(varDecl.Initializer);
            WriteLine($"{typeStr} {varDecl.Name} = {initExpr};");
        }
        else
        {
            WriteLine($"{typeStr} {varDecl.Name};");
        }
    }

    private void GenerateAssignment(IrAssignment assignment)
    {
        var targetExpr = GenerateExpression(assignment.Target);
        var valueExpr = GenerateExpression(assignment.Value);
        WriteLine($"{targetExpr} = {valueExpr};");
    }

    private void GenerateReturn(IrReturn returnStmt)
    {
        if (returnStmt.Value != null)
        {
            var valueExpr = GenerateExpression(returnStmt.Value);
            WriteLine($"return {valueExpr};");
        }
        else
        {
            WriteLine("return;");
        }
    }

    private void GenerateIf(IrIf ifStmt)
    {
        var condExpr = GenerateExpression(ifStmt.Condition);
        WriteLine($"if ({condExpr})");
        WriteLine("{");
        _indentLevel++;
        GenerateBlock(ifStmt.ThenBlock);
        _indentLevel--;
        
        if (ifStmt.ElseBlock != null)
        {
            WriteLine("}");
            WriteLine("else");
            WriteLine("{");
            _indentLevel++;
            GenerateBlock(ifStmt.ElseBlock);
            _indentLevel--;
        }
        
        WriteLine("}");
    }

    private string GenerateExpression(IrExpression expression)
    {
        return expression switch
        {
            IrConstant constant => GenerateConstant(constant),
            IrParameter parameter => parameter.Name,
            IrFieldAccess fieldAccess => $"{GenerateExpression(fieldAccess.Object)}.{fieldAccess.FieldName}",
            IrBinaryOp binaryOp => GenerateBinaryOp(binaryOp),
            IrUnaryOp unaryOp => GenerateUnaryOp(unaryOp),
            IrMethodCall methodCall => GenerateMethodCall(methodCall),
            IrConstructor constructor => GenerateConstructor(constructor),
            _ => $"/* TODO: {expression.GetType().Name} */"
        };
    }

    private string GenerateConstant(IrConstant constant)
    {
        return constant.Value switch
        {
            float f => f.ToString("G9") + "f",
            double d => d.ToString("G17"),
            int i => i.ToString(),
            uint u => u.ToString() + "u",
            bool b => b ? "true" : "false",
            _ => constant.Value.ToString() ?? "0"
        };
    }

    private string GenerateBinaryOp(IrBinaryOp binaryOp)
    {
        var left = GenerateExpression(binaryOp.Left);
        var right = GenerateExpression(binaryOp.Right);
        var op = binaryOp.Operator switch
        {
            BinaryOperator.Add => "+",
            BinaryOperator.Subtract => "-",
            BinaryOperator.Multiply => "*",
            BinaryOperator.Divide => "/",
            BinaryOperator.Modulo => "%",
            BinaryOperator.Equal => "==",
            BinaryOperator.NotEqual => "!=",
            BinaryOperator.LessThan => "<",
            BinaryOperator.LessThanOrEqual => "<=",
            BinaryOperator.GreaterThan => ">",
            BinaryOperator.GreaterThanOrEqual => ">=",
            BinaryOperator.LogicalAnd => "&&",
            BinaryOperator.LogicalOr => "||",
            BinaryOperator.BitwiseAnd => "&",
            BinaryOperator.BitwiseOr => "|",
            BinaryOperator.BitwiseXor => "^",
            BinaryOperator.ShiftLeft => "<<",
            BinaryOperator.ShiftRight => ">>",
            _ => "?"
        };
        return $"({left} {op} {right})";
    }

    private string GenerateUnaryOp(IrUnaryOp unaryOp)
    {
        var operand = GenerateExpression(unaryOp.Operand);
        var op = unaryOp.Operator switch
        {
            UnaryOperator.Negate => "-",
            UnaryOperator.LogicalNot => "!",
            UnaryOperator.BitwiseNot => "~",
            _ => "?"
        };
        return $"({op}{operand})";
    }

    private string GenerateMethodCall(IrMethodCall methodCall)
    {
        var args = string.Join(", ", methodCall.Arguments.Select(GenerateExpression));
        
        // Map shader intrinsics to HLSL built-ins
        var hlslMethod = MapIntrinsicToHlsl(methodCall.MethodName);
        
        if (methodCall.Target != null)
        {
            var target = GenerateExpression(methodCall.Target);
            return $"{target}.{hlslMethod}({args})";
        }
        
        return $"{hlslMethod}({args})";
    }

    private string GenerateConstructor(IrConstructor constructor)
    {
        var typeStr = GetHlslType(constructor.Type);
        var args = string.Join(", ", constructor.Arguments.Select(GenerateExpression));
        return $"{typeStr}({args})";
    }

    private string GetHlslType(IrType type)
    {
        return type switch
        {
            IrScalarType scalar => scalar.ScalarKind switch
            {
                ScalarKind.F32 => "float",
                ScalarKind.I32 => "int",
                ScalarKind.U32 => "uint",
                ScalarKind.Bool => "bool",
                _ => "unknown"
            },
            IrVectorType vector => GetHlslVectorType(vector),
            IrMatrixType matrix => GetHlslMatrixType(matrix),
            IrStructType structType => structType.Name,
            IrTextureType texture => $"Texture2D<{GetHlslType(texture.PixelType)}>",
            IrSamplerType => "SamplerState",
            _ => "unknown"
        };
    }

    private string GetHlslVectorType(IrVectorType vector)
    {
        var baseType = vector.ElementType.ScalarKind switch
        {
            ScalarKind.F32 => "float",
            ScalarKind.I32 => "int",
            ScalarKind.U32 => "uint",
            ScalarKind.Bool => "bool",
            _ => "unknown"
        };
        return $"{baseType}{vector.Dimension}";
    }

    private string GetHlslMatrixType(IrMatrixType matrix)
    {
        var baseType = matrix.ElementType.ScalarKind switch
        {
            ScalarKind.F32 => "float",
            _ => "unknown"
        };
        return $"{baseType}{matrix.Rows}x{matrix.Columns}";
    }

    private string GetSemanticString(List<IrDecoration> decorations)
    {
        foreach (var decoration in decorations)
        {
            if (decoration is IrBuiltInDecoration builtIn)
            {
                return builtIn.Semantic switch
                {
                    BuiltInSemantic.Position => " : SV_POSITION",
                    BuiltInSemantic.FragDepth => " : SV_DEPTH",
                    BuiltInSemantic.InstanceId => " : SV_InstanceID",
                    BuiltInSemantic.VertexId => " : SV_VertexID",
                    _ => ""
                };
            }
            else if (decoration is IrLocationDecoration location)
            {
                return $" : TEXCOORD{location.Location}";
            }
        }
        return "";
    }

    private string MapIntrinsicToHlsl(string methodName)
    {
        // Most shader intrinsics map directly to HLSL
        return methodName switch
        {
            "Clamp" => "clamp",
            "Saturate" => "saturate",
            "Lerp" => "lerp",
            "Min" => "min",
            "Max" => "max",
            "Abs" => "abs",
            "Sin" => "sin",
            "Cos" => "cos",
            "Tan" => "tan",
            "Asin" => "asin",
            "Acos" => "acos",
            "Atan" => "atan",
            "Atan2" => "atan2",
            "Pow" => "pow",
            "Exp" => "exp",
            "Log" => "log",
            "Log2" => "log2",
            "Sqrt" => "sqrt",
            "Rsqrt" => "rsqrt",
            "Floor" => "floor",
            "Ceil" => "ceil",
            "Fract" => "frac",  // Note: HLSL uses 'frac' not 'fract'
            "Round" => "round",
            "Dot" => "dot",
            "Length" => "length",
            "Distance" => "distance",
            "Normalize" => "normalize",
            "Cross" => "cross",
            "Ddx" => "ddx",
            "Ddy" => "ddy",
            "Fwidth" => "fwidth",
            "Sample" => "Sample",
            "Load" => "Load",
            _ => methodName  // Pass through unknown methods
        };
    }

    private void WriteLine(string? line = null)
    {
        if (line != null)
        {
            _output.Append(new string(' ', _indentLevel * IndentString.Length));
            _output.AppendLine(line);
        }
        else
        {
            _output.AppendLine();
        }
    }
}
