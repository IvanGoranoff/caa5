
using System;
using System.Collections.Generic;

namespace LogicalExpressions
{
    class Program
    {
        static void Main()
        {
            // Create a dictionary to store the logical functions
            Dictionary<string, string> functions = new Dictionary<string, string>();

            // Read the expression from the console
            Console.WriteLine("Please enter the logical expression:");
            string expression = Console.ReadLine();

            // Check if the expression contains syntax errors
            if (!CheckSyntax(expression))
            {
                Console.WriteLine("Error: Invalid syntax.");
                return;
            }

            // Parse the expression
            string[] tokens = expression.Split(' ');

            // Check if the first argument is a valid command
            if (tokens[0].ToLower() == "define")
            {
                // Parse the function definition
                string functionName = tokens[1];
                string functionExpression = tokens[2];

                // Check if the function definition contains syntax errors
                if (!CheckSyntax(functionExpression))
                {
                    Console.WriteLine("Error: Invalid syntax.");
                    return;
                }

                // Check if the function name is already in use
                if (functions.ContainsKey(functionName))
                {
                    Console.WriteLine("Error: Function '{0}' is already defined.", functionName);
                    return;
                }

                // Add the function to the dictionary
                functions.Add(functionName, functionExpression);
            }
            else if (tokens[0].ToLower() == "solve")
            {
                // Parse the function name and arguments
                string functionName = tokens[1];
                List<string> arguments = new List<string>();
                for (int i = 2; i < tokens.Length; i++)
                {
                    arguments.Add(tokens[i]);
                }

                // Check if the function is defined
                if (functions.ContainsKey(functionName))
                {
                    // Get the function expression
                    string functionExpression = functions[functionName];

                    // Evaluate the function
                    int result = EvaluateFunction(functionExpression, arguments, functions);

                    // Print the result
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine("Error: Function '{0}' is not defined.", functionName);
                }
            }
        }

        // Checks for invalid syntax
        static bool CheckSyntax(string expression)
        {
            // Check for invalid characters
            if (expression.Contains("&") && expression.Contains("|") && expression.Contains("!"))
            {
                return false;
            }

            // Check for unbalanced parentheses
            int openParenCount = 0;
            int closeParenCount = 0;
            foreach (char c in expression)
            {
                if (c == '(')
                {
                    openParenCount++;
                }
                else if (c == ')')
                {
                    closeParenCount++;
                }
            }
            if (openParenCount != closeParenCount)
            {
                return false;
            }
            return true;
        }

        // Evaluates a logical expression
        static int EvaluateFunction(string expression, List<string> arguments, Dictionary<string, string> functions)
        {
            // Replace the function arguments with their values
            for (int i = 0; i < arguments.Count; i++)
            {
                expression = expression.Replace("arg" + i, arguments[i]);
            }

            // Replace the user defined functions with their expression
            foreach (KeyValuePair<string, string> kvp in functions)
            {
                expression = expression.Replace(kvp.Key, kvp.Value);
            }

            // Evaluate the expression
            int result = EvaluateExpression(expression);
            return result;
        }

        // Evaluates a logical expression
        static int EvaluateExpression(string expression)
        {
            // Check for the NOT operator
            if (expression.Contains("!"))
            {
                // Find the index of the NOT operator
                int index = expression.IndexOf("!");

                // Get the operand
                string operand = expression.Substring(index + 1);

                // Evaluate the operand
                int operandValue = EvaluateExpression(operand);

                // Negate the operand
                int result = (operandValue == 0) ? 1 : 0;
                return result;
            }

            // Check for the AND operator
            else if (expression.Contains("&"))
            {
                // Find the index of the AND operator
                int index = expression.IndexOf("&");

                // Get the left and right operands
                string leftOperand = expression.Substring(0, index);
                string rightOperand = expression.Substring(index + 1);

                // Evaluate the operands
                int leftOperandValue = EvaluateExpression(leftOperand);
                int rightOperandValue = EvaluateExpression(rightOperand);

                // Perform the AND operation
                int result = (leftOperandValue == 1 && rightOperandValue == 1) ? 1 : 0;
                return result;
            }

            // Check for the OR operator
            else if (expression.Contains("|"))
            {
                // Find the index of the OR operator
                int index = expression.IndexOf("|");

                // Get the left and right operands
                string leftOperand = expression.Substring(0, index);
                string rightOperand = expression.Substring(index + 1);

                // Evaluate the operands
                int leftOperandValue = EvaluateExpression(leftOperand);
                int rightOperandValue = EvaluateExpression(rightOperand);

                // Perform the OR operation
                int result = (leftOperandValue == 1 || rightOperandValue == 1) ? 1 : 0;
                return result;
            }

            // Otherwise, the expression is a single operand
            else
            {
                // Convert the operand to an integer
                int result = Int32.Parse(expression);
                return result;
            }
        }
    }
}
