// <copyright file="OperatorNodeFactory.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// OperatorNodeFactory method.
    /// </summary>
    public class OperatorNodeFactory
    {
        /// <summary>
        /// A dictionary for our OperatorNodeFactory, keying char values to their types.
        /// </summary>
        private readonly Dictionary<char, Type> operators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// Constructor which calls LookForOperator by default.
        /// </summary>
        public OperatorNodeFactory()
        {
            this.LookForOperator((op, type) => this.operators.Add(op, type));
        }

        /// <summary>
        /// Allows us to pass the OnOperator Method as an argument.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <param name="type">type.</param>
        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// Bool function to check if operator is a key in the operators dict.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>bool.</returns>
        public bool IsAOperator(char op)
        {
            return this.operators.ContainsKey(op);
        }

        /// <summary>
        /// Returns the precedence of the given operator.
        /// </summary>
        /// <param name="ch">operator.</param>
        /// <returns>ushort.</returns>
        public ushort Prec(char ch)
        {
            if (this.IsAOperator(ch))
            {
                Type type = this.operators[ch];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object value = propertyInfo.GetValue(type);
                    if (value.GetType().Name == "UInt16")
                    {
                        return (ushort)value;
                    }
                }
            }

            // default return 0 if nothing is found.
            return 0;
        }

        /// <summary>
        /// Returns the Associativity of the given operator.
        /// </summary>
        /// <param name="ch">operator.</param>
        /// <returns>int.</returns>
        public OperatorNode.AssociativityEnum Associativity(char ch)
        {
            if (this.IsAOperator(ch))
            {
                Type type = this.operators[ch];
                PropertyInfo propertyInfo = type.GetProperty("Associativity");
                if (propertyInfo != null)
                {
                    object value = propertyInfo.GetValue(type);
                    if (value.GetType().Name == "Associative")
                    {
                        return (OperatorNode.AssociativityEnum)value;
                    }
                }
            }

            // default return left if nothing is found.
            return OperatorNode.AssociativityEnum.Left;
        }

        /// <summary>
        /// creates an operator Node.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>operator node.</returns>
        public OperatorNode CreateOperatorNode(char op)
        {
            if (this.IsAOperator(op))
            {
                object operatorNodeObject = Activator.CreateInstance(this.operators[op]);
                if (operatorNodeObject is OperatorNode node)
                {
                    return node;
                }
            }

            // return exception if nothing is found.
            throw new Exception("No Operator");
        }

        /// <summary>
        /// checks if operator is available.
        /// </summary>
        /// <param name="onOperator"> is a delegate with properties op, type.</param>
        private void LookForOperator(OnOperator onOperator)
        {
            Type operatorNodeType = typeof(OperatorNode);
            foreach (var assemply in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> operatorTypes = assemply.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                foreach (var type in operatorTypes)
                {
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        object value = operatorField.GetValue(type);
                        if (value is char operatorSymbol)
                        {
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
