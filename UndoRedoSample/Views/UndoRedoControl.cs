// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DejaVu;

namespace UndoRedoSample.Views
{
	public partial class UndoRedoControl : UserControl
	{
		public UndoRedoControl()
		{
			InitializeComponent();

			UndoRedoManager.CommandDone += delegate
			{
				listUndo.DataSource = new List<string>(UndoRedoManager.UndoCommands);
				listRedo.DataSource = new List<string>(UndoRedoManager.RedoCommands);

				btnUndo.Enabled = UndoRedoManager.CanUndo;
				btnRedo.Enabled = UndoRedoManager.CanRedo;
			};
		}

		// Undo
		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UndoRedoManager.Undo();
		}

		// Redo
		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UndoRedoManager.Redo();
		}

		private void btnViewLog_Click(object sender, EventArgs e)
		{
			new LogViewer(UndoRedoManager.GetLog()).ShowDialog();
		}

	}
}
