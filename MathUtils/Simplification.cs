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
        List<string> segments = new List<string>();
        List<char> operators = new List<char>();
        StringBuilder currentSegment = new StringBuilder();
        int parenthesisDepth = 0;

        Debug.WriteLine($"Original expression: {expression}");

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];

            if (c == '(')
            {
                if (parenthesisDepth == 0)
                {
                    // --- COEFFICIENT DETECTION ---
                    int coeffEnd = i - 1;
                    int coeffStart = coeffEnd;

                    // Skip spaces
                    while (coeffStart >= 0 && char.IsWhiteSpace(expression[coeffStart])) coeffStart--;

                    // Move backward through digits and decimal point
                    while (coeffStart >= 0 && (char.IsDigit(expression[coeffStart]) || expression[coeffStart] == '.'))
                        coeffStart--;

                    // Include unary minus if present
                    if (coeffStart >= 0 && expression[coeffStart] == '-')
                    {
                        if (coeffStart == 0 || "+-*/".Contains(expression[coeffStart - 1]))
                            coeffStart--;
                    }

                    coeffStart++; // move to the first digit of coefficient

                    if (coeffStart <= coeffEnd)
                    {
                        string coefficient = expression.Substring(coeffStart, coeffEnd - coeffStart + 1).Trim();
                        if (!string.IsNullOrEmpty(coefficient))
                        {
                            segments.Add(coefficient);
                            operators.Add('*');
                            Debug.WriteLine($"Detected coefficient: {coefficient}, inserted '*' operator");

                            currentSegment.Clear();
                        }
                    }
                }

                if (parenthesisDepth > 0)
                    currentSegment.Append(c);

                parenthesisDepth++;
            }
            else if (c == ')')
            {
                parenthesisDepth--;
                if (parenthesisDepth > 0)
                    currentSegment.Append(c);
                else
                {
                    if (currentSegment.Length > 0)
                    {
                        string seg = currentSegment.ToString();
                        segments.Add(seg);
                        Debug.WriteLine($"Added segment from parentheses: {seg}");
                        currentSegment.Clear();
                    }
                }
            }
            else if ("+-*/".Contains(c) && parenthesisDepth == 0)
            {
                bool isUnaryMinus = false;

                if (c == '-')
                {
                    // Look ahead to skip spaces
                    int lookahead = i + 1;
                    while (lookahead < expression.Length && char.IsWhiteSpace(expression[lookahead])) lookahead++;

                    // If next character is a digit or '(', it's a unary minus / negative coefficient
                    if (lookahead < expression.Length && (char.IsDigit(expression[lookahead]) || expression[lookahead] == '('))
                    {
                        isUnaryMinus = true;
                    }
                }

                if (!isUnaryMinus)
                {
                    if (currentSegment.Length > 0)
                    {
                        string seg = currentSegment.ToString();
                        segments.Add(seg);
                        Debug.WriteLine($"Added segment before operator '{c}': {seg}");
                        currentSegment.Clear();
                    }
                    operators.Add(c);
                    Debug.WriteLine($"Added top-level operator: {c}");
                }
                else
                {
                    // Unary minus should be kept in currentSegment
                    currentSegment.Append(c);
                }
            }
            else
            {
                currentSegment.Append(c);
            }
        }

        if (currentSegment.Length > 0)
        {
            string seg = currentSegment.ToString();
            segments.Add(seg);
            Debug.WriteLine($"Added final segment: {seg}");
        }

        segments = segments.Where(s => !string.IsNullOrEmpty(s)).ToList();

        Debug.WriteLine("All segments:");
        foreach (var seg in segments) Debug.WriteLine($" - {seg}");

        Debug.WriteLine("All operators:");
        foreach (var op in operators) Debug.WriteLine($" - {op}");

        string finalExpression = "";
        for (int j = 0; j < segments.Count; j++)
        {
            double val = EvaluateSegment(segments[j]);
            Debug.WriteLine($"Evaluated segment '{segments[j]}' = {val}");
            finalExpression += val.ToString();
            if (j < operators.Count)
                finalExpression += operators[j];
        }

        Debug.WriteLine($"Final expression before last evaluation: {finalExpression}");
        double result = EvaluateSegment(finalExpression);
        Debug.WriteLine($"Final result: {result}");

        return result;
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
        int i = 0;

        while (i < expression.Length)
        {
            while (i < expression.Length && char.IsWhiteSpace(expression[i])) i++;

            bool negative = false;
            if (i < expression.Length && expression[i] == '-' &&
                (i == 0 || "+-*/".Contains(expression[i - 1])))
            {
                negative = true;
                i++;
            }

            StringBuilder sb = new StringBuilder();
            while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
            {
                sb.Append(expression[i]);
                i++;
            }

            if (sb.Length == 0)
                throw new FormatException($"Expected number at position {i} in '{expression}'");

            double val = double.Parse(sb.ToString());
            if (negative) val = -val;
            values.Add(val);

            while (i < expression.Length && char.IsWhiteSpace(expression[i])) i++;

            if (i < expression.Length && "+-*/".Contains(expression[i]))
            {
                operators.Add(expression[i]);
                i++;
            }
        }

        if (values.Count != operators.Count + 1)
            throw new InvalidOperationException($"Mismatch between values ({values.Count}) and operators ({operators.Count}) in '{expression}'");

        for (int j = 0; j < operators.Count; j++)
        {
            if (operators[j] == '*' || operators[j] == '/')
            {
                double left = values[j];
                double right = values[j + 1];
                if (operators[j] == '/' && right == 0) throw new DivideByZeroException();
                double result = operators[j] == '*' ? left * right : left / right;
                values[j] = result;
                values.RemoveAt(j + 1);
                operators.RemoveAt(j);
                j--;
            }
        }

        for (int j = 0; j < operators.Count; j++)
        {
            double left = values[j];
            double right = values[j + 1];
            double result = operators[j] == '+' ? left + right : left - right;
            values[j] = result;
            values.RemoveAt(j + 1);
            operators.RemoveAt(j);
            j--;
        }

        return values[0];
    }
}
