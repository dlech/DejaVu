namespace UndoRedoDemo.Views
{
	partial class UndoRedoControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblRedo = new System.Windows.Forms.Label();
			this.lblUndo = new System.Windows.Forms.Label();
			this.listRedo = new System.Windows.Forms.ListBox();
			this.listUndo = new System.Windows.Forms.ListBox();
			this.btnRedo = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.btnViewLog = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnViewLog);
			this.groupBox2.Controls.Add(this.lblRedo);
			this.groupBox2.Controls.Add(this.lblUndo);
			this.groupBox2.Controls.Add(this.listRedo);
			this.groupBox2.Controls.Add(this.listUndo);
			this.groupBox2.Controls.Add(this.btnRedo);
			this.groupBox2.Controls.Add(this.btnUndo);
			this.groupBox2.Location = new System.Drawing.Point(2, 2);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox2.Size = new System.Drawing.Size(199, 167);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Plumming stuff";
			// 
			// lblRedo
			// 
			this.lblRedo.AutoSize = true;
			this.lblRedo.Location = new System.Drawing.Point(99, 19);
			this.lblRedo.Name = "lblRedo";
			this.lblRedo.Size = new System.Drawing.Size(55, 13);
			this.lblRedo.TabIndex = 10;
			this.lblRedo.Text = "Redo List:";
			// 
			// lblUndo
			// 
			this.lblUndo.AutoSize = true;
			this.lblUndo.Location = new System.Drawing.Point(5, 19);
			this.lblUndo.Name = "lblUndo";
			this.lblUndo.Size = new System.Drawing.Size(55, 13);
			this.lblUndo.TabIndex = 10;
			this.lblUndo.Text = "Undo List:";
			// 
			// listRedo
			// 
			this.listRedo.FormattingEnabled = true;
			this.listRedo.Location = new System.Drawing.Point(101, 34);
			this.listRedo.Margin = new System.Windows.Forms.Padding(2);
			this.listRedo.Name = "listRedo";
			this.listRedo.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listRedo.Size = new System.Drawing.Size(91, 69);
			this.listRedo.TabIndex = 9;
			// 
			// listUndo
			// 
			this.listUndo.FormattingEnabled = true;
			this.listUndo.Location = new System.Drawing.Point(7, 34);
			this.listUndo.Margin = new System.Windows.Forms.Padding(2);
			this.listUndo.Name = "listUndo";
			this.listUndo.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listUndo.Size = new System.Drawing.Size(91, 69);
			this.listUndo.TabIndex = 9;
			// 
			// btnRedo
			// 
			this.btnRedo.Enabled = false;
			this.btnRedo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnRedo.Location = new System.Drawing.Point(102, 107);
			this.btnRedo.Margin = new System.Windows.Forms.Padding(2);
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Size = new System.Drawing.Size(90, 24);
			this.btnRedo.TabIndex = 3;
			this.btnRedo.Text = "Redo";
			this.btnRedo.UseVisualStyleBackColor = true;
			this.btnRedo.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Enabled = false;
			this.btnUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnUndo.Location = new System.Drawing.Point(8, 107);
			this.btnUndo.Margin = new System.Windows.Forms.Padding(2);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(90, 24);
			this.btnUndo.TabIndex = 2;
			this.btnUndo.Text = "Undo";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
			// 
			// btnViewLog
			// 
			this.btnViewLog.Location = new System.Drawing.Point(61, 136);
			this.btnViewLog.Name = "btnViewLog";
			this.btnViewLog.Size = new System.Drawing.Size(75, 23);
			this.btnViewLog.TabIndex = 9;
			this.btnViewLog.Text = "View Log...";
			this.btnViewLog.UseVisualStyleBackColor = true;
			this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
			// 
			// UndoRedoControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Name = "UndoRedoControl";
			this.Size = new System.Drawing.Size(207, 176);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox listRedo;
		private System.Windows.Forms.ListBox listUndo;
		private System.Windows.Forms.Button btnRedo;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.Label lblRedo;
		private System.Windows.Forms.Label lblUndo;
		private System.Windows.Forms.Button btnViewLog;
	}
}
