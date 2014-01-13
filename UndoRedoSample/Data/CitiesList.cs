// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.Text;
using DejaVu;
using DejaVu.Collections.Generic;
using System.Drawing;

namespace UndoRedoDemo
{
	class CitiesList : UndoRedoList<City>
	{
		public static CitiesList Load()
		{
			using (UndoRedoManager.Start("Init"))
			{
				CitiesList cities = new CitiesList();
				cities.Add(new City("New York", 5, 5, 50, 30, Color.RoyalBlue));
				cities.Add(new City("Berlin", 5, 40, 80, 30, Color.Coral));
				cities.Add(new City("Moscow", 5, 75, 60, 30, Color.LimeGreen));
				cities.Add(new City("Rio", 5, 110, 40, 30, Color.Violet));
				UndoRedoManager.ClearHistory();
				return cities;
			}
		}
		public static void Save()
		{
			throw new NotImplementedException();
		}
	}
}
