// <copyright file="MultiplyNode.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Operator Node for doing multiplication.
    /// </summary>
    internal class MultiplyNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyNode"/> class.
        /// Constructs nothing.
        /// </summary>
        public MultiplyNode()
        {
        }

        /// <summary>
        /// Gets the operator using lamda operator.
        /// </summary>
        public static char Operator => '*';

        /// <summary>
        /// Gets the precedence using lamda operator.
        /// </summary>
        public static ushort Precedence => 3;

        /// <summary>
        /// Gets the Associativity using lamda operator.
        /// All operators except ^ are left associative.
        /// </summary>
        public static AssociativityEnum Associativity => AssociativityEnum.Left;

        /// <summary>
        /// Overloaded evaluate function which calls our operator (+) on the Values from
        /// calling Evaluate on our Left and Right children.
        /// </summary>
        /// <returns>double.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
