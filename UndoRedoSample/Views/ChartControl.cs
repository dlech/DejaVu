// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Drawing;
using System.Windows.Forms;
using DejaVu;
using UndoRedoSample.Data;

namespace UndoRedoSample.Views
{
    sealed partial class ChartControl : Control
    {
        private const int FontSize = 16;
        private static readonly Font font = new Font("Verdana", FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
		private CitiesList _boxes;
        
        public ChartControl()
        {
            InitializeComponent();
			DoubleBuffered = true;
			UndoRedoManager.CommandDone += delegate { Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            g.FillRectangle(Brushes.Snow, 0, 0, Width, Height);
            g.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1);

            if (_boxes != null)
            {
                foreach (var box in _boxes)
                {
                    using (var pen = new SolidBrush(box.Color))
                    {
                        g.FillRectangle(pen, box.X, box.Y, box.Width, box.Height);
                    }
                    var p = new Point(box.X + box.Width + FontSize / 2, box.Y + box.Height / 2 - FontSize / 2);
                    g.DrawString(box.Name, font, Brushes.DimGray, p);
                }
            }
            else 
            {
                // this code for design mode mainly
                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                var r = new RectangleF(0, 0, Width, Height);
                g.DrawString("Cities Chart", font, Brushes.Black, r, format);
            }
            base.OnPaint(pe);
        }

		internal void SetData(CitiesList cities)
		{
			_boxes = cities;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}
	}
}
