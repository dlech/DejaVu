﻿using System.ComponentModel;

namespace DejaVu
{
	public partial class Component1 : Component
	{
		public Component1()
		{
			InitializeComponent();
		}

		public Component1(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}
	}
}
