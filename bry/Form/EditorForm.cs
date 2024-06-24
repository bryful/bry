using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;

namespace bry
{
	public partial class EditorForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public TextEditor editor
		{
			get { return aEdit1.editor; }
		}
		public new Font Font
		{
			get { return base.Font; }
			set 
			{ 
				base.Font = value; 
				aEdit1.Font = value;
			}
		}
		public EditorForm()
		{
			InitializeComponent();
			ChkSize();
		}
		private void ChkSize()
		{
			int h = menuStrip1.Bottom;
			aEdit1.Location = new Point(0,h);
			aEdit1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height -h);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkSize();
		}
	}
}
