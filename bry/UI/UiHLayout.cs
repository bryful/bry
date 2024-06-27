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
		}
		protected override void ChkLayout()
		{
			if (NowChkLayout) return;
			if (this.Controls.Count <= 0) return;
			NowChkLayout = true;
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
			int allFixed = 0;
			if( ec > 0)
			{
				wExpanding = (rct.Width - wfix)/ ec;
				if (wExpanding < 0) wExpanding = 0;
			}
			else
			{
				allFixed = (rct.Width - wfix) / (fc + 1);
			}

			int x = rct.Left+ allFixed;
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
			NowChkLayout = false;

		}
	}
}
