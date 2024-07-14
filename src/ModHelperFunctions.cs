using System;
using System.Collections.Generic;
using System.Linq;
using FarmHelper.API.Attributes;
using FarmHelper.Helpers;

namespace FarmHelper;

internal class ModHelperFunctions
{
    internal static void LoadAll()
    {
        FuncHelper.AddAll<ModHelperFunctions>();
        LocalizerHelper.Add("code_tooltip_pow", @"`pow(x, y)`
Returns the number `x` raised to the power `y`.

Examples:
`pow(2, 2)   # 4`
`pow(5, 10)  # 9765625 `
`pow(10, -3) # 0.001`

Takes the time of `1` operations to execute.");
        LocalizerHelper.Add("code_tooltip_floor", @"`floor(x)`
Returns the largest integral value less than or equal to `x`.

Examples:
`floor(1.5) # 1`
`floor(1)   # 1`
`floor(2.9) # 2`
`floor(0.9) # 0`

Takes the time of `1` operations to execute.");
        LocalizerHelper.Add("code_tooltip_round", @"`round(x)`
Returns the nearest integer from `x`.

Examples:
`round(1.5)  # 2`
`round(2.1)  # 2`

Takes the time of `1` operations to execute.");
    }
    
    [PyFunction("pow", "#33b5aa")]
    private static double Pow(Interpreter interpreter, PyNumber a, PyNumber b)
    {
        var result = Math.Pow(a, b);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("floor", "#33b5aa")]
    private static double Floor(Interpreter interpreter, PyNumber x)
    {
        var result = Math.Floor(x);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("round", "#33b5aa")]
    private static double Round(Interpreter interpreter, PyNumber x)
    {
        var result = Math.Round(x);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("addition", "#33b5aa")]
    private static double Addition(Interpreter interpreter, PyNumber a, PyNumber b, params PyNumber[] args)
    {
        var total = args.Aggregate(a + b, (current, number) => current + number);

        interpreter.State.ReturnValue = new PyNumber(total);
        return interpreter.GetOpCount(NodeType.Expr);
    }
}