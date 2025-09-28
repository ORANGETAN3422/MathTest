using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MathTest.MathUtils;

public static class Simplification
{
    /// <summary>
    /// Simplifies a constant expression, accounting for brackets (+, -, *, /)
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static decimal SimplifyConstantExpression(string expression)
    {
        List<string> terms = new List<string>();
        List<char> operators = new List<char>();

        //break expression into terms and operators
        for (int i = 0; i < expression.Length;)
        {
            while (i < expression.Length && char.IsWhiteSpace(expression[i])) i++; // skip whitespaces
            if (i >= expression.Length) break;

            StringBuilder termBuilder = new StringBuilder();

            // 1 - unary minus before parentheses
            if (expression[i] == '-' && i + 1 < expression.Length && expression[i + 1] == '(')
            {
                terms.Add("-1");
                operators.Add('*');
                i++; // skip unary minus
            }

            // 2️ - parentheses
            if (expression[i] == '(')
            {
                int parenCount = 1;
                i++;
                while (i < expression.Length && parenCount > 0)
                {
                    if (expression[i] == '(') parenCount++;
                    else if (expression[i] == ')') parenCount--;

                    if (parenCount > 0)
                        termBuilder.Append(expression[i]);

                    i++;
                }
                terms.Add(EvaluateSegment(termBuilder.ToString()).ToString());

                if (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '(' || expression[i] == '-')) operators.Add('*'); // implicit mult
            }

            // 3 - constants
            else if (char.IsDigit(expression[i]) || expression[i] == '-' || expression[i] == '.')
            {
                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.' ||
                       (expression[i] == '-' && (i == 0 || "+-*/(".Contains(expression[i - 1])))))
                {
                    termBuilder.Append(expression[i]);
                    i++;
                }
                terms.Add(termBuilder.ToString());

                if (i < expression.Length && expression[i] == '(') operators.Add('*'); // implicit mult 2
            }

            // 4 - operators
            if (i < expression.Length && "+-*/".Contains(expression[i]))
            {
                operators.Add(expression[i]);
                i++;
            }
        }

        StringBuilder fullExpression = new StringBuilder();
        for (int i = 0; i < terms.Count; i++)
        {
            fullExpression.Append(terms[i]);
            if (i < operators.Count)
            {
                fullExpression.Append(operators[i]);
            }
        }

        //Debug.WriteLine("Full expression reconstructed: " + fullExpression.ToString());
        return EvaluateSegment(fullExpression.ToString());
    }

    /// <summary>
    /// simplifies a constant expression (+, -, *, /)
    /// </summary>
    /// <param name="expression">string mathematical expression</param>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException"></exception>
    private static decimal EvaluateSegment(string expression)
    {
        List<decimal> values = new List<decimal>();
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

            values.Add(decimal.Parse(constantBuilder.ToString()));

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

    private static void EvaluateOperators(List<decimal> values, List<char> operators, string validOps)
    {
        for (int i = 0; i < operators.Count; i++)
        {
            char op = operators[i];
            if (!validOps.Contains(op)) continue;

            decimal left = values[i];
            decimal right = values[i + 1];

            decimal result = op switch
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
