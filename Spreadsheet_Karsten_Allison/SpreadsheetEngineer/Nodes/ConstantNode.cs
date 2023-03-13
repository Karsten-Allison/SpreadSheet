// <copyright file="ConstantNode.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Operator Node for containg a constant.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Private value.
        /// </summary>
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// constructs a constant node given a value.
        /// </summary>
        /// <param name="value">a double to be stored.</param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Overloaded evaluate function which gets the value.
        /// </summary>
        /// <returns>double.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
