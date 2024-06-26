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
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Windows;

namespace bry
{
	public partial class UiForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		[ScriptUsage(ScriptAccess.None)]
		protected override string GetPersistString()
		{
			return "UiForm";
		}
		[ScriptUsage(ScriptAccess.None)]
		public UiForm()
		{
			InitializeComponent();
		}
		[ScriptUsage(ScriptAccess.Full)]
		public void clear()
		{
			this.Controls.Clear();
		}
		[ScriptUsage(ScriptAccess.Full)]
		public void clearControls()
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
					if ((cc is Panel)|| (cc is GroupBox)||(cc is UiHLayout))
					{
						ClearSub(cc.Controls);
					}
					cc.Dispose();
					c.RemoveAt(i);
				}
				c.Clear();
			}
		}
		
		public UiHLayout addHLayout(string cap="HLayout")
		{
			UiHLayout ly = new UiHLayout();
			ly.Text = cap;
			this.Controls.Add(ly);
			return ly;
		}
		public UiVLayout addVLayout(string cap = "VLayout")
		{
			UiVLayout ly = new UiVLayout();
			ly.Text = cap;
			this.Controls.Add(ly);
			return ly;
		}
		// ********************************************************
		[ScriptUsage(ScriptAccess.None)]
		protected override void OnControlAdded(ControlEventArgs e)
		{
			if (e.Control is UiControl)
			{

			}
			else
			{
				this.Controls.Remove(e.Control);
			}
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.Invalidate();
		}
		// ********************************************************
		public List<UiControl> uiControlList
		{
			get
			{
				return listupControl();
			}
		}

		public List<UiControl> listupControl()
		{
			List<UiControl> uiControls = new List<UiControl>();

			ListupControlSub(this.Controls);

			return uiControls;
		}
		private List<UiControl> ListupControlSub(Control.ControlCollection cc)
		{
			List<UiControl> uiControls = new List<UiControl>();

			if(cc.Count > 0)
			{
				for (int i = 0;i< cc.Count; i++)
				{
					if (cc[i] is UiControl)
					{
						UiControl ccc = (UiControl)cc[i];
						uiControls.Add(ccc);
						if (ccc is UiLayout)
						{
							List<UiControl> ls = ListupControlSub(ccc.Controls);
							uiControls.AddRange(ls);

						}
					}
				}
			}
			return uiControls;
		}
		public string[] listToStrings(List<UiControl> a)
		{
			List<string> list = new List<string>();
			foreach(UiControl cc in a)
			{
				list.Add(cc.Name);
			}
			return list.ToArray();	
		}
		public string[] listToStrings()
		{
			return listToStrings(listupControl());
		}

	}
}
