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
		}
		protected override void ChkLayout()
		{
			if (NowChkLayout) return;
			if (this.Controls.Count <= 0) return;
			NowChkLayout = true;
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
				allFixed = (rct.Height - hfix)/(fc+1);
			}

			int y = rct.Top+ allFixed;
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
					y += uc.Height+ allFixed;
				}
			}
			NowChkLayout = false;

		}
	}
}
