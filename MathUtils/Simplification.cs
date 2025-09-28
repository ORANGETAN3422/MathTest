using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.MathUtils;

public static class Simplification
{
    /// <summary>
    /// Simplifies a constant expression, accounting for brackets (+, -, *, /)
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static double SimplifyConstantExpression(string expression)
    {
        List<double> values = new List<double>();
        List<char> operators = new List<char>();

        Debug.WriteLine(EvaluateSegment("5*-2"));
        return 0d;
    }

    /// <summary>
    /// simplifies a constant expression (+, -, *, /)
    /// </summary>
    /// <param name="expression">string mathematical expression</param>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException"></exception>
    private static double EvaluateSegment(string expression)
    {
        List<double> values = new List<double>();
        List<char> operators = new List<char>();

        for (int i = 0; i < expression.Length;)
        {
            while (i < expression.Length && char.IsWhiteSpace(expression[i])) i++; // skip whitespace
            if (i >= expression.Length) break;

            StringBuilder constantBuilder = new StringBuilder();
            if (expression[i] == '-' && (i == 0 || "+-*/".Contains(expression[i - 1]))) // unary minus
            {
                constantBuilder.Append('-');
                i++;
            }

            while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.')) // number
            {
                constantBuilder.Append(expression[i]);
                i++;
            }

            if (constantBuilder.Length == 0) throw new FormatException($"Expected number at position {i} in '{expression}'");

            values.Add(double.Parse(constantBuilder.ToString()));

            while (i < expression.Length && char.IsWhiteSpace(expression[i])) i++; // skip whitespace
            if (i >= expression.Length) break;

            if ("+-*/".Contains(expression[i])) // operator
            {
                operators.Add(expression[i]);
                i++;
            }
        }

        if (values.Count != operators.Count + 1)
            throw new InvalidOperationException($"Mismatch between values ({values.Count}) and operators ({operators.Count}) in '{expression}'");

        EvaluateOperators(values, operators, "*/");
        EvaluateOperators(values, operators, "+-");

        return values[0];
    }

    private static void EvaluateOperators(List<double> values, List<char> operators, string validOps)
    {
        for (int i = 0; i < operators.Count; i++)
        {
            char op = operators[i];
            if (!validOps.Contains(op)) continue;

            double left = values[i];
            double right = values[i + 1];

            double result = op switch
            {
                '*' => left * right,
                '/' when right == 0 => throw new DivideByZeroException(),
                '/' => left / right,
                '+' => left + right,
                '-' => left - right,
                _ => throw new InvalidOperationException($"Unknown operator {op}")
            };

            values[i] = result;
            values.RemoveAt(i + 1);
            operators.RemoveAt(i);
            i--;
        }
    }
}
