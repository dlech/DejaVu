// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DejaVu;
using UndoRedoSample.Data;

namespace UndoRedoSample.Views
{
	partial class EditCityControl : UserControl
	{
		CitiesList _cities;
		// default position of new city
	    readonly UndoRedo<int> _cityPosition = new UndoRedo<int>(145);
	    readonly UndoRedo<int> _cityOrder = new UndoRedo<int>(5);

		public EditCityControl()
		{
			InitializeComponent();

			// bind cbBoxColor
			cbCityColor.DataSource = new List<string>(GetColors());

			UndoRedoManager.CommandDone += delegate { ReloadData();  };

			txtName.TextChanged += delegate { btnApply.Enabled = true; };
			numWidth.ValueChanged += delegate { btnApply.Enabled = true; };
			numWidth.KeyDown += delegate { btnApply.Enabled = true; };
			cbCityColor.SelectedValueChanged += delegate { btnApply.Enabled = true; };
		}

	    static IEnumerable<string> GetColors()
		{
		    return typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static).Select(p => p.Name.Substring(p.Name.IndexOf(" ") + 1));
		}

	    public void SetData(CitiesList data)
		{			
			_cities = data;
			bsrcCities.DataSource = data;
			cbCity.DataSource = bsrcCities;
			ReloadData();
		}

        bool _reloading = false;
		void ReloadData()
		{
            if (!_reloading)
            {
                _reloading = true;
                SuspendLayout();

                City c = CurrentCity;
                bsrcCities.ResetBindings(false);
                CurrentCity = c;
                if (CurrentCity != null)
                {
                    txtName.Text = CurrentCity.Name;
                    numWidth.Value = CurrentCity.Width;
                    cbCityColor.SelectedItem = CurrentCity.Color.Name;
                }
                groupEdit.Visible = (CurrentCity != null);
                btnApply.Enabled = false;

                ResumeLayout();
                _reloading = false;
            }
		}
		
		City CurrentCity
		{
			get { return (City)cbCity.SelectedItem; }
            set
            {
               /* foreach (object city in cbCity.Items)
                {
                    //if (city == value)
                }*/
                if (value != null)
                {
                    int i = cbCity.Items.IndexOf(value);
                    if (i != -1)
                        cbCity.SelectedIndex = i;
                }
            }
		}
		
		// Select City
		private void cbBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ReloadData();
		}
		
		// Edit City
		private void btnApply_Click(object sender, EventArgs e)
		{
			using (UndoRedoManager.Start("Edit " + CurrentCity.Name))
			{
				CurrentCity.Name = txtName.Text;
				CurrentCity.Color = Color.FromName((string)cbCityColor.SelectedItem);
				CurrentCity.Width = (int)numWidth.Value;
				UndoRedoManager.Commit();
			}
		}		

		// Add City
		private void btnAdd_Click(object sender, EventArgs e)
		{
			string name = "City" + _cityOrder.Value;

			using (UndoRedoManager.Start("Add " + name))
			{
				City city = new City(name, 5, _cityPosition.Value, 10, 35, Color.Cyan);
				_cities.Add(city);
				_cityPosition.Value += 40;
				_cityOrder.Value++;
				UndoRedoManager.Commit();
                CurrentCity = city;
			}
		}

		// Remove City
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (CurrentCity != null)
			{
				using (UndoRedoManager.Start("Remove " + CurrentCity.Name))
				{
					_cities.Remove(CurrentCity);
					UndoRedoManager.Commit();
				}
			}
		}
	}
}
