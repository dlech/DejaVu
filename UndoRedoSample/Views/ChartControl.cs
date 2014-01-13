// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using DejaVu;

namespace UndoRedoDemo
{
    partial class ChartControl : Control
    {
		private static int fontSize = 16;
		private static Font font = new Font("Verdana", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
		private CitiesList boxes;
        
        public ChartControl()
        {
            InitializeComponent();
			DoubleBuffered = true;
			UndoRedoManager.CommandDone += delegate { Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.FillRectangle(Brushes.Snow, 0, 0, Width, Height);
            g.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1);

            if (boxes != null)
            {
                foreach (City box in boxes)
                {
                    using (SolidBrush pen = new SolidBrush(box.Color))
                    {
                        g.FillRectangle(pen, box.X, box.Y, box.Width, box.Height);
                    }
                    Point p = new Point(box.X + box.Width + fontSize / 2, box.Y + box.Height / 2 - fontSize / 2);
                    g.DrawString(box.Name, font, Brushes.DimGray, p);
                }
            }
            else 
            {
                // this code for design mode mainly
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                RectangleF r = new RectangleF(0, 0, Width, Height);
                g.DrawString("Cities Chart", font, Brushes.Black, r, format);
            }
            base.OnPaint(pe);
        }

		internal void SetData(CitiesList cities)
		{
			boxes = cities;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}
	}
}
