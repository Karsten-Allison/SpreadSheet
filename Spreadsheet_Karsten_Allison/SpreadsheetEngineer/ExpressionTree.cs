// <copyright file="ExpressionTree.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// ExpressionTree class contains a tree of operators, constant values, and variable values.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// the root of our node.
        /// </summary>
        private readonly Node root;

        /// <summary>
        /// Creates a new instance of our OperatorNodeFactory.
        /// </summary>
        private readonly OperatorNodeFactory factory = new OperatorNodeFactory();

        /// <summary>
        /// A dictionary for our ExpressionTree, keying double values to the variables we assign them too.
        /// </summary>
        private Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Constructor for the ExpressionTree, taking an expression as an argument.
        /// </summary>
        /// <param name="expression">a string containg an expression.</param>
        public ExpressionTree(string expression)
        {
            this.variables = new Dictionary<string, double>();
            this.root = this.Compile(expression);
        }

        /// <summary>
        /// Overloaded Evaluate function which just calls the other one.
        /// </summary>
        /// <returns>double.</returns>
        public double Evaluate()
        {
            return this.root.Evaluate();
        }

        /// <summary>
        /// adds a given variable (string) into the local dictionary with a user given double value.
        /// </summary>
        /// <param name="name">variable name.</param>
        /// <param name="value">double value.</param>
        public void SetVariable(string name, double value)
        {
            this.variables[name] = value;
        }

        /// <summary>
        /// Gets the keys from our variale dictionary.
        /// </summary>
        /// <returns>array of keys.</returns>
        public string[] GetKeys()
        {
            return this.variables.Keys.ToArray();
        }

        /// <summary>
        /// Does the Shunting Yard Algorithmn on an expression.
        /// See http://math.oxford.emory.edu/site/cs171/shuntingYardAlgorithm/ .
        /// </summary>
        /// <param name="expression">a string containing our expression.</param>
        /// <returns>Postfix expression (List of strings).</returns>
        private List<string> ShuntingYard(string expression)
        {
            List<string> postFixExpression = new List<string>();
            Stack<char> operators = new Stack<char>();
            int operandStart = -1;
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (this.IsOperatorOrParenthesis(c))
                {
                    if (operandStart != -1)
                    {
                        string operand = expression.Substring(operandStart, i - operandStart);
                        postFixExpression.Add(operand);
                        operandStart = -1;
                    }

                    if (this.IsLeftParenthesis(c))
                    {
                        operators.Push(c);
                    }
                    else if (this.IsRightParenthesis(c))
                    {
                        char op = operators.Pop();
                        while (!this.IsLeftParenthesis(op))
                        {
                            postFixExpression.Add(op.ToString());
                            op = operators.Pop();
                        }
                    }
                    else if (this.factory.IsAOperator(c))
                    {
                        if (operators.Count == 0 || this.IsLeftParenthesis(operators.Peek()))
                        {
                            operators.Push(c);
                        }
                        else if (this.IsLowerPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsRightAssociative(c)))
                        {
                            operators.Push(c);
                        }
                        else if (this.IsHigherPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsLeftAssociative(c)))
                        {
                            do
                            {
                                char op = operators.Pop();
                                postFixExpression.Add(op.ToString());
                            }
                            while (operators.Count > 0 && (this.IsLowerPrecedence(c, operators.Peek()) || (this.IsSamePrecedence(c, operators.Peek()) && this.IsLeftAssociative(c))));

                            operators.Push(c);
                        }
                    }
                }
                else if (operandStart == -1)
                {
                    operandStart = i;
                }
            }

            if (operandStart != -1)
            {
                postFixExpression.Add(expression.Substring(operandStart, expression.Length - operandStart));
            }

            while (operators.Count > 0)
            {
                postFixExpression.Add(operators.Pop().ToString());
            }

            return postFixExpression;
        }

        /// <summary>
        /// Compiles our expression tree using our shuntingYard alogirthmn.
        /// See https://en.wikipedia.org/wiki/Binary_expression_tree .
        /// </summary>
        /// <param name="expression">A string containing our expression.</param>
        /// <returns>return expression tree.</returns>
        private Node Compile(string expression)
        {
            Stack<Node> nodes = new Stack<Node>();
            var postfixExpression = this.ShuntingYard(expression);

            foreach (var token in postfixExpression)
            {
                if (token.Length == 1 && this.IsOperatorOrParenthesis(token[0]))
                {
                    OperatorNode node = this.factory.CreateOperatorNode(token[0]);
                    node.Right = nodes.Pop();
                    node.Left = nodes.Pop();
                    nodes.Push(node);
                }
                else
                {
                    if (double.TryParse(token, out double num))
                    {
                        nodes.Push(new ConstantNode(num));
                    }
                    else
                    {
                        nodes.Push(new VariableNode(token, ref this.variables));
                    }
                }
            }

            return nodes.Pop();
        }

        /// <summary>
        /// check if char is operator or parenthesis.
        /// </summary>
        /// <param name="ch">takes in char.</param>
        /// <returns>true or false.</returns>
        private bool IsOperatorOrParenthesis(char ch)
        {
            return this.factory.IsAOperator(ch) || ch == '(' || ch == ')';
        }

        /// <summary>
        /// check if ch is '('.
        /// </summary>
        /// <param name="ch">char.</param>
        /// <returns>bool.</returns>
        private bool IsLeftParenthesis(char ch)
        {
            return ch == '(';
        }

        /// <summary>
        /// check if ch ')'.
        /// </summary>
        /// <param name="ch">char.</param>
        /// <returns>bool.</returns>
        private bool IsRightParenthesis(char ch)
        {
            return ch == ')';
        }

        /// <summary>
        /// check if operator1 has higher precedence then operator2.
        /// </summary>
        /// <param name="operator1">operator1.</param>
        /// <param name="operator2">operatorr2.</param>
        /// <returns>bool.</returns>
        private bool IsHigherPrecedence(char operator1, char operator2)
        {
            return this.factory.Prec(operator1) > this.factory.Prec(operator2);
        }

        /// <summary>
        /// check if operator1 has lower precedence then operator2.
        /// </summary>
        /// <param name="operator1">operator1.</param>
        /// <param name="operator2">operator2.</param>
        /// <returns>bool.</returns>
        private bool IsLowerPrecedence(char operator1, char operator2)
        {
            return this.factory.Prec(operator1) < this.factory.Prec(operator2);
        }

        /// <summary>
        /// check for same precedence.
        /// </summary>
        /// <param name="operator1">operator1.</param>
        /// <param name="operator2">operator2.</param>
        /// <returns>bool.</returns>
        private bool IsSamePrecedence(char operator1, char operator2)
        {
            return this.factory.Prec(operator1) == this.factory.Prec(operator2);
        }

        /// <summary>
        /// checks right associativity.
        /// </summary>
        /// <param name="operator1">operator.</param>
        /// <returns>bool.</returns>
        private bool IsRightAssociative(char operator1)
        {
            return this.factory.Associativity(operator1) == OperatorNode.AssociativityEnum.Right;
        }

        /// <summary>
        /// checks left associtivity.
        /// </summary>
        /// <param name="operator1">operator.</param>
        /// <returns>bool.</returns>
        private bool IsLeftAssociative(char operator1)
        {
            return this.factory.Associativity(operator1) == OperatorNode.AssociativityEnum.Left;
        }
    }
}