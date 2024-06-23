using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace bry
{
	public partial class RefForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public new Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				findBox.Font = value;
				refList.Font = value;
				ChkSize();
			}
		}
		public RefForm()
		{
			InitializeComponent();
			ChkSize();
		}
		public void ChkSize()
		{
			this.SuspendLayout();

			findBox.Location= new Point(0,0);
			findBox.Size = new Size(this.ClientSize.Width, findBox.Height);
			refList.Location = new Point(0, findBox.Height + 3);
			refList.Size = new Size(this.ClientSize.Width, this.Height - findBox.Height - 3);

			this.ResumeLayout();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkSize();
		}
	}
}
