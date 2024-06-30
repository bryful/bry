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
		private LayoutOrientation m_LayoutOrientation = LayoutOrientation.Vertical;
		public LayoutOrientation LayoutOrientation
		{
			get { return (m_LayoutOrientation); }
			set
			{
				m_LayoutOrientation =value;
				ChkLayout();
				this.Invalidate();
			}
		}
		[ScriptUsage(ScriptAccess.None)]
		public UiLayout() 
		{

		}
		// ***********************************************
		[ScriptUsage(ScriptAccess.None)]
		protected override void ChkLayout()
		{
			if (this.Controls.Count <= 0) return;
			if (NowChkLayout) return;
			NowChkLayout = true;

			if(m_LayoutOrientation== LayoutOrientation.Vertical)
			{
				ChkLayoutV();
			}
			else
			{
				ChkLayoutH();
			}

			NowChkLayout = false;
		}
		// ***********************************************
		[ScriptUsage(ScriptAccess.None)]
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ScanMinSize();
			ChkLayout();
		}
		// ***********************************************
		public void add(UiControl control)
		{
			this.Controls.Add(control);
			ChkLayout();
		}
		
		// ****************************************************
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			ScanMinSize();
			ChkLayout();

		}
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			ScanMinSize();
			ChkLayout();
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
		// ****************************************************************
		private void ChkLayoutH()
		{

			Rectangle rct = TrueClientRect;


			//固定幅の合計
			int wfix = 0;
			int fc = 0;
			int ec = 0;

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;
					if (uc.SizePolicyHor == SizePolicy.Fixed)
					{
						fc++;
						wfix += uc.Width;
					}
					else if (uc.SizePolicyHor == SizePolicy.Expanding)
					{
						ec++;
					}
				}
			}
			//可変幅の計算
			int wExpanding = 0;
			int allFixed = 0;
			if (ec > 0)
			{
				wExpanding = (rct.Width - wfix) / ec;
				if (wExpanding < 0) wExpanding = 0;
			}
			else
			{
				allFixed = (rct.Width - wfix) / (fc + 1);
			}

			int x = rct.Left + allFixed;
			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;
					if (uc.SizePolicyHor == SizePolicy.Fixed)
					{

					}
					else if (uc.SizePolicyHor == SizePolicy.Expanding)
					{
						uc.Width = wExpanding;
					}
					if (uc.SizePolicyVer == SizePolicy.Fixed)
					{
						uc.SetVerCenter();
					}
					else
					{
						uc.SetVerFill();
					}
					uc.Left = x;
					x += uc.Width + allFixed;
				}
			}

		}
		private void ChkLayoutV()
		{
			Rectangle rct = TrueClientRect;


			//固定幅の合計
			int hfix = 0;
			int fc = 0;
			int ec = 0;

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;
					if (uc.SizePolicyVer == SizePolicy.Fixed)
					{
						fc++;
						hfix += uc.Height;
					}
					else if (uc.SizePolicyVer == SizePolicy.Expanding)
					{
						ec++;
					}
				}
			}
			//可変幅の計算
			int hExpanding = 0;
			int allFixed = 0;
			if (ec > 0)
			{
				hExpanding = (rct.Height - hfix) / ec;
				if (hExpanding < 0) hExpanding = 0;
			}
			else
			{
				allFixed = (rct.Height - hfix) / (fc + 1);
			}

			int y = rct.Top + allFixed;
			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;

					if (uc.SizePolicyVer == SizePolicy.Fixed)
					{

					}
					else if (uc.SizePolicyVer == SizePolicy.Expanding)
					{
						uc.Height = hExpanding;
					}
					if (uc.SizePolicyHor == SizePolicy.Fixed)
					{
						uc.SetHorCenter();
					}
					else
					{
						uc.SetHorFill();
					}
					uc.Top = y;
					y += uc.Height + allFixed;
				}
			}

		}
	
		public void ScanMinSize()
		{
			if (this.Controls.Count == 0)
			{
				base.MinimumSize = new Size(0, 0);
				return;
			}
			int w = 0; int h=0;
			for (int i = 0;i < this.Controls.Count;i++)
			{
				Control c = this.Controls[i];
				if (w < c.MinimumSize.Width) w = c.MinimumSize.Width;
				if (h < c.MinimumSize.Height) w = c.MinimumSize.Height;
			}
			if (w != 0) w += Margin.Left + Margin.Right;
			if (h != 0) h += Margin.Top + Margin.Bottom;
			base.MinimumSize = new Size(w, h);
		}
	}
}
