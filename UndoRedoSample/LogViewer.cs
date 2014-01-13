using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UndoRedoSample
{
	public partial class LogViewer : Form
	{
		public LogViewer(string log)
		{
			InitializeComponent();
			textBox1.Text = "Commands log (latest on bottom).\r\n\r\n" + log;
			textBox1.Select(0, 0);
		}
	}
}
