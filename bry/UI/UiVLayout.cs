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
	public class UiVLayout : UiLayout
	{
		[ScriptUsage(ScriptAccess.None)]
		public UiVLayout() 
		{
			this.Dock = System.Windows.Forms.DockStyle.Fill;
		}
		protected override void ChkLayout()
		{

			if (this.Controls.Count <= 0) return;
			if (NowChkLayout == true) return;
			NowChkLayout = true;
			//実際の範囲
			Rectangle tr = new Rectangle(
				Padding.Left + Margin.Left,
				Padding.Top + Margin.Top,
				this.Width - (Padding.Left + Padding.Right + Margin.Left + Margin.Right),
				this.Height - (Padding.Top + Padding.Bottom + Margin.Top + Margin.Bottom)

				);


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
			if (ec > 0)
			{
				hExpanding = (tr.Height - hfix) / ec;
				if (hExpanding < 0) hExpanding = 0;
			}
			//実際の大きさ変更

			int top = Padding.Top+Margin.Top;

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;
					int w = uc.Width;
					if (w > tr.Width) w = tr.Width;
					int h = uc.Height;
					if (h > tr.Height) h = tr.Height;
					//top
					int x = 0;
					if (uc.SizePolicyVer == SizePolicy.Fixed)
					{
						if (uc.SizePolicyHor == SizePolicy.Fixed)
						{
							x = (tr.Width - w)/2 + tr.Left;
						}
						else
						{
							x = tr.Left;
							w = tr.Width;
						}
					}
					else if (uc.SizePolicyVer == SizePolicy.Expanding)
					{
						h = hExpanding;
						if (uc.SizePolicyHor == SizePolicy.Fixed)
						{
							x = (tr.Width - w)/2 + tr.Left;
						}
						else
						{
							x= tr.Left;
							w = tr.Width;
						}
					}
					uc.Location = new Point(x, top);
					uc.Size = new Size(w, h);
					top += h;
				}
			}
			NowChkLayout = false;
		}
	}
}
