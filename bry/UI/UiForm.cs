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

		private Rectangle m_TrueClientRect = new Rectangle();
		public Rectangle TrueClientRect
		{
			get { return m_TrueClientRect; }
		}
		private void ChkTrueClientRect()
		{
			int w = this.ClientRectangle.Width;
			int h = this.ClientRectangle.Height;
			int l = this.ClientRectangle.Left;
			int t = this.ClientRectangle.Top;

			if (this.Controls.Count > 0)
			{
				foreach (Control c in this.Controls)
				{
					if ((c is MenuStrip) || (c is StatusStrip) || (c is ToolStrip))
					{
						switch (c.Dock)
						{
							case DockStyle.Top:
								t += c.Height;
								h -= c.Height;
								break;
							case DockStyle.Bottom:
								h -= c.Height;
								break;
							case DockStyle.Left:
								w -= c.Width;
								l += c.Width;
								break;
							case DockStyle.Right:
								w -= c.Width;
								break;
						}
					}
				}

			}

			// Margin補正
			w -= (this.Margin.Left + this.Margin.Right);
			h -= (this.Margin.Top + this.Margin.Bottom);
			l += (this.Margin.Left);
			t += (this.Margin.Top);
			w -= (this.Padding.Left + this.Padding.Right);
			h -= (this.Padding.Top + this.Padding.Bottom);
			l += (this.Padding.Left);
			t += (this.Padding.Top);
			m_TrueClientRect = new Rectangle(l,t, w, h);
		}
		[ScriptUsage(ScriptAccess.None)]
		public UiForm()
		{
			InitializeComponent();
			ChkTrueClientRect();
		}
		[ScriptUsage(ScriptAccess.Full)]
		public void clear()
		{
			m_UiLayout = null;
			this.Controls.Clear();
		}
		private UiLayout m_UiLayout = null;
		public UiHLayout addHLayout()
		{
			UiHLayout ly = newHLayout();
			ly.Location = TrueClientRect.Location;
			ly.Size = TrueClientRect.Size;
			this.Controls.Add(ly);
			m_UiLayout = ly;
			return ly;
		}
		public UiVLayout addVLayout()
		{
			UiVLayout ly = newVLayout();
			ly.Location = TrueClientRect.Location;
			ly.Size = TrueClientRect.Size;
			this.Controls.Add(ly);
			m_UiLayout = ly;
			return ly;
		}
		public void initLayout()
		{
			initLayoutSub(this);
		}
		public void initLayoutSub(Control c)
		{
			if (c is UiLayout)
			{
				((UiLayout)c).callChkLayout();
			}
			if (c.Controls.Count > 0)
			{
				for (int i = 0; i < c.Controls.Count; i++)
				{
					if (c.Controls[i] is UiLayout)
					{
						UiLayout yi = (UiLayout)c.Controls[i];
						initLayoutSub(yi);
					}
				}
			}
		}
		// ********************************************************
		public UiBtn newBtn(
			string tx = "",
			int w = 130,
			int h = 31,
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
			int h = 31,
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
			int w = 150,
			int h = 31,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Fixed)
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
		public UiListBox newListBox(
	string tx = "",
	int w = 150,
	int h = 200,
	SizePolicy hp = SizePolicy.Expanding,
	SizePolicy vp = SizePolicy.Expanding)
		{
			UiListBox ctrl = new UiListBox();
			ctrl.Name = CanUseName("listbox");
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
			base.OnControlAdded(e);
			ChkTrueClientRect();
		}
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			ChkTrueClientRect();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkTrueClientRect();
			if(m_UiLayout != null)
			{
				m_UiLayout.Location = TrueClientRect.Location;
				m_UiLayout.Size = TrueClientRect.Size;
			}
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
