// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System.Drawing;
using DejaVu;

namespace UndoRedoSample.Data
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

		readonly UndoRedo<string> _name = new UndoRedo<string>();
		public string Name
		{
			get { return _name.Value; }
			set 
			{
				_name.Value = value; 
			}
		}
        readonly UndoRedo<Color> _color = new UndoRedo<Color>();
        public Color Color
        {
            get { return _color.Value; }
            set { _color.Value = value; }
		}

		readonly UndoRedo<int> _x = new UndoRedo<int>();
        public int X
        {
            get { return _x.Value; }
            set { _x.Value = value; }
        }
        readonly UndoRedo<int> _y = new UndoRedo<int>();
        public int Y
        {
            get { return _y.Value; }
            set { _y.Value = value; }
        }
        readonly UndoRedo<int> _width = new UndoRedo<int>();
        public int Width
        {
            get { return _width.Value; }
            set { _width.Value = value; }
        }
        readonly UndoRedo<int> _height = new UndoRedo<int>();
        public int Height
        {
            get { return _height.Value; }
            set { _height.Value = value; }
        }
    }

}
