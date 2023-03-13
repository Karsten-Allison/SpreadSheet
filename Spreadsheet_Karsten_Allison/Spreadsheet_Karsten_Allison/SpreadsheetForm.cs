// <copyright file="SpreadsheetForm.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321;
using SpreadsheetEngine;

namespace Spreadsheet_Karsten_Allison
{
    /// <summary>
    /// Form that contains a spreadsheet type application.
    /// </summary>
    public partial class MainSpreadsheet : Form
    {
        /// <summary>
        /// inilizes a spreedsheet instance of size 50*26.
        /// </summary>
        private Spreadsheet spreadSheet = new Spreadsheet(50, 26);

        /// <summary>
        /// initializes a instance of the UndoRedoClass for storing/getting undo or redo commands.
        /// </summary>
        private UndoRedoClass undoRedo = new UndoRedoClass();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainSpreadsheet"/> class.
        /// initilizes the spreadsheet.
        /// </summary>
        public MainSpreadsheet()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// does nothing.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void DataGridMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// generates the main spreadsheet.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void Spreadsheet_Load(object sender, EventArgs e)
        {
            // wipe grid information
            this.dataGridMain.Rows.Clear();
            this.dataGridMain.Columns.Clear();

            // subscribing to property changed event
            this.spreadSheet.CellPropertyChanged += this.CellPropertyChanged;

            string[] funnylist =
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            };

            this.dataGridMain.RowHeadersWidth = 50;
            this.dataGridMain.ColumnCount = 26;
            this.dataGridMain.RowCount = 50;

            for (int i = 0; i < 26; i++)
            {
                this.dataGridMain.Columns[i].Name = funnylist[i];
            }

            for (int i = 1; i < 51; i++)
            {
                this.dataGridMain.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        /// <summary>
        /// Opens a new LoadFileDialog, sets the settings, then loads the xml.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "XML files (*.xml)|*.xml";
            loadFileDialog.Title = "Load SpreadSheet";

            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                // "Clear all spreadsheet data before loading file data. The load-from-file action is NOT a merge with existing content."
                this.Clear();

                Stream ifStream = new FileStream(loadFileDialog.FileName, FileMode.Open, FileAccess.Read);
                this.spreadSheet.Load(ifStream);
                ifStream.Dispose();

                // "Clear the undo/redo stacks after loading a file."
                this.undoRedo.ClearUndoRedo();
                this.RefreshButtons();
            }
        }

        /// <summary>
        /// Updates the cell value when the property is changed.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell currentcell = sender as Cell;

            if (e.PropertyName == "Value")
            {
                this.dataGridMain.Rows[currentcell.RowIndex].Cells[currentcell.ColumnIndex].Value = currentcell.Value;
            }

            if (e.PropertyName == "BGColor")
            {
                this.dataGridMain.Rows[currentcell.RowIndex].Cells[currentcell.ColumnIndex].Style.BackColor = Color.FromArgb((int)currentcell.BGColor);
            }
        }

        /// <summary>
        /// When starting to edit the cell, show the "text" of the cell instead of its value.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void DataGridMain_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Cell editingCell = this.spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);

            this.dataGridMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editingCell.Text;
        }

        /// <summary>
        /// When ending to edit the cell, Show the cells "value" instead of its text.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void DataGridMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Cell editingCell = this.spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);

            List<IDoUndoRedoCommand> undoRedoList = new List<IDoUndoRedoCommand>();

            undoRedoList.Add(new UndoRedoText(editingCell.Text, editingCell.CellTag));

            if (this.dataGridMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != null)
            {
                editingCell.Text = this.dataGridMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                editingCell.Text = string.Empty;
            }

            DoCommand tempCmd = new DoCommand(undoRedoList, "change cell text");

            this.undoRedo.AddUndo(tempCmd);

            this.RefreshButtons();

            this.dataGridMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editingCell.Value;
        }

        /// <summary>
        /// Refreshes the first toolStrip Column which has the Undo or Redo buttons.
        /// </summary>
        private void RefreshButtons()
        {
            ToolStripItemCollection toolStripItems = this.menuStrip1.Items;

            // Gets the Edit Column of the toolstrip, [1] has the Cell column.
            ToolStripMenuItem toolStripEditColumn = toolStripItems[0] as ToolStripMenuItem;

            toolStripEditColumn.DropDownItems[0].Enabled = this.undoRedo.BoolUndoable;
            toolStripEditColumn.DropDownItems[0].Text = "Undo " + this.undoRedo.UndoStackNextCommand;

            toolStripEditColumn.DropDownItems[1].Enabled = this.undoRedo.BoolRedoable;
            toolStripEditColumn.DropDownItems[1].Text = "Redo " + this.undoRedo.RedoStackNextCommand;
        }

        /// <summary>
        /// Upon clicking the change color button, opens a color dialouge and applys it to all selected datagridmainCells.
        /// Also pushes the old cell colors into a undoredolist, so it can then be passed into our undostack.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void ChangeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedColor = 0;

            ColorDialog colorDialog = new ColorDialog();

            List<IDoUndoRedoCommand> undoRedoList = new List<IDoUndoRedoCommand>();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color.ToArgb();

                foreach (DataGridViewCell cell in this.dataGridMain.SelectedCells)
                {
                    Cell spreadsheetCell = this.spreadSheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    UndoRedoColor undoRedoC = new UndoRedoColor((int)spreadsheetCell.BGColor, spreadsheetCell.CellTag);

                    undoRedoList.Add(undoRedoC);

                    spreadsheetCell.BGColor = (uint)selectedColor;
                }

                DoCommand tempCmd = new DoCommand(undoRedoList, "change bg color");

                this.undoRedo.AddUndo(tempCmd);

                this.RefreshButtons();
            }
        }

        /// <summary>
        /// Upon clicking the Undo button, calls the Undo command from the undoRedo class, and refreshes the buttons.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void UndoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.undoRedo.Undo(this.spreadSheet);
            this.RefreshButtons();
        }

        /// <summary>
        /// Upon clicking the Undo button, calls the Redo command from the undoRedo class, and refreshes the buttons.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void RedoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.undoRedo.Redo(this.spreadSheet);
            this.RefreshButtons();
        }

        /// <summary>
        /// Opens a new SaveFileDialog, sets the settings, and then saves the xml.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog.Title = "Save SpreadSheet";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream ofStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                this.spreadSheet.Save(ofStream);
                ofStream.Dispose();
            }
        }

        /// <summary>
        /// Clears the spreadsheet.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // might as well add this as a button if im making the function.
            this.Clear();
        }

        /// <summary>
        /// checks if a cell has modified text, value, or bg color. If so, "clear" it.
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < this.spreadSheet.RowCount; i++)
            {
                for (int j = 0; j < this.spreadSheet.ColumnCount; j++)
                {
                    // note that 4294967295 = uint max value, which is also our default color (White/FFFFFFFF/Fortississississississimo musically).
                    if (this.spreadSheet.GetCell(i, j).Text != string.Empty
                        || this.spreadSheet.GetCell(i, j).Value != string.Empty
                        || this.spreadSheet.GetCell(i, j).BGColor != 4294967295)
                    {
                        this.spreadSheet.GetCell(i, j).Text = string.Empty;
                        this.spreadSheet.GetCell(i, j).BGColor = 4294967295;
                    }
                }
            }
        }
    }
}
