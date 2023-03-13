// <copyright file="VariableNode.cs" company="Karsten Allison 011783232">
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
    /// Node containing a variable string value.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// read only variable value that is a string.
        /// </summary>
        private readonly string variableValue;

        /// <summary>
        /// instantiate dictionary with string and double called pairs.
        /// </summary>
        private readonly Dictionary<string, double> variableDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="newVariableValue">Variable value is equal to the input variable.</param>
        /// <param name="pairs">this is the dictionary passed in.</param>
        public VariableNode(string newVariableValue, ref Dictionary<string, double> pairs)
        {
            this.variableDictionary = pairs;
            this.variableValue = newVariableValue;
            this.variableDictionary[this.variableValue] = 0;
        }

        /// <summary>
        /// Overloaded evaluate function which returns our keyed value.
        /// </summary>
        /// <returns>double.</returns>
        public override double Evaluate()
        {
            // if variablevalue(its name) doesnt exist in the dictionary, return double.nan.
            if (!this.variableDictionary.ContainsKey(this.variableValue))
            {
                return double.NaN;
            }

            // otherwise return the value.
            return this.variableDictionary[this.variableValue];
        }
    }
}
