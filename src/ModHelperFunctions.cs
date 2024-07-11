using System;
using FarmHelper.API.Attributes;

namespace FarmHelper;

internal class ModHelperFunctions
{
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
}