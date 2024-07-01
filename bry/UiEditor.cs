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
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
namespace bry
{
	public class UiEditor :UiControl
	{
		private AEdit m_Editor = new AEdit();
		public AEdit Editor { get { return m_Editor; } }
		public void SetText(string s)
		{
			m_Editor.SetText(s);
		}
		public void SetSInfo(SInfo[] a)
		{
			m_Editor.SetSInfo(a);
		}
		public string FontFamily
		{
			get
			{
				//System.Windows.Media.FontFamily
				return m_Editor.editor.FontFamily.Source;
			}
			set
			{
				m_Editor.editor.FontFamily = new System.Windows.Media.FontFamily(value);
			}
		}
		public new double FontSize
		{
			get { return m_Editor.editor.FontSize; }
			set
			{
				m_Editor.editor.FontSize = value;
			}
		}
		public new string Text
		{
			get { return m_Editor.editor.Text; }
			set { m_Editor.editor.Text = value; }
		}
		[Category("Editor"), Browsable(false)]
		public TextDocument Document
		{
			get { return m_Editor.Document; }
			set
			{
				m_Editor.Document = value;
			}
		}
		[Category("Editor"), Browsable(false)]
		public TextArea TextArea
		{
			get { return m_Editor.TextArea; }
		}
		public void AppendText(string s)
		{
			m_Editor.AppendText(s);
		}
		public UiEditor()
		{
			m_Editor.Dock = DockStyle.Fill;
			this.SizePolicyHor= SizePolicy.Expanding;
			this.SizePolicyVer = SizePolicy.Expanding;
			this.Controls.Add(m_Editor);
		}
	}
}
