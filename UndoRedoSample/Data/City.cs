// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.Text;
using DejaVu;
using DejaVu.Collections.Generic;
using System.Drawing;

namespace UndoRedoDemo
{	
    class City
    {
        public City(string name, int x, int y, int width, int height, Color color)
        {
			UndoRedoManager.Log("Create city " + name);
			Name = name;
            Color = color;

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

		readonly UndoRedo<string> name = new UndoRedo<string>();
		public string Name
		{
			get { return name.Value; }
			set 
			{
				name.Value = value; 
			}
		}
        readonly UndoRedo<Color> color = new UndoRedo<Color>();
        public Color Color
        {
            get { return color.Value; }
            set { color.Value = value; }
		}

		readonly UndoRedo<int> x = new UndoRedo<int>();
        public int X
        {
            get { return x.Value; }
            set { x.Value = value; }
        }
        readonly UndoRedo<int> y = new UndoRedo<int>();
        public int Y
        {
            get { return y.Value; }
            set { y.Value = value; }
        }
        readonly UndoRedo<int> width = new UndoRedo<int>();
        public int Width
        {
            get { return width.Value; }
            set { width.Value = value; }
        }
        readonly UndoRedo<int> height = new UndoRedo<int>();
        public int Height
        {
            get { return height.Value; }
            set { height.Value = value; }
        }
    }

}
