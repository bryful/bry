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
			m_TrueClientRect = new Rectangle(l, t, w, h);
		}
		[ScriptUsage(ScriptAccess.None)]
		public UiForm()
		{
			InitializeComponent();
			ChkTrueClientRect();
		}
		[BryScript]
		[ScriptUsage(ScriptAccess.Full)]
		public void clear()
		{
			m_UiLayout = null;
			this.Controls.Clear();
		}
		private UiLayout m_UiLayout = null;
		[BryScript]
		public UiLayout UiLayout
		{
			get { return m_UiLayout; }
		}
		[BryScript]
		public void add(UiControl control)
		{
			if(m_UiLayout==null)
			{
				addLayout();
			}
			m_UiLayout.add(control);
		}
		[BryScript]
		public UiLayout addLayout(LayoutOrientation lo = LayoutOrientation.Vertical)
		{
			UiLayout ly = newLayout(
				lo, 
				TrueClientRect.Width,
				TrueClientRect.Height,
				SizePolicy.Expanding,
				SizePolicy.Expanding
				);
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
		[BryScript]
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
		[BryScript]
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
		[BryScript]
		public UiLayout newFormLayout(
			LayoutOrientation orientation = LayoutOrientation.Vertical
			)
		{
			UiLayout ctrl = new UiLayout();
			ctrl.Name = CanUseName("formlayout");
			ctrl.Text = "formlayout";
			ctrl.Size = m_TrueClientRect.Size;
			ctrl.Location = m_TrueClientRect.Location;
			ctrl.SizePolicyHor = SizePolicy.Expanding;
			ctrl.SizePolicyVer = SizePolicy.Expanding;
			ctrl.LayoutOrientation = orientation;
			return ctrl;
		}

		[BryScript]
		public UiLayout newLayout(
			LayoutOrientation lo = LayoutOrientation.Vertical,
			int w = 200,
			int h = 200,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiLayout ctrl = new UiLayout();
			ctrl.Name = CanUseName("layout");
			ctrl.Text = ctrl.Name;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			ctrl.LayoutOrientation = lo;
			return ctrl;
		}
		[BryScript]
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
		[BryScript]
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
		[BryScript]
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

		[BryScript]
		public Object listupControl()
		{
			List<UiControl> list = new List<UiControl>();

			list = ListupControlSub(list,this.Controls);

			Object[] ret = new Object[list.Count];
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					ret[i] = (Object)list[i];
				}
			}
			return ScriptEngine.Current.Script.Array.from(ret);
		}
		private List<UiControl> ListupControlSub(List<UiControl> lst,Control.ControlCollection cc)
		{

			if(cc.Count > 0)
			{
				for (int i = 0;i< cc.Count; i++)
				{
					if (cc[i] is UiControl)
					{
						UiControl ccc = (UiControl)cc[i];
						lst.Add(ccc);
						if (ccc is UiLayout)
						{
							lst = ListupControlSub(lst,ccc.Controls);

						}
					}
				}
			}
			return lst;
		}
		public object[] aryFrom(UiControl[] a)
		{
			var ret = new Object[a.Length];
			if (a.Length > 0)
			{
				for (int i = 0; i < a.Length; i++)
				{
					ret[i] = a[i];
				}
			}
			return ret;
		}

	}
}
