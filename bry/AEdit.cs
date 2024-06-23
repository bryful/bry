using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;

using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting; // ← 名前空間
using ICSharpCode.AvalonEdit.Highlighting.Xshd; // ← 名前空間
using System.IO;
using System.Reflection;
using ICSharpCode.AvalonEdit.Document;

namespace bry
{
	public class AEdit : System.Windows.Forms.Control
	{
		private TextEditor m_editor = new TextEditor();
		[Category("Editor"),Browsable(false)]
		public TextEditor editor
		{
			get { return m_editor; }
		}

		private ElementHost host = new ElementHost();

		[Category("Editor")]
		public bool ShowLineNumbers
		{
			get { return m_editor.ShowLineNumbers; }
			set { m_editor.ShowLineNumbers = value;}
		}
		[Category("Editor")]
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get { return m_editor.HorizontalScrollBarVisibility; }
			set { m_editor.HorizontalScrollBarVisibility = value; }
		}
		[Category("Editor")]
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get { return m_editor.VerticalScrollBarVisibility; }
			set { m_editor.VerticalScrollBarVisibility = value; }
		}
		[Category("Editor"), Browsable(false)]
		public TextEditorOptions Options
		{
			get { return m_editor.Options; }
			set
			{
				m_editor.Options = value;
			}
		}
		[Category("EditorOptions")]
		public bool ShowEndOfLine
		{
			get { return m_editor.Options.ShowEndOfLine; }
			set
			{
				m_editor.Options.ShowEndOfLine = value;
			}
		}
		[Category("EditorOptions")]
		public bool ShowSpaces
		{
			get { return m_editor.Options.ShowSpaces; }
			set
			{
				m_editor.Options.ShowSpaces = value;
			}
		}
		[Category("EditorOptions")]
		public bool ShowTabs
		{
			get { return m_editor.Options.ShowTabs; }
			set
			{
				m_editor.Options.ShowTabs = value;
			}
		}
		[Category("EditorOptions")]
		public bool ShowBoxForControlCharacters
		{
			get { return m_editor.Options.ShowBoxForControlCharacters; }
			set
			{
				m_editor.Options.ShowBoxForControlCharacters = value;
			}
		}
		[Category("EditorOptions")]
		public bool HighlightCurrentLine
		{
			get { return m_editor.Options.HighlightCurrentLine; }
			set
			{
				m_editor.Options.HighlightCurrentLine = value;
			}
		}
		[Category("EditorOptions")]
		public int IndentationSize
		{
			get { return m_editor.Options.IndentationSize; }
			set
			{
				m_editor.Options.IndentationSize = value;
			}
		}
		[Category("EditorOptions")]
		public bool ShowColumnRuler
		{
			get { return m_editor.Options.ShowColumnRuler; }
			set
			{
				m_editor.Options.ShowColumnRuler = value;
			}
		}
		[Category("EditorOptions")]
		public int ColumnRulerPosition
		{
			get { return m_editor.Options.ColumnRulerPosition; }
			set
			{
				m_editor.Options.ColumnRulerPosition = value;
			}
		}
		[Category("Editor"), Browsable(false)]
		public TextDocument Document
		{
			get { return m_editor.Document; }
			set
			{
				m_editor.Document = value;
			}
		}
		public new string Text
		{
			get { return m_editor.Text; }
			set {
				m_editor.Text = value;
			}	
		}
		public new Font Font
		{
			get { return base.Font; }
			set 
			{ 
				base.Font = value;
				host.Font = value;
				m_editor.FontFamily = new  System.Windows.Media.FontFamily( value.FontFamily.Name);
				m_editor.FontSize = value.Size;

			}
		}
		public AEdit()
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			using (Stream resourceStream = thisAssembly.GetManifestResourceStream("bry.JavaScript-Mode.xshd"))
			{
				var reader = new System.Xml.XmlTextReader(resourceStream);
				var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);

				m_editor.SyntaxHighlighting = definition;
			}


			this.Size = new Size(200,200);
			host.SetBounds(0,0,200,200);
			host.Child = m_editor;
			this.Controls.Add(host);

		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			host.SetBounds(0,0,this.Width,this.Height);
		}

	}
}
