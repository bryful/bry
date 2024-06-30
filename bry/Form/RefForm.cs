using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bry
{
	public partial class RefForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		protected override string GetPersistString()
		{
			return "RefForm";
		}
		public  Font RefFont
		{
			get { return helpList1.Font; }
			set
			{
				helpList1.Font = value;
			}
		}
		public void SetSInfo(SInfo[] s)
		{
			helpList1.SetItems(s);
		}
		public MainForm MainForm
		{
			get { return helpList1.MainForm; }
			set { helpList1.MainForm = value; }
		}
		public RefForm()
		{
			InitializeComponent();
		}
	}
}
