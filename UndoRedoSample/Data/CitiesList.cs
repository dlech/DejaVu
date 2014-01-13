// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Drawing;
using DejaVu;
using DejaVu.Collections.Generic;

namespace UndoRedoSample.Data
{
	class CitiesList : UndoRedoList<City>
	{
		public static CitiesList Load()
		{
			using (UndoRedoManager.Start("Init"))
			{
				var cities = new CitiesList
				{
				    new City("New York", 5, 5, 50, 30, Color.RoyalBlue),
				    new City("Berlin", 5, 40, 80, 30, Color.Coral),
				    new City("Moscow", 5, 75, 60, 30, Color.LimeGreen),
				    new City("Rio", 5, 110, 40, 30, Color.Violet)
				};
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
