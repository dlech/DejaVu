// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DejaVu;
using System.Reflection;

namespace UndoRedoDemo.Views
{
	partial class EditCityControl : UserControl
	{
		CitiesList cities;
		// default position of new city
		UndoRedo<int> cityPosition = new UndoRedo<int>(145);
		UndoRedo<int> cityOrder = new UndoRedo<int>(5);

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

		IEnumerable<string> GetColors()
		{
			foreach (PropertyInfo p in typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static))
				yield return p.Name.Substring(p.Name.IndexOf(" ") + 1);
		}

		public void SetData(CitiesList data)
		{			
			cities = data;
			bsrcCities.DataSource = data;
			cbCity.DataSource = bsrcCities;
			ReloadData();
		}

        bool reloading = false;
		void ReloadData()
		{
            if (!reloading)
            {
                reloading = true;
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
                reloading = false;
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
			string name = "City" + cityOrder.Value;

			using (UndoRedoManager.Start("Add " + name))
			{
				City city = new City(name, 5, cityPosition.Value, 10, 35, Color.Cyan);
				cities.Add(city);
				cityPosition.Value += 40;
				cityOrder.Value++;
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
					cities.Remove(CurrentCity);
					UndoRedoManager.Commit();
				}
			}
		}
	}
}
