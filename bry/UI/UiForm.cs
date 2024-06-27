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
		
		public UiHLayout addHLayout()
		{
			UiHLayout ly = newHLayout();
			this.Controls.Add(ly);
			return ly;
		}
		public UiVLayout addVLayout()
		{
			UiVLayout ly = newVLayout();
			this.Controls.Add(ly);
			return ly;
		}
		public void initLayout()
		{
			if (this.Controls.Count > 0)
			{
				for(int i = 0; i < this.Controls.Count; i++)
				{
					if (this.Controls[i] is UiLayout)
					{
						UiLayout yi = (UiLayout)this.Controls[i];
						yi.Dock = DockStyle.Fill;
						yi.layouter();
					}
				}
			}
		}
		// ********************************************************
		public UiBtn newBtn(
			string tx = "",
			int w = 150,
			int h = 24,
			SizePolicy hp = SizePolicy.Fixed,
			SizePolicy vp = SizePolicy.Fixed)
		{
			UiBtn ctrl = new UiBtn();
			ctrl.Name = CanUseName("btn");
			if (tx == "") tx = ctrl.Name;
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		public UiLabel newLabel(
			string tx = "",
			int w = 150,
			int h = 24,
			SizePolicy hp = SizePolicy.Fixed,
			SizePolicy vp = SizePolicy.Fixed)
		{
			UiLabel ctrl = new UiLabel();
			ctrl.Name = CanUseName("label");
			if (tx == "") tx = ctrl.Name;
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		public UiHLayout newHLayout(
			string tx = "",
			int w = 8,
			int h = 8,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiHLayout ctrl = new UiHLayout();
			ctrl.Name = CanUseName("hlayout");
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		public UiVLayout newVLayout(
			string tx = "",
			int w = 8,
			int h = 8,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiVLayout ctrl = new UiVLayout();
			ctrl.Name = CanUseName("vlayout");
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		public UiSpace newSpace(
			string tx = "",
			int w = 8,
			int h = 8,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiSpace ctrl = new UiSpace();
			ctrl.Name = CanUseName("space");
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		public UiTextBox newTextBox(
			string tx = "",
			int w = 8,
			int h = 8,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiTextBox ctrl = new UiTextBox();
			ctrl.Name = CanUseName("textbox");
			if (tx == "") tx = ctrl.Name;
			ctrl.Text = tx;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			return ctrl;
		}
		// ********************************************************
		public string CanUseName(string key)
		{
			List<string> mkeys = FindKeyFromControls(this.Controls,key);
			if (mkeys.Count==0)
			{
				return key + "1";
			}
			int idx = 0;
			for(int i = 0;i<mkeys.Count;i++)
			{
				string n = mkeys[i].Substring(key.Length).Trim();
				int v = 0;
				if (int.TryParse(n,out v))
				{
					if (idx<v) idx= v;
				}
			}
			return $"{key}{idx+1}";
		}
		private List<string> FindKeyFromControls(Control.ControlCollection cc,string key)
		{
			List<string> ret = new List<string>();

			key = key.ToLower();
			if (cc.Count > 0)
			{
				foreach(Control c in cc)
				{
					if (c is UiControl)
					{
						if (c.Name.ToLower().IndexOf(key)==0)
						{
							ret.Add(c.Name);
						}
					}
					if (c is UiLayout)
					{
						List<string> ret2 = FindKeyFromControls(c.Controls, key);
						if (ret2.Count > 0)
						{
							ret.AddRange(ret2);
						}
					}
				}
			}
			return ret;
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
