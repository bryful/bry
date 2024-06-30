using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

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
		public void SetText(string s)
		{
			aEdit1.SetText(s);
		}
		public void SetSInfo(SInfo[] a)
		{
			aEdit1.SetSInfo(a);
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
			ChkSize();
		}
		public void ChkSize()
		{
			aEdit1.Location = new Point(
				menuStrip1.Left+2,
				menuStrip1.Height + menuStrip1.Top+1
				);
			aEdit1.Size = new Size(
				this.ClientRectangle.Width-4,
				this.ClientRectangle.Height - 
				(menuStrip1.Top + menuStrip1.Height+2)
				);
		}
		protected override void OnResize(EventArgs e)
		{
			ChkSize();
			base.OnResize(e);
		}



	}
}
