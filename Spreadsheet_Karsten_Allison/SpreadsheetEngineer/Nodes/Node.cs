// <copyright file="Node.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// abstract node class which is empty.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// abstract evaluate method to be overloaded.
        /// </summary>
        /// <returns>double.</returns>
        public abstract double Evaluate();
    }
}
