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
	public class UiLabel : UiControl
	{
		public UiLabel() 
		{
			
			LineAlignment = StringAlignment.Center;
			Alignment = StringAlignment.Near;
			this.SetStyle(
ControlStyles.DoubleBuffer |
ControlStyles.UserPaint |
ControlStyles.AllPaintingInWmPaint |
ControlStyles.SupportsTransparentBackColor,
true);
			this.UpdateStyles();
			base.BackColor = Color.Transparent;
		}
		[ScriptUsage(ScriptAccess.None)]
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			using (SolidBrush sb = new SolidBrush(BackColor))
			{
				Rectangle rct = new Rectangle(
					Margin.Left,
					Margin.Top,
					this.Width - (Margin.Left + Margin.Right) - 1,
					this.Height - (Margin.Top + Margin.Bottom) - 1
					);
				
				sb.Color = ForeColor;
				g.DrawString(this.Text, this.Font, sb, rct, m_StringFormat);


			}
		}
	}
}
