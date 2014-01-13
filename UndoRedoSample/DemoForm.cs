// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System.Windows.Forms;
using UndoRedoSample.Data;

namespace UndoRedoSample
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