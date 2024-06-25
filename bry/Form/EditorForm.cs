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
		

		public int Index { get; set; } = 0;
		protected override string GetPersistString()
		{
			return $"EditorFrom_{Index}";
		}
		public TextEditor editor
		{
			get { return aEdit1.editor; }
		}
		public string FontFamily
		{
			get 
			{
				//System.Windows.Media.FontFamily
				return aEdit1.editor.FontFamily.Source; 
			}
			set 
			{
				aEdit1.editor.FontFamily = new System.Windows.Media.FontFamily(value);
			}
		}
		public double FontSize
		{
			get { return aEdit1.editor.FontSize; }
			set
			{
				aEdit1.editor.FontSize = value;
			}
		}
		public EditorForm()
		{
			InitializeComponent();
			//ChkSize();
		}
		
		
		

		private void EditorForm_DockStateChanged(object sender, EventArgs e)
		{
			//this.editor.Text = DockAreas.ToString();
		}
	}
}
