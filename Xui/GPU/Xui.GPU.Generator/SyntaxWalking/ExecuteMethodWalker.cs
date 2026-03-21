using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xui.GPU.Generator.TypeMapping;
using Xui.GPU.IR;

namespace Xui.GPU.Generator.SyntaxWalking;

/// <summary>
/// Walks an Execute method body and builds an IrBlock of IR statements.
/// </summary>
internal class ExecuteMethodWalker
{
    private readonly SemanticModel _model;
    private readonly ShaderTypeMapper _typeMapper;
    private readonly Dictionary<string, (IrExpression Expr, IrType Type)> _scope = new();
    private readonly IrType _outputType;
    private readonly List<IrStatement> _statements = new();

    public ExecuteMethodWalker(SemanticModel model, ShaderTypeMapper typeMapper,
        string inputName, IrType inputType,
        string bindingsName, IrType bindingsType,
        IrType outputType)
    {
        _model = model;
        _typeMapper = typeMapper;
        _outputType = outputType;

        _scope[inputName] = (new IrParameter(inputName, inputType), inputType);
        _scope[bindingsName] = (new IrParameter(bindingsName, bindingsType), bindingsType);
    }

    public IrBlock Walk(MethodDeclarationSyntax method)
    {
        if (method.Body != null)
        {
            foreach (var statement in method.Body.Statements)
                WalkStatement(statement);
        }
        else if (method.ExpressionBody != null)
        {
            // Expression-bodied method: => expr;
            var expr = WalkExpression(method.ExpressionBody.Expression);
            HandleReturnExpression(method.ExpressionBody.Expression, expr);
        }

        return new IrBlock(_statements);
    }

    private void WalkStatement(StatementSyntax statement)
    {
        switch (statement)
        {
            case LocalDeclarationStatementSyntax localDecl:
                WalkLocalDeclaration(localDecl);
                break;
            case ReturnStatementSyntax returnStmt:
                WalkReturn(returnStmt);
                break;
            case ExpressionStatementSyntax exprStmt:
                WalkExpressionStatement(exprStmt);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported statement type: {statement.GetType().Name} at {statement.GetLocation()}");
        }
    }

    private void WalkLocalDeclaration(LocalDeclarationStatementSyntax localDecl)
    {
        foreach (var variable in localDecl.Declaration.Variables)
        {
            var name = variable.Identifier.Text;
            var typeInfo = _model.GetTypeInfo(localDecl.Declaration.Type);
            var irType = _typeMapper.MapType(typeInfo.Type!);

            if (variable.Initializer != null)
            {
                var initExpr = WalkExpression(variable.Initializer.Value);
                _statements.Add(new IrVarDecl(name, irType, initExpr));
            }
            else
            {
                _statements.Add(new IrVarDecl(name, irType));
            }

            _scope[name] = (new IrParameter(name, irType), irType);
        }
    }

    private void WalkReturn(ReturnStatementSyntax returnStmt)
    {
        if (returnStmt.Expression == null)
        {
            _statements.Add(new IrReturn(null));
            return;
        }

        var expr = WalkExpression(returnStmt.Expression);
        HandleReturnExpression(returnStmt.Expression, expr);
    }

    private void HandleReturnExpression(ExpressionSyntax syntax, IrExpression expr)
    {
        // If the return expression is an object initializer, it was already desugared
        // into assignments + the expr is an IrParameter("output", ...).
        _statements.Add(new IrReturn(expr));
    }

    private void WalkExpressionStatement(ExpressionStatementSyntax exprStmt)
    {
        if (exprStmt.Expression is AssignmentExpressionSyntax assignment)
        {
            var target = WalkExpression(assignment.Left);
            var value = WalkExpression(assignment.Right);
            _statements.Add(new IrAssignment(target, value));
        }
    }

    private IrExpression WalkExpression(ExpressionSyntax expression)
    {
        switch (expression)
        {
            case IdentifierNameSyntax identifier:
                return WalkIdentifier(identifier);

            case MemberAccessExpressionSyntax memberAccess:
                return WalkMemberAccess(memberAccess);

            case ObjectCreationExpressionSyntax objectCreation:
                return WalkObjectCreation(objectCreation);

            case ImplicitObjectCreationExpressionSyntax implicitCreation:
                return WalkImplicitObjectCreation(implicitCreation);

            case BinaryExpressionSyntax binary:
                return WalkBinary(binary);

            case InvocationExpressionSyntax invocation:
                return WalkInvocation(invocation);

            case CastExpressionSyntax cast:
                return WalkExpression(cast.Expression);

            case ParenthesizedExpressionSyntax parens:
                return WalkExpression(parens.Expression);

            default:
                throw new InvalidOperationException(
                    $"Unsupported expression: {expression.GetType().Name} '{expression}' at {expression.GetLocation()}");
        }
    }

    private IrExpression WalkIdentifier(IdentifierNameSyntax identifier)
    {
        var name = identifier.Identifier.Text;
        if (_scope.TryGetValue(name, out var entry))
            return entry.Expr;

        throw new InvalidOperationException($"Unknown identifier '{name}' at {identifier.GetLocation()}");
    }

    private IrExpression WalkMemberAccess(MemberAccessExpressionSyntax memberAccess)
    {
        var memberName = memberAccess.Name.Identifier.Text;

        // Check for static constants like F32.One, F32.Zero
        var symbolInfo = _model.GetSymbolInfo(memberAccess);
        if (symbolInfo.Symbol is IFieldSymbol fieldSymbol && fieldSymbol.IsStatic)
        {
            return ResolveStaticField(fieldSymbol);
        }
        if (symbolInfo.Symbol is IPropertySymbol propSymbol && propSymbol.IsStatic)
        {
            return ResolveStaticProperty(propSymbol);
        }

        // Instance field access: input.Position, bindings.MVP, etc.
        var targetExpr = WalkExpression(memberAccess.Expression);
        var memberTypeInfo = _model.GetTypeInfo(memberAccess);
        var memberIrType = _typeMapper.MapType(memberTypeInfo.Type!);

        return new IrFieldAccess(targetExpr, memberName, memberIrType);
    }

    private IrExpression ResolveStaticField(IFieldSymbol field)
    {
        var containingType = field.ContainingType.ToDisplayString();
        return ResolveStaticMember(containingType, field.Name);
    }

    private IrExpression ResolveStaticProperty(IPropertySymbol prop)
    {
        var containingType = prop.ContainingType.ToDisplayString();
        return ResolveStaticMember(containingType, prop.Name);
    }

    private IrExpression ResolveStaticMember(string containingType, string memberName)
    {
        var f32 = new IrScalarType(ScalarKind.F32);

        if (containingType == "Xui.GPU.Shaders.Types.F32")
        {
            return memberName switch
            {
                "Zero" => new IrConstant(f32, 0.0f),
                "One" => new IrConstant(f32, 1.0f),
                _ => throw new InvalidOperationException($"Unknown F32 static member: {memberName}")
            };
        }

        throw new InvalidOperationException($"Unknown static member: {containingType}.{memberName}");
    }

    private IrExpression WalkObjectCreation(ObjectCreationExpressionSyntax creation)
    {
        var typeInfo = _model.GetTypeInfo(creation);
        var irType = _typeMapper.MapType(typeInfo.Type!);

        // Object initializer: new Varyings { Position = ..., Color = ... }
        if (creation.Initializer != null)
        {
            return DesugarObjectInitializer(irType, creation.Initializer);
        }

        // Constructor with arguments: new Float4(input.Position, F32.One)
        if (creation.ArgumentList != null)
        {
            var args = creation.ArgumentList.Arguments
                .Select(a => WalkExpression(a.Expression))
                .ToList();
            return new IrConstructor(irType, args);
        }

        return new IrConstructor(irType, new List<IrExpression>());
    }

    private IrExpression WalkImplicitObjectCreation(ImplicitObjectCreationExpressionSyntax creation)
    {
        var typeInfo = _model.GetTypeInfo(creation);
        var irType = _typeMapper.MapType(typeInfo.Type!);

        if (creation.Initializer != null)
        {
            return DesugarObjectInitializer(irType, creation.Initializer);
        }

        if (creation.ArgumentList != null)
        {
            var args = creation.ArgumentList.Arguments
                .Select(a => WalkExpression(a.Expression))
                .ToList();
            return new IrConstructor(irType, args);
        }

        return new IrConstructor(irType, new List<IrExpression>());
    }

    private IrExpression DesugarObjectInitializer(IrType irType, InitializerExpressionSyntax initializer)
    {
        // Desugar: new T { A = x, B = y } -->
        //   T output;
        //   output.A = x;
        //   output.B = y;
        //   (evaluates to output)

        const string tempName = "output";
        _statements.Add(new IrVarDecl(tempName, irType));
        var outputParam = new IrParameter(tempName, irType);
        _scope[tempName] = (outputParam, irType);

        foreach (var expr in initializer.Expressions)
        {
            if (expr is AssignmentExpressionSyntax assignment)
            {
                var fieldName = ((IdentifierNameSyntax)assignment.Left).Identifier.Text;
                var valueExpr = WalkExpression(assignment.Right);

                var fieldTypeInfo = _model.GetTypeInfo(assignment.Right);
                var fieldIrType = _typeMapper.MapType(fieldTypeInfo.Type!);

                _statements.Add(new IrAssignment(
                    new IrFieldAccess(outputParam, fieldName, fieldIrType),
                    valueExpr));
            }
        }

        return outputParam;
    }

    private IrExpression WalkBinary(BinaryExpressionSyntax binary)
    {
        var left = WalkExpression(binary.Left);
        var right = WalkExpression(binary.Right);

        var resultTypeInfo = _model.GetTypeInfo(binary);
        var resultIrType = _typeMapper.MapType(resultTypeInfo.Type!);

        var op = binary.Kind() switch
        {
            SyntaxKind.MultiplyExpression => BinaryOperator.Multiply,
            SyntaxKind.AddExpression => BinaryOperator.Add,
            SyntaxKind.SubtractExpression => BinaryOperator.Subtract,
            SyntaxKind.DivideExpression => BinaryOperator.Divide,
            SyntaxKind.ModuloExpression => BinaryOperator.Modulo,
            SyntaxKind.EqualsExpression => BinaryOperator.Equal,
            SyntaxKind.NotEqualsExpression => BinaryOperator.NotEqual,
            SyntaxKind.LessThanExpression => BinaryOperator.LessThan,
            SyntaxKind.LessThanOrEqualExpression => BinaryOperator.LessThanOrEqual,
            SyntaxKind.GreaterThanExpression => BinaryOperator.GreaterThan,
            SyntaxKind.GreaterThanOrEqualExpression => BinaryOperator.GreaterThanOrEqual,
            SyntaxKind.LogicalAndExpression => BinaryOperator.LogicalAnd,
            SyntaxKind.LogicalOrExpression => BinaryOperator.LogicalOr,
            _ => throw new InvalidOperationException(
                $"Unsupported binary operator: {binary.Kind()} at {binary.GetLocation()}")
        };

        return new IrBinaryOp(op, left, right, resultIrType);
    }

    private IrExpression WalkInvocation(InvocationExpressionSyntax invocation)
    {
        // Handle shader intrinsics: Shader.Clamp(...), etc.
        var symbolInfo = _model.GetSymbolInfo(invocation);
        if (symbolInfo.Symbol is IMethodSymbol method)
        {
            var args = invocation.ArgumentList.Arguments
                .Select(a => WalkExpression(a.Expression))
                .ToList();

            var resultTypeInfo = _model.GetTypeInfo(invocation);
            var resultIrType = _typeMapper.MapType(resultTypeInfo.Type!);

            return new IrMethodCall(method.Name, args, resultIrType);
        }

        throw new InvalidOperationException(
            $"Unsupported invocation: '{invocation}' at {invocation.GetLocation()}");
    }
}
