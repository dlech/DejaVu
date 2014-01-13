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
