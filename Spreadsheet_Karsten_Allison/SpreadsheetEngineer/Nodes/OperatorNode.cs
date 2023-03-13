// <copyright file="OperatorNode.cs" company="Karsten Allison 011783232">
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
    /// OperatorNode which contains a operator (char c) and potentially a left and right child.
    /// </summary>
    public abstract class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// Constructor for an operator node, takes the operator char as an argument.
        /// </summary>
        public OperatorNode()
        {
        }

        /// <summary>
        /// sets the right or left associtivity.
        /// </summary>
        public enum AssociativityEnum
        {
            /// <summary>
            /// associativity of right node.
            /// </summary>
            Right,

            /// <summary>
            /// associativity of left node.
            /// </summary>
            Left,

            // only the ^ operater needs left associtivty, but good programming practice to keep this available.
        }

        /// <summary>
        /// Gets or sets the left child in the node.
        /// </summary>
        public Node Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the right child in the node.
        /// </summary>
        public Node Right
        {
            get;
            set;
        }
    }
}
