// <copyright file="UndoRedoClass.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace SpreadsheetEngine
{
    /// <summary>
    /// A class for storing/retreiving Undo and Redo commands inside of stacks.
    /// </summary>
    public class UndoRedoClass
    {
        /// <summary>
        /// A stack containing our undo commands.
        /// </summary>
        private Stack<DoCommand> undoStack = new Stack<DoCommand>();

        /// <summary>
        /// A stack containing our redo commands.
        /// </summary>
        private Stack<DoCommand> redoStack = new Stack<DoCommand>();

        /// <summary>
        /// Gets a value indicating whether if the undostack has anything in it.
        /// </summary>
        public bool BoolUndoable
        {
            get
            {
                return this.undoStack.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether if the redostack has anything in it.
        /// </summary>
        public bool BoolRedoable
        {
            get
            {
                return this.redoStack.Count > 0;
            }
        }

        /// <summary>
        /// Gets the command name of the top undostack item.
        /// </summary>
        public string UndoStackNextCommand
        {
            get
            {
                if (this.undoStack.Count > 0)
                {
                    return this.undoStack.Peek().CommandName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the command name of the top redostack item.
        /// </summary>
        public string RedoStackNextCommand
        {
            get
            {
                if (this.redoStack.Count > 0)
                {
                    return this.redoStack.Peek().CommandName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Adds a command to the undo stack.
        /// </summary>
        /// <param name="undo">A DoCommand interface instance.</param>
        public void AddUndo(DoCommand undo)
        {
            this.undoStack.Push(undo);
            this.redoStack.Clear();
        }

        /// <summary>
        /// Executes the top UndoStack Command, then adds it to the RedoStack.
        /// </summary>
        /// <param name="spreadSheet">spreadSheet Instance.</param>
        public void Undo(Spreadsheet spreadSheet)
        {
            this.redoStack.Push(this.undoStack.Pop().Execute(spreadSheet));
        }

        /// <summary>
        /// Executes the top RedoStack Command, then adds it to the UndoStack.
        /// </summary>
        /// <param name="spreadSheet">spreadSheet Instance.</param>
        public void Redo(Spreadsheet spreadSheet)
        {
            this.undoStack.Push(this.redoStack.Pop().Execute(spreadSheet));
        }

        /// <summary>
        /// clears the undoStack and redoStack.
        /// </summary>
        public void ClearUndoRedo()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }
    }
}
