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
		#region props
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
		[Category("Editor"), Browsable(false)]
		public TextArea TextArea
		{
			get { return m_editor.TextArea; }
		}
		public new string Text
		{
			get { return m_editor.Text; }
			set {
				m_editor.Text = value;
			}	
		}
		public void AppendText(string s)
		{
			m_editor.AppendText(s);
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
		#endregion
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
			m_editor.KeyDown += M_editor_KeyDown;

		}



		private string GetWord(string s, int pos)
		{
			string ret = "";

			if (pos > s.Length) pos = s.Length;
			for (int i = pos - 1; i >= 0; i--)
			{
				if(i<0) break;
				string c = s.Substring(i,1);
				if((c ==" ")|| (c == "\t") || (c == "\n") || (c == "\r")
					|| (c == "\"") || (c == "\'") || (c == ")"))
				{
					break;
				}
				ret = c + ret;
			}
			return ret;
		}
		// ***********************************************************
		private void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
		{
			if (e.Text == ".") // ピリオドを入力したとき
			{
				int offset = m_editor.TextArea.Caret.Offset;
				string wd = GetWord(m_editor.Document.Text, offset-1);
				if (IsCategory(wd))
				{
					completionWindow = new CompletionWindow(m_editor.TextArea);
					SetCompData(completionWindow, wd);
					completionWindow.Show();
					completionWindow.Closed += delegate { completionWindow = null; };
				}
			}
		}
		private void M_editor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (((e.Key & Key.Space) == Key.Space)&&((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control))
			{ 
				e.Handled = true;
				completionWindow = new CompletionWindow(m_editor.TextArea);
				SetCompDataNoDot(completionWindow);
				completionWindow.Show();
				completionWindow.Closed += delegate { completionWindow = null; };
			}
		}

		// //////
		private void SetCompData(CompletionWindow cw,string cat)
		{
			if (cat == "") return;
            IList<ICompletionData> data = cw.CompletionList.CompletionData;

			foreach(SInfo sInfo in SInfoList)
			{
				if (string.Compare(sInfo.Category, cat, true) == 0)
				{
					string cd = sInfo.Name;
					if (sInfo.Kind == SInfoKind.Method) cd += "()";
					data.Add(new MyCompletionData(cd));
				}
			}

		}
		private void SetCompDataNoDot(CompletionWindow cw)
		{
			IList<ICompletionData> data = cw.CompletionList.CompletionData;

			foreach (SInfo sInfo in SInfoList)
			{
				if (sInfo.Category=="")
				{
					string cd = sInfo.Name;
					if (sInfo.Kind == SInfoKind.Method) cd += "()";
					data.Add(new MyCompletionData(cd));
				}
			}
			if (SInfoCategorys.Length > 0)
			{
				foreach (string cat in SInfoCategorys)
				{
					data.Add(new MyCompletionData(cat));
				}
			}
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
		// ***********************************************************
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
		private SInfo[] SInfoList = new SInfo[0];
		private string[] SInfoCategorys = new string[0];

		private List<string> AppendList(List<string>lst, string s)
		{
			if (s=="") return lst;

			bool isIn=false;
			foreach(var ss in lst)
			{
				if (String.Compare(ss,s,true)==0)
				{
					isIn = true; break; 
				}
			}
			if (isIn == false) lst.Add(s);
			return lst;
		}
		private bool IsCategory(string cat)
		{
			bool ret = false;
			if (cat == "") return ret;
			if(SInfoCategorys.Length>0)
			{
				foreach(var s in SInfoCategorys)
				{
					if (String.Compare(s,cat,true)==0)
					{
						ret = true; break;
					}
				}
			}
			return ret;
		}
		public void SetSInfo(SInfo[] a)
		{
			SInfoList = a;
			List<string> list = new List<string>();
			foreach (SInfo s in SInfoList)
			{
				list =AppendList(list,s.Category);
			}
			list.Sort();
			SInfoCategorys = list.ToArray();
		}
		private MyCompletionData[] FindChar(string s)
		{
			List<MyCompletionData> ret = new List<MyCompletionData>();
			/*
			if (myCompletionDatas.Length == 0) return ret.ToArray();
			if (s=="") return ret.ToArray();
			foreach(var dat in myCompletionDatas)
			{
				if (String.Compare(dat.Text.Substring(0,1),s,true)==0)
				{
					ret.Add(dat);
				}
			}*/
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
