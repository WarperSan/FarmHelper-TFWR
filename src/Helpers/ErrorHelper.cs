using System;
using System.Collections.Generic;
using System.Reflection;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping with the generation of errors
/// </summary>
public static class ErrorHelper
{
    /// <summary>
    /// Generates an exception for the wrong number of arguments given
    /// </summary>
    /// <param name="functionName">Name of the function</param>
    /// <param name="expectedArguments">Types of arguments expected</param>
    /// <param name="actualArguments">Arguments given</param>
    public static Exception WrongNumberOfArguments(
        string functionName,
        ParameterInfo[] expectedArguments,
        List<IPyObject> actualArguments
    ) {
        var expectedNames = new List<string>();
        var actualNames = new string[actualArguments.Count];

        foreach (var expected in expectedArguments)
        {
            if (expected.IsOptional)
                continue;
            
            if (expected.GetCustomAttribute<ParamArrayAttribute>() != null)
                continue;
            
            expectedNames.Add(expected.ParameterType.FullName);
        }

        for (var i = 0; i < actualArguments.Count; i++)
            actualNames[i] = CodeUtilities.ToNiceString(actualArguments[i], isSequenceElement: true);
        
        return new ExecuteException(CodeUtilities.LocalizeAndFormat(
            "error_wrong_number_args_detailed", 
            functionName + "()", 
            expectedNames.Count,
            string.Join(", ", expectedNames),
            string.Join(", ", actualNames)
        ));
    }

    /// <summary>
    /// Generates an exception for the wrong type of argument given 
    /// </summary>
    /// <param name="functionName">Name of the function</param>
    /// <param name="expectedArgument">Type of the argument expected</param>
    /// <param name="actualArgument">Argument given</param>
    /// <param name="index">Index of the argument</param>
    public static Exception WrongArgumentType(string functionName, Type expectedArgument, IPyObject actualArgument, int index)
    {
        return new ExecuteException(CodeUtilities.LocalizeAndFormat(
            "error_wrong_args_detailed",
            functionName + "()",
            expectedArgument.FullName,
            index,
            CodeUtilities.ToNiceString(actualArgument, isSequenceElement: true)
        ));
    }
}