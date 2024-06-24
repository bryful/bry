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
		protected override string GetPersistString()
		{
			return "RefForm";
		}
		public  Font RefFont
		{
			get { return findBox.Font; }
			set
			{
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

			findBox.Location= new Point(5,5);
			findBox.Size = new Size(this.ClientSize.Width-10, findBox.Height);
			refList.Location = new Point(5, findBox.Height + 10);
			refList.Size = new Size(this.ClientSize.Width-10, this.Height - findBox.Height - 15);

			this.ResumeLayout();
		}
		protected override void OnResize(EventArgs e)
		{
			ChkSize();
			base.OnResize(e);
			ChkSize();
		}
	}
}
