using ModHelper.API;
using UnityEngine;

namespace ModHelper;

internal class ModHelperFunctions
{
    [PyFunction("pow")]
    private static double Pow(Interpreter interpreter, PyNumber a, PyNumber b)
    {
        var result = Mathf.Pow((float)a, (float)b);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("floor")]
    private static double Floor(Interpreter interpreter, PyNumber x)
    {
        var result = Mathf.FloorToInt((float)x);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }

    [PyFunction("round")]
    private static double Round(Interpreter interpreter, PyNumber x)
    {
        var result = Mathf.Round((float)x);
        interpreter.State.ReturnValue = new PyNumber(result);
        return interpreter.GetOpCount(NodeType.Expr);
    }
}