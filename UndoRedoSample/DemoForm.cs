// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace UndoRedoDemo
{
    public partial class DemoForm : Form
    {
		public DemoForm()
        {
            InitializeComponent();

            // init data
			CitiesList cities = CitiesList.Load();
            chartControl.SetData(cities);
			editCityControl.SetData(cities);
        }
    }
}