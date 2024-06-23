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
		}
	}
}
