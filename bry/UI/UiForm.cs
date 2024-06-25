using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;

namespace bry
{
	public partial class UiForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		protected override string GetPersistString()
		{
			return "UiForm";
		}
		public UiForm()
		{
			InitializeComponent();
		}
		public void ClearControls()
		{
			this.SuspendLayout();
			ClearSub(this.Controls);
			this.ResumeLayout();
		}
		private void ClearSub(Control.ControlCollection  c)
		{
			if (c.Count > 0)
			{
				for(int i = c.Count-1; i >=0; i--) 
				{
					Control cc = c[i];
					if ((cc is Panel)|| (cc is GroupBox)||(cc is UiLayout))
					{
						ClearSub(cc.Controls);
					}
					cc.Dispose();
					c.RemoveAt(i);
				}
				c.Clear();
			}
		}
	
	}
}
