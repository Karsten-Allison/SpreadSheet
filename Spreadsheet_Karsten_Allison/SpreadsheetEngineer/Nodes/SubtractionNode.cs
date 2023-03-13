// <copyright file="SubtractionNode.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Operator Node for doing subtraction.
    /// </summary>
    internal class SubtractionNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionNode"/> class.
        /// constructs nothing.
        /// </summary>
        public SubtractionNode()
        {
        }

        /// <summary>
        /// Gets the operator using lamda operator.
        /// </summary>
        public static char Operator => '-';

        /// <summary>
        /// Gets the Precedence using lamda operator.
        /// </summary>
        public static ushort Precedence => 4;

        /// <summary>
        /// Gets the Associativity using lamda operator.
        /// /// All operators except ^ are left associative.
        /// </summary>
        public static AssociativityEnum Associativity => AssociativityEnum.Left;

        /// <summary>
        /// Overloaded evaluate function which calls our operator (+) on the Values from
        /// calling Evaluate on our Left and Right children.
        /// </summary>
        /// <returns>double.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
