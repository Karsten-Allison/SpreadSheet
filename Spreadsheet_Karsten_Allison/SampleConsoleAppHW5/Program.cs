// <copyright file="Program.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetEngine;
using CptS321;

namespace SampleConsoleAppHW5
{
    /// <summary>
    /// Console application created for demoing an expression tree.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main code of the console application.
        /// </summary>
        /// <param name="args">unused paramneter.</param>
        private static void Main(string[] args)
        {
            bool running = true;
            string expression = "A+12";
            ExpressionTree exp = new ExpressionTree(expression);

            while (running)
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine("Menu (current expression=\"" + expression + "\")");
                Console.WriteLine("  1 = Enter a new expression");
                Console.WriteLine("  2 = Set a variable value");
                Console.WriteLine("  3 = Evaluate Tree");
                Console.WriteLine("  4 = Quit");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("Enter new expression: ");
                        expression = Console.ReadLine();
                        exp = new ExpressionTree(expression);
                        break;

                    case "2":
                        // If user trys to enter a variable value without having an expression, restart.
                        if (expression == string.Empty)
                        {
                            Console.WriteLine("No expression to edit.");
                            break;
                        }

                        Console.Write("Enter variable name: ");
                        string variableArg = Console.ReadLine();
                        Console.Write("Enter variable value: ");
                        string valueArg = Console.ReadLine();

                        // If user trys to enter an invalid double value, restart.
                        if (double.TryParse(valueArg, out double valueDouble))
                        {
                            exp.SetVariable(variableArg, valueDouble);
                        }
                        else
                        {
                            Console.WriteLine("Not a valid double.");
                            break;
                        }

                        break;

                    case "3":
                        Console.Write("Expression Tree Value: ");
                        Console.WriteLine(exp.Evaluate().ToString());
                        break;

                    case "4":
                        Console.WriteLine("Done");
                        running = false;
                        System.Threading.Thread.Sleep(3000);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
