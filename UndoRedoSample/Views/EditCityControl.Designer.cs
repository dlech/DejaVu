namespace UndoRedoSample.Views
{
	partial class EditCityControl
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
            this.components = new System.ComponentModel.Container();
            this.groupEdit = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.cbCityColor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.bsrcCities = new System.Windows.Forms.BindingSource(this.components);
            this.groupEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsrcCities)).BeginInit();
            this.SuspendLayout();
            // 
            // groupEdit
            // 
            this.groupEdit.Controls.Add(this.btnRemove);
            this.groupEdit.Controls.Add(this.label4);
            this.groupEdit.Controls.Add(this.txtName);
            this.groupEdit.Controls.Add(this.btnAdd);
            this.groupEdit.Controls.Add(this.label3);
            this.groupEdit.Controls.Add(this.numWidth);
            this.groupEdit.Controls.Add(this.label2);
            this.groupEdit.Controls.Add(this.btnApply);
            this.groupEdit.Controls.Add(this.cbCityColor);
            this.groupEdit.Location = new System.Drawing.Point(3, 31);
            this.groupEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupEdit.Name = "groupEdit";
            this.groupEdit.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupEdit.Size = new System.Drawing.Size(265, 214);
            this.groupEdit.TabIndex = 9;
            this.groupEdit.TabStop = false;
            this.groupEdit.Text = "Edit";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(137, 176);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(120, 30);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "Remove City";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Color:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(99, 18);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(157, 22);
            this.txtName.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(136, 142);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add City";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Chart Value:";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(99, 48);
            this.numWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(157, 22);
            this.numWidth.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(136, 106);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(120, 30);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbCityColor
            // 
            this.cbCityColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCityColor.FormattingEnabled = true;
            this.cbCityColor.Location = new System.Drawing.Point(99, 76);
            this.cbCityColor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCityColor.Name = "cbCityColor";
            this.cbCityColor.Size = new System.Drawing.Size(159, 24);
            this.cbCityColor.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose city:";
            // 
            // cbCity
            // 
            this.cbCity.DisplayMember = "Name";
            this.cbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(103, 1);
            this.cbCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(167, 24);
            this.cbCity.TabIndex = 7;
            this.cbCity.SelectedIndexChanged += new System.EventHandler(this.cbBox_SelectedIndexChanged);
            // 
            // EditCityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCity);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditCityControl";
            this.Size = new System.Drawing.Size(275, 251);
            this.groupEdit.ResumeLayout(false);
            this.groupEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsrcCities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupEdit;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numWidth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.ComboBox cbCityColor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbCity;
		private System.Windows.Forms.BindingSource bsrcCities;
	}
}
