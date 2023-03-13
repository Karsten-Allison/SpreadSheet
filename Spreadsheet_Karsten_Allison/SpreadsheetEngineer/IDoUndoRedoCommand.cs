// <copyright file="IDoUndoRedoCommand.cs" company="Karsten Allison 011783232">
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
    /// Interface for commands to be executed on a spreadsheet.
    /// </summary>
    public interface IDoUndoRedoCommand
    {
        /// <summary>
        /// abstract interface method called exec, which is inherited by whatever uses this.
        /// </summary>
        /// <param name="spreadSheet">spreadsheet instance.</param>
        /// <returns>abstract something idk.</returns>
        IDoUndoRedoCommand Exec(Spreadsheet spreadSheet);
    }

    /// <summary>
    /// class for executing an array(or list) of commands.
    /// </summary>
    public class DoCommand
    {
        /// <summary>
        /// string containing our command name.
        /// </summary>
        public string CommandName;

        /// <summary>
        /// Array containing our commands.
        /// </summary>
        private IDoUndoRedoCommand[] commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoCommand"/> class.
        /// </summary>
        /// <param name="commands">array of the commands.</param>
        /// <param name="name">name of the command that was executed.</param>
        public DoCommand(IDoUndoRedoCommand[] commands, string name)
        {
            this.commands = commands;
            this.CommandName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoCommand"/> class.
        /// </summary>
        /// <param name="commands">list of commands.</param>
        /// <param name="name">name of the command that was executed.</param>
        public DoCommand(List<IDoUndoRedoCommand> commands, string name)
        {
            this.commands = commands.ToArray();
            this.CommandName = name;
        }

        /// <summary>
        /// Creates and returns an array containing a list of commands, and the name of the command.
        /// </summary>
        /// <param name="sheet">spreadSheet class instance.</param>
        /// <returns>(DoCommand)array of commands.</returns>
        public DoCommand Execute(Spreadsheet sheet)
        {
            List<IDoUndoRedoCommand> commandList = new List<IDoUndoRedoCommand>();

            foreach (IDoUndoRedoCommand command in this.commands)
            {
                commandList.Add(command.Exec(sheet));
            }

            DoCommand commandArray = new DoCommand(commandList.ToArray(), this.CommandName);

            return commandArray;
        }
    }
}