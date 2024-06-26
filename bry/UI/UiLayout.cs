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
	public class UiLayout : UiControl
	{
		private bool m_IsDrawLayout = false;
		public bool IsDrawLayout
		{
			get { return m_IsDrawLayout; }
			set
			{
				m_IsDrawLayout = value;
				this.Invalidate();
			}
		}
		[ScriptUsage(ScriptAccess.None)]
		public UiLayout() 
		{
			this.Dock = System.Windows.Forms.DockStyle.Fill;
		}
		// ***********************************************
		
		// ***********************************************
		[ScriptUsage(ScriptAccess.None)]
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkLayout();

		}
		// ***********************************************
		public UiBtn addBtn(string name,
			int w = 150,
			int h = 24,
			SizePolicy hp = SizePolicy.Expanding, 
			SizePolicy vp = SizePolicy.Expanding) 
		{
			UiBtn ctrl = new UiBtn();
			ctrl.Name = name;
			ctrl.Text = name;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			this.Controls.Add(ctrl);
			return ctrl;
		}
		// ***********************************************
		public UiSpace addSpace(string name,
			int w = 150,
			int h = 24,
			SizePolicy hp = SizePolicy.Expanding,
			SizePolicy vp = SizePolicy.Expanding)
		{
			UiSpace ctrl = new UiSpace();
			ctrl.Name = name;
			ctrl.Text = name;
			ctrl.Size = new Size(w, h);
			ctrl.SizePolicyHor = hp;
			ctrl.SizePolicyVer = vp;
			this.Controls.Add(ctrl);
			return ctrl;
		}
		// ****************************************************
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			if (e.Control is UiControl) 
			{
				ChkLayout() ;
			}

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (m_IsDrawLayout)
			{
				using (Pen p = new Pen(ForeColor, 1))
				{
					e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
				}
			}
		}
	}
}
