using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting; // ← 名前空間
using ICSharpCode.AvalonEdit.Highlighting.Xshd; // ← 名前空間


namespace bry
{
	public class AEdit : System.Windows.Forms.Control
	{
		#region props
		private TextEditor m_editor = new TextEditor();
		[BryScript]
		[Category("Editor"),Browsable(false)]
		public TextEditor editor
		{
			get { return m_editor; }
		}

		private ElementHost host = new ElementHost();

		[BryScript]
		[Category("Editor")]
		public bool ShowLineNumbers
		{
			get { return m_editor.ShowLineNumbers; }
			set { m_editor.ShowLineNumbers = value;}
		}
		[BryScript]
		[Category("Editor")]
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get { return m_editor.HorizontalScrollBarVisibility; }
			set { m_editor.HorizontalScrollBarVisibility = value; }
		}
		[BryScript]
		[Category("Editor")]
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get { return m_editor.VerticalScrollBarVisibility; }
			set { m_editor.VerticalScrollBarVisibility = value; }
		}
		[BryScript]
		[Category("Editor"), Browsable(false)]
		public TextEditorOptions Options
		{
			get { return m_editor.Options; }
			set
			{
				m_editor.Options = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool ShowEndOfLine
		{
			get { return m_editor.Options.ShowEndOfLine; }
			set
			{
				m_editor.Options.ShowEndOfLine = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool ShowSpaces
		{
			get { return m_editor.Options.ShowSpaces; }
			set
			{
				m_editor.Options.ShowSpaces = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool ShowTabs
		{
			get { return m_editor.Options.ShowTabs; }
			set
			{
				m_editor.Options.ShowTabs = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool ShowBoxForControlCharacters
		{
			get { return m_editor.Options.ShowBoxForControlCharacters; }
			set
			{
				m_editor.Options.ShowBoxForControlCharacters = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool HighlightCurrentLine
		{
			get { return m_editor.Options.HighlightCurrentLine; }
			set
			{
				m_editor.Options.HighlightCurrentLine = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public int IndentationSize
		{
			get { return m_editor.Options.IndentationSize; }
			set
			{
				m_editor.Options.IndentationSize = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public bool ShowColumnRuler
		{
			get { return m_editor.Options.ShowColumnRuler; }
			set
			{
				m_editor.Options.ShowColumnRuler = value;
			}
		}
		[BryScript]
		[Category("EditorOptions")]
		public int ColumnRulerPosition
		{
			get { return m_editor.Options.ColumnRulerPosition; }
			set
			{
				m_editor.Options.ColumnRulerPosition = value;
			}
		}
		[BryScript]
		[Category("Editor"), Browsable(false)]
		public TextDocument Document
		{
			get { return m_editor.Document; }
			set
			{
				m_editor.Document = value;
			}
		}
		[BryScript]
		[Category("Editor"), Browsable(false)]
		public TextArea TextArea
		{
			get { return m_editor.TextArea; }
		}
		[BryScript]
		public new string Text
		{
			get { return m_editor.Text; }
			set {
				m_editor.Text = value;
			}	
		}
		[BryScript]
		public void AppendText(string s)
		{
			m_editor.AppendText(s);
		}
		private Font m_font = new Font("System",9);
		[BryScript]
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
					|| (c == "\"") || (c == "\'") || (c == ",")
					|| (c == ")") || (c == "=") || (c == "."))
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
				if (wd != "")
				{
					completionWindow = new CompletionWindow(m_editor.TextArea);
					if (IsCategory(wd))
					{
						SetCompDataDotCat(completionWindow, wd);
					}
					else
					{
						SetCompDataDotNoCat(completionWindow);
					}
					completionWindow.Show();
					completionWindow.Closed += delegate { completionWindow = null; };
				}
			}
		}
		private void M_editor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
			{
				if (e.Key  == Key.Space)
				{
					e.Handled = true;
					completionWindow = new CompletionWindow(m_editor.TextArea);
					SetCompDataOne(completionWindow);
					completionWindow.Show();
					completionWindow.Closed += delegate { completionWindow = null; };
				}
			}

		}

		// //////
		private void SetCompDataDotCat(CompletionWindow cw,string cat)
		{
			if (cat == "") return;
            IList<ICompletionData> data = cw.CompletionList.CompletionData;

			foreach(SInfo sInfo in SInfoListDotCat)
			{
				if (string.Compare(sInfo.Category, cat, true) == 0)
				{
					string cd = sInfo.Name;
					if (sInfo.Kind == SInfoKind.Method) cd += "()";
					data.Add(new MyCompletionData(cd));
				}
			}

		}
		private void SetCompDataDotNoCat(CompletionWindow cw)
		{
			IList<ICompletionData> data = cw.CompletionList.CompletionData;

			foreach (SInfo sInfo in SInfoListDotNoCat)
			{
				string cd = sInfo.Name;
				if (sInfo.Kind == SInfoKind.Method) cd += "()";
				data.Add(new MyCompletionData(cd));
			}

		}
		private void SetCompDataOne(CompletionWindow cw)
		{
			IList<ICompletionData> data = cw.CompletionList.CompletionData;

			foreach (SInfo sInfo in SInfoListOne)
			{
				string cd = sInfo.Name;
				if (sInfo.Kind == SInfoKind.Method) cd += "()";
				data.Add(new MyCompletionData(cd));
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
		[BryScript]
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
		
		
		private string[] SInfoCategorys = new string[0];

		private SInfo[] SInfoListOne = new SInfo[0];
		private SInfo[] SInfoListDotCat = new SInfo[0];
		private SInfo[] SInfoListDotNoCat = new SInfo[0];


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

		private int IndexOgCategory(List<string>lst,string n)
		{
			int ret = -1;
			if(lst.Count>0)
			{
				for(int i =lst.Count-1; i>=0;i--)
				{
					if(string.Compare(lst[i],n,true)==0)
					{
						ret = i;
						break;
					}
				}
			}
			return ret;
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
		private List<SInfo> DupChk(List<SInfo> lst)
		{
			List<SInfo> ret = new List<SInfo>();
			if (lst.Count > 0)
			{
				ret.Add(lst[0]);
				for (int i = 1; i<lst.Count; i++)
				{
					if ((lst[i - 1].Name == lst[i].Name))
					{
						//何もしない
					}
					else
					{
						ret.Add(lst[i]);
					}
				}
			}
			return ret;
		}
		public void SetSInfo(SInfo[] ary)
		{
			List<SInfo> listOne = new List<SInfo>();
			List<SInfo> listDotCat = new List<SInfo>();
			List<SInfo> listDotNoCat = new List<SInfo>();
			List<string> CatList = new List<string>();

			foreach (SInfo s in ary)
			{
				if (s.Category == "")
				{
					listOne.Add(s);
				}
				else
				{
					int idx = IndexOgCategory(CatList, s.Category);
					if (idx < 0) CatList.Add(s.Category);
					if (s.IsGlobal)
					{
						listDotNoCat.Add(s);
					}
					else
					{
						listDotCat.Add(s);
					}
				}
			}
			CatList.Sort();
			SInfoCategorys = CatList.ToArray();

			listOne.Sort((a, b) => string.Compare(a.Name, b.Name));
			SInfoListOne = listOne.ToArray();

			listDotCat.Sort((a, b) => string.Compare(a.Name, b.Name));
			SInfoListDotCat = listDotCat.ToArray();

			listDotNoCat.Sort((a, b) => string.Compare(a.Name, b.Name));
			listDotNoCat = DupChk(listDotNoCat);
			SInfoListDotNoCat = listDotNoCat.ToArray();

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
