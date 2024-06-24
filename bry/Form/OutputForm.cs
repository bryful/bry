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
		protected override string GetPersistString()
		{
			return "OutputForm";
		}
		public TextBox Output
		{
			get { return textBox1; }
		}
		public Font OutputFont
		{
			get { return textBox1.Font; }
			set
			{
				textBox1.Font = value;
			}
		}
		public OutputForm()
		{
			InitializeComponent();
		}
	}
}
