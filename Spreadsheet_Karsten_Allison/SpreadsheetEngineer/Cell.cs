// <copyright file="Cell.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CptS321;
using SpreadsheetEngine;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Cell class, holds text, values, and array location properties.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// the (string) text value of the cell.
        /// </summary>
        private protected string text = string.Empty;

        /// <summary>
        /// the (string) value of a cell.
        /// </summary>
        private protected string value = string.Empty;

        /// <summary>
        /// the (string) tag value of a cell.
        /// </summary>
        private protected string cellTag;

        /// <summary>
        /// the (int) color of a cell. Default -1 which makes it white.
        /// </summary>
        private protected int cellColor = -1;

        /// <summary>
        /// the (int) row number of a cell.
        /// </summary>
        private int rowIndex;

        /// <summary>
        /// the (int) column number of a cell.
        /// </summary>
        private int columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// A cell constructor, that takes a rIndex, and cIndex for use in its array position.
        /// </summary>
        /// <param name="rIndex">row index.</param>
        /// <param name="cIndex">column index.</param>
        public Cell(int rIndex, int cIndex)
        {
            this.rowIndex = rIndex;
            this.columnIndex = cIndex;
            this.cellTag += Convert.ToChar('A' + cIndex);
            this.cellTag += (rIndex + 1).ToString();
        }

        /// <summary>
        /// PropertyChanged Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets the rowIndex of a cell.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets the columnIndex of a cell.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets the value of a cell.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the cell's "tag".
        /// </summary>
        public string CellTag
        {
            get { return this.cellTag; }
        }

        /// <summary>
        /// Gets or sets the text of a cell.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value)
                {
                    return;
                }

                this.text = value;

                // send that the Text event was updated.
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// Gets or sets the color of a cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return (uint)this.cellColor;
            }

            set
            {
                if (this.cellColor == value)
                {
                    return;
                }
                else
                {
                    this.cellColor = (int)value;

                    this.PropertyChanged(this, new PropertyChangedEventArgs("BGColor"));
                }
            }
        }
    }
}
