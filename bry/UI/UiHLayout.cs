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
	public class UiHLayout : UiLayout
	{
		[ScriptUsage(ScriptAccess.None)]
		public UiHLayout() 
		{
			this.Dock = System.Windows.Forms.DockStyle.Fill;
		}
		protected override void ChkLayout()
		{
			if (this.Controls.Count <= 0) return;
			if (NowChkLayout==true) return;
			NowChkLayout = true;
			//実際の範囲
			Rectangle tr = new Rectangle(
				Padding.Left +Margin.Left,
				Padding.Top +Margin.Top,
				this.Width - (Padding.Left + Padding.Right+ Margin.Left + Margin.Right),
				this.Height - (Padding.Top + Padding.Bottom+ Margin.Top + Margin.Bottom)
				);


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
					if(uc.SizePolicyHor == SizePolicy.Fixed) 
					{
						fc++;
						wfix += uc.Width;
					}else if (uc.SizePolicyHor == SizePolicy.Expanding)
					{
						ec++;
					}
				}
			}
			//可変幅の計算
			int wExpanding = 0;
			if( ec > 0)
			{
				wExpanding = (tr.Width - wfix)/ ec;
				if (wExpanding < 0) wExpanding = 0;
			}
			//実際の大きさ変更

			int left = Padding.Left + Margin.Left;

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i] is UiControl)
				{
					UiControl uc = (UiControl)this.Controls[i];
					if (uc == null) continue;
					int w = uc.Width;
					if (w > tr.Width) w = tr.Width;
					int h = uc.Height;
					if (h > tr.Height) w = tr.Height;
					//left
					int y = 0; 
					if (uc.SizePolicyHor == SizePolicy.Fixed)
					{
						if (uc.SizePolicyVer == SizePolicy.Fixed)
						{
							y = (tr.Height-h)/2 + tr.Top;
						}
						else
						{
							y = tr.Top;
							h = tr.Height;
						}
					}
					else if (uc.SizePolicyHor == SizePolicy.Expanding)
					{
						w = wExpanding;
						if (uc.SizePolicyVer == SizePolicy.Fixed)
						{
							y = (tr.Height - h)/2 + tr.Top;
						}
						else
						{
							y = tr.Top;
							h = tr.Height;
						}
					}
					uc.Location = new Point(left, y);
					uc.Size = new Size(w, h);
					left += w;
				}
			}
			NowChkLayout = false;
		}
	}
}
