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
using ICSharpCode.AvalonEdit.Utils;

using ICSharpCode.AvalonEdit.Highlighting; // ← 名前空間
using ICSharpCode.AvalonEdit.Highlighting.Xshd; // ← 名前空間
using System.IO;
using System.Reflection;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.CodeCompletion;


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
		private Font m_font = new Font("System",9);
		public new Font Font
		{
			get { return m_font; }
			set 
			{
				m_font = value;
				m_editor.FontFamily = new  System.Windows.Media.FontFamily( value.FontFamily.Name);
				m_editor.FontSize = value.Size;

			}
		}
		private CompletionWindow completionWindow;
		public AEdit()
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			using (Stream resourceStream = thisAssembly.GetManifestResourceStream("bry.Avalon.JavaScript-Mode.xshd"))
			{
				var reader = new System.Xml.XmlTextReader(resourceStream);
				var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);

				m_editor.SyntaxHighlighting = definition;
			}


			this.Size = new Size(200,200);
			host.SetBounds(0,0,200,200);
			host.Child = m_editor;
			this.Controls.Add(host);

			m_editor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
			m_editor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

		}
		private void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
		{
			MyCompletionData[] Items = FindChar(e.Text);
			if(Items.Length>0)
			{
				completionWindow = new CompletionWindow(m_editor.TextArea);
				IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                foreach (var item in Items)
                {
					data.Add(item);
				}
				completionWindow.Show();

				// ウインドウを閉じたときの処理
				completionWindow.Closed += delegate { completionWindow = null; };
			}
			/*
			if (e.Text == ".") // ピリオドを入力したとき
			{
				// CodeCompletion 用ウインドウを開く
				completionWindow = new CompletionWindow(m_editor.TextArea);
				IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
				data.Add(new MyCompletionData("aaB"));
				data.Add(new MyCompletionData("aBB"));
				data.Add(new MyCompletionData("aaaB"));
				completionWindow.Show();

				// ウインドウを閉じたときの処理
				completionWindow.Closed += delegate { completionWindow = null; };
			*/
		}

		//---------------------------------------------------------------------------------------
		private void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
		{
			if (e.Text.Length > 0 && completionWindow != null)
			{
				if (!char.IsLetterOrDigit(e.Text[0]))
				{
					// CompletionWindow を開いているときに、英数字以外を入力しても文字を入力する
					completionWindow.CompletionList.RequestInsertion(e);
				}
			}

			// e.Handled = true; を設定してはならない
		}
		private void ChkSize()
		{
			host.SetBounds(0, 0, this.Width, this.Height);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkSize();
		}
		public void SetText(string s)
		{
			if (s == "") return;
			if (m_editor.SelectionLength==0)
			{
				m_editor.Document.Insert(m_editor.SelectionStart, s);
			}
			else
			{
				m_editor.TextArea.Selection.ReplaceSelectionWithText(s);
			}
		}
		private MyCompletionData[] myCompletionDatas = new MyCompletionData[0];

		public void SetSInfo(SInfo[] a)
		{
			myCompletionDatas = new MyCompletionData[0];
			List<MyCompletionData> ds = new List<MyCompletionData> ();

			if(a.Length > 0)
			{
				string[] sa = ScriptInfo.SInfoToList(a);
				foreach (string s in sa)
				{
					ds.Add(new MyCompletionData(s));
				}
			}
			myCompletionDatas = ds.ToArray();
		}
		private MyCompletionData[] FindChar(string s)
		{
			List<MyCompletionData> ret = new List<MyCompletionData>();
			if (myCompletionDatas.Length == 0) return ret.ToArray();
			if (s=="") return ret.ToArray();
			foreach(var dat in myCompletionDatas)
			{
				if (String.Compare(dat.Text.Substring(0,1),s,true)==0)
				{
					ret.Add(dat);
				}
			}
			return ret.ToArray();
		}
	}
	public class MyCompletionData : ICompletionData
	{
		public MyCompletionData(string text)
		{
			Text = text;
		}

		public System.Windows.Media.ImageSource Image { get { return null; } }
		public string Text { get; }

		// Use this property if you want to show a fancy UIElement in the list.
		public object Content => Text;

		public object Description => "Description for " + Text;
		public double Priority { get; set; } = 0;
		public void Complete(TextArea textArea, ISegment completionSegment,
			EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, Text);
		}
	}
}
