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
	public class UiTextBox :UiControl
	{
		private TextBox m_TextBox = new TextBox();
		public new string Text
		{
			get { return m_TextBox.Text; }
			set
			{
				base.Text = value;
				m_TextBox.Text = value;
			}
		}
		[Browsable(true)]
		public new System.Drawing.Font Font
		{
			get { return base.Font; }
			set 
			{ 
				base.Font = value;
				m_TextBox.Font = value;
				ChkSize();
			}
		}
		[Category("UI"), Browsable(true)]
		public new SizePolicy SizePolicyVer
		{
			get { return base.SizePolicyVer; }
			set
			{
				base.SizePolicyVer = SizePolicy.Fixed;
			}
		}
		// **************************************************************
		[Category("UI"), Browsable(true)]
		public new float FontSize
		{
			get { return base.FontSize; }
			set
			{
				base.FontSize = value;
				ChkSize();
				this.Invalidate();
			}
		}
		// ************************************************
		public UiTextBox()
		{
			base.SizePolicyVer = SizePolicy.Fixed;
			m_TextBox.Size = new Size(100, 25);
			m_TextBox.Location = new Point(Margin.Left, Margin.Top);
			base.Size = new Size(
				m_TextBox.Width + Margin.Left+Margin.Right,
				m_TextBox.Height + Margin.Top + Margin.Bottom
				);
			this.Controls.Add( m_TextBox );
		}
		// ************************************************
		protected override void OnResize(EventArgs e)
		{
			ChkSize();
			base.OnResize(e);
		}
		private void ChkSize()
		{
			m_TextBox.Size = new Size(
				this.Width - (Margin.Left + Margin.Right),
				this.Height - (Margin.Top + Margin.Bottom)
				);
			this.Height = m_TextBox.Height + (Margin.Top + Margin.Bottom);
			m_TextBox.Location = new Point(
				Margin.Left, Margin.Top);
		}
		
	}
}
