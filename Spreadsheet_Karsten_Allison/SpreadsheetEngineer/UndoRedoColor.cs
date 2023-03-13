// <copyright file="UndoRedoColor.cs" company="Karsten Allison 011783232">
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
    /// Class that inherits from our DoUndoRedoCommand interface, used for storing
    /// the old cellColor values.
    /// </summary>
    public class UndoRedoColor : IDoUndoRedoCommand
    {
        /// <summary>
        /// int cellColor, which contains the color of the cell.
        /// </summary>
        private int cellColor;

        /// <summary>
        /// string cellName, which contains the "name" or "tag" of the cell.
        /// </summary>
        private string cellName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoColor"/> class.
        /// Constructor for the UndoRedoColor class, which takes  and stores the inputted name and color of the cell.
        /// </summary>
        /// <param name="oldCellColor">(int)color.</param>
        /// <param name="oldCellName">Name of cell.</param>
        public UndoRedoColor(int oldCellColor, string oldCellName)
        {
            this.cellColor = oldCellColor;
            this.cellName = oldCellName;
        }

        /// <summary>
        /// inherited abstract interface method, saves and returns the oldColor of a cell.
        /// </summary>
        /// <param name="spreadSheet">spreadSheet Instance.</param>
        /// <returns>(UndoRedoColor)the old cell color.</returns>
        public IDoUndoRedoCommand Exec(Spreadsheet spreadSheet)
        {
            Cell cell = spreadSheet.GetCell(this.cellName);

            int old = (int)cell.BGColor;

            cell.BGColor = (uint)this.cellColor;

            UndoRedoColor oldColor = new UndoRedoColor(old, this.cellName);

            return oldColor;
        }
    }
}
