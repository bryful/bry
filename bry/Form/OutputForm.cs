using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bry
{
	public partial class OutputForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public TextBox Output
		{
			get { return textBox1; }
		}
		public new Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				textBox1.Font = value;
			}
		}
		public OutputForm()
		{
			InitializeComponent();
		}
	}
}
