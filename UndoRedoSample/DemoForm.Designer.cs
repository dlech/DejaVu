using UndoRedoSample.Views;

namespace UndoRedoSample
{
    partial class DemoForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.panel1 = new System.Windows.Forms.Panel();
			this.chartControl = new ChartControl();
			this.undoRedoControl1 = new UndoRedoControl();
			this.editCityControl = new EditCityControl();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.undoRedoControl1);
			this.panel1.Controls.Add(this.editCityControl);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(210, 382);
			this.panel1.TabIndex = 3;
			// 
			// chartControl
			// 
			this.chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControl.Location = new System.Drawing.Point(210, 0);
			this.chartControl.Margin = new System.Windows.Forms.Padding(2);
			this.chartControl.Name = "chartControl";
			this.chartControl.Size = new System.Drawing.Size(231, 382);
			this.chartControl.TabIndex = 2;
			this.chartControl.Text = "boxesControl1";
			// 
			// undoRedoControl1
			// 
			this.undoRedoControl1.Location = new System.Drawing.Point(3, 203);
			this.undoRedoControl1.Name = "undoRedoControl1";
			this.undoRedoControl1.Size = new System.Drawing.Size(206, 177);
			this.undoRedoControl1.TabIndex = 1;
			// 
			// editCityControl
			// 
			this.editCityControl.Location = new System.Drawing.Point(3, 3);
			this.editCityControl.Name = "editCityControl";
			this.editCityControl.Size = new System.Drawing.Size(206, 204);
			this.editCityControl.TabIndex = 0;
			// 
			// DemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(441, 382);
			this.Controls.Add(this.chartControl);
			this.Controls.Add(this.panel1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "DemoForm";
			this.Text = "Undo/Redo Demo";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private ChartControl chartControl;
		private System.Windows.Forms.Panel panel1;
		private UndoRedoControl undoRedoControl1;
		private EditCityControl editCityControl;
    }
}

