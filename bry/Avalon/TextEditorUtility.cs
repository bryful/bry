/*  AvalonEdit とともに使うユーティリティである。
 * 
 *  著作権は pmansato にあるが、コードの流用・改変は自由である。
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents; // LogicalDirection
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Runtime.InteropServices; // DllImport
using System.Diagnostics;
using System.Security;
using System.Security.Permissions; // UIPermission

using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Document; // ITextSource
using ICSharpCode.AvalonEdit.Editing; // Caret
using ICSharpCode.AvalonEdit.Highlighting; // HighlightingManager
using ICSharpCode.AvalonEdit.Search; // Search

//namespace emanual.Wpf.TextEditor
namespace bry
{
	public static class TextEditorUtility
	{
		//---------------------------------------------------------------------------------------------
		// 現在のアプリケーションを実行したときのディレクトリ名を取得する
		public static string GetExecutableFolder()
		{
			return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		}

		//---------------------------------------------------------------------------------------------
		// 現在のアプリケーションを実行したときのフルパスを取得する
		public static string GetExecutablePath()
		{
			return System.Reflection.Assembly.GetEntryAssembly().Location;
		}
	
		//---------------------------------------------------------------------------------------------
		// ドキュメントの先頭にキャレットを移動する
		public static void MoveToDocumentStart(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			textEditor.TextArea.Caret.Offset = 0;
			textEditor.TextArea.Caret.BringCaretToView();
		}

		//---------------------------------------------------------------------------------------------
		// ドキュメントの末尾にキャレットを移動する
		public static void MoveToDocumentEnd(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			textEditor.TextArea.Caret.Offset = textEditor.Document.TextLength - 1;
			textEditor.TextArea.Caret.BringCaretToView();
		}

		//---------------------------------------------------------------------------------------------
		// WordWrap = true のとき、[Home] で現在キャレットのある行の行頭にキャレットを移動する
		// （注意：このメソッドを呼び出すとき、WordWrap = true が前提なので、呼び出す前にチェックすること）
		public static void MoveCaretToStartOfLine(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			Caret caret = textEditor.TextArea.Caret;

			// 現在キャレットのある位置の VisualLine を取得する
			VisualLine visualLine = textEditor.TextArea.TextView.GetVisualLine(caret.Line);
			var textLines = visualLine.TextLines;

			int result = 0;
			int column = caret.Column - 1;
			int totalLength = 0, prevLineLength = 0;

			if (textLines.Count > 1) // 複数行にまたがるとき
			{
				foreach (var textLine in textLines)
				{
					totalLength += textLine.Length;

					if (column < totalLength)
					{
						result = prevLineLength;
						break;
					}
					else
						prevLineLength = totalLength;
				}
			}

			caret.Column = result + 1;
		}

		//---------------------------------------------------------------------------------------------
		// WordWrap = true のとき、[End] で現在キャレットのある行の行末にキャレットを移動する
		// （注意：このメソッドを呼び出すとき、WordWrap = true が前提なので、呼び出す前にチェックすること）
		public static void MoveCaretToEndOfLine(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			Caret caret = textEditor.TextArea.Caret;

			// 現在キャレットのある位置の VisualLine を取得する
			VisualLine visualLine = textEditor.TextArea.TextView.GetVisualLine(caret.Line);
			var textLines = visualLine.TextLines;

			int result = 0;
			int column = caret.Column - 1;
			int totalLength = 0, prevLineLength = 0;

			if (textLines.Count > 1) // 複数行にまたがるとき
			{
				foreach (var textLine in textLines)
				{
					totalLength += textLine.Length;

					if (column < totalLength)
					{
						result = totalLength - 1;
						break;
					}
					else
						prevLineLength = totalLength;
				}
			}
			else
			{
				result = visualLine.VisualLength;
			}

			caret.Column = result + 1;
		}

		//---------------------------------------------------------------------------------------------
		// 指定のハイライト表示定義名に対応するハイライト表示定義を取得する
		// name : AvalonEdit に組み込み済みのハイライト表示定義名（C# のとき、"C#"）
		public static IHighlightingDefinition GetHighlightingByName(string name)
		{
			var manager = HighlightingManager.Instance;
			IHighlightingDefinition definition = manager.GetDefinition(name);

			return definition;
		}

		//---------------------------------------------------------------------------------------------
		// ファイルのフルパスの短縮形を取得する
		// width    : 表示幅
		// filePath : ファイルのフルパス
		// fontSize : フォントのサイズ（単位：論理ピクセル）
		// typeface : Typeface オブジェクト
		// type     : 短縮形の形式
		//   0 = c:\...\abc\xyz\MSPaint.exe
		//   1 = c:\Program Files\Common Files\...\MSPaint.exe
		public static string GetEllipsisFileName(double width, string filePath, double fontSize, Typeface typeface, int type)
		{
			Size size = MeasureString(filePath, fontSize, typeface);

			// パスの幅が表示幅より小さいとき、フルパスのままを返す
			if (size.Width <= width)
				return filePath;

			string s;
			const string ELLIPSIS = "...";
			string rootDir = Path.GetPathRoot(filePath);
			string fileName = Path.GetFileName(filePath); // filePath がディレクトリ名のときは最後の要素を取り出す

			// c:\...\fileName の形式でも制限長さを超えるとき、fileName だけを返す
			s = rootDir + ELLIPSIS + "\\" + fileName;
			if (MeasureString(s, fontSize, typeface).Width > width)
				return fileName;

			// パスを \ で区切る（通常、先頭はルートディレクトリ名、最後はファイル名）
			string[] array = filePath.Split('\\');

			if (type == 0)
			{
				int start = 2; // ルートディレクトリ名と次のディレクトリ名をとばす

				do
				{
					s = rootDir + ELLIPSIS;

					for (int i = start; i < array.Length; ++i)
						s += "\\" + array[i];

					++start;
				}
				while (MeasureString(s, fontSize, typeface).Width > width);
			}
			else
			{
				int end = 2; // パスの最後の 2 つの要素は加算しない

				do
				{
					s = rootDir;

					for (int i = 1; i < array.Length - end; ++i)
						s += array[i] + "\\";

					s += ELLIPSIS + "\\" + fileName;
					++end;
				}
				while (MeasureString(s, fontSize, typeface).Width > width);
			}

			return s;
		}

		//---------------------------------------------------------------------------------------
		// テキストの幅と高さを取得する（GetEllipsisFileName メソッド内で使う）
		private static Size MeasureString(string text, double fontSize, Typeface typeFace)
		{
			FormattedText formattedText = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture,
					 FlowDirection.LeftToRight, typeFace, fontSize, Brushes.Black);

			return new Size(formattedText.Width, formattedText.Height);
		}

		//---------------------------------------------------------------------------------------------
		// AvalonEdit の内容をハイライト表示を有効にして FlowDocument に変換する
		public static FlowDocument GetFlowDocument(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			IHighlighter highlighter = textEditor.TextArea.GetService(typeof(IHighlighter)) as IHighlighter;

			var paragraph = new Paragraph();

			foreach (DocumentLine line in textEditor.Document.Lines)
			{
				var builder = new HighlightedInlineBuilder(textEditor.Document.GetText(line));

				if (highlighter != null)
				{
					HighlightedLine highlightedLine = highlighter.HighlightLine(line.LineNumber);

					foreach (HighlightedSection section in highlightedLine.Sections)
					{
						builder.SetHighlighting(section.Offset - line.Offset, section.Length, section.Color);
					}
				}

				paragraph.Inlines.AddRange(builder.CreateRuns());
				paragraph.Inlines.Add(new LineBreak());
			}

			var document = new FlowDocument(paragraph);
			document.FontFamily = textEditor.FontFamily;
			document.FontSize = textEditor.FontSize;

			return document;
		}
	
		//---------------------------------------------------------------------------------------------
		// AvalonEdit の内容をハイライト表示を有効にして HTML ドキュメントとして取得する
		/*
		public static string GetHtmlDocument(ICSharpCode.AvalonEdit.TextEditor textEditor, IHighlightingDefinition definition)
		{
			HighlightingManager manager = HighlightingManager.Instance;
			var highlighter = new DocumentHighlighter(textEditor.Document, definition.MainRuleSet);

			var option = new HtmlOptions(textEditor.Options);
			option.TabSize = textEditor.Options.IndentationSize;

			var sb = new System.Text.StringBuilder();
			sb.AppendLine("<html>");
			sb.AppendLine("<head>");
			sb.AppendLine("<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.0 Strict//EN\">");
			sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
			sb.AppendLine("</head>");
			sb.AppendLine("<body>");
			sb.AppendLine("<pre>");

			for (int i = 1; i < textEditor.Document.LineCount; ++i)
			{
				sb.AppendLine(String.Format("{0}", highlighter.HighlightLine(i).ToHtml(option)));
			}

			sb.AppendLine("</pre>");
			sb.AppendLine("</body>");
			sb.AppendLine("</html>");

			return sb.ToString();
		}
		*/
		//---------------------------------------------------------------------------------------------
		// ドキュメントの各行を保持するリストを取得する（行の改行文字は含まない）
		public static List<string> GetTextListForDocument(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			var source = textEditor.Document.CreateSnapshot() as RopeTextSource;
			var reader = source.CreateReader();

			var list = new List<String>();

			while (reader.Peek() >= 0)
			{
				list.Add(reader.ReadLine());
			}

			return list;
		}

		//---------------------------------------------------------------------------------------------
		// テキストの行頭のホワイトスペースを削除する
		public static void DeleteLeadingWhitespace(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			textEditor.Document.BeginUpdate();

			for (int i = 0; i < textEditor.Document.Lines.Count; ++i)
			{
				DocumentLine line = textEditor.Document.Lines[i];
				string s = textEditor.Document.GetText(line.Offset, line.Length);
				s = s.TrimStart();

				textEditor.Document.Replace(line.Offset, line.Length, s);
			}

			textEditor.Document.EndUpdate();
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲内のテキストの行末のホワイトスペースを削除する
		public static void DeleteTrailingWhitespace(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			textEditor.Document.BeginUpdate();

			for (int i = 0; i < textEditor.Document.Lines.Count; ++i)
			{
				DocumentLine line = textEditor.Document.Lines[i];
				string s = textEditor.Document.GetText(line.Offset, line.Length);
				s = s.TrimEnd();

				textEditor.Document.Replace(line.Offset, line.Length, s);
			}

			textEditor.Document.EndUpdate();
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の行をインデントする（行頭に一つの半角スペースを挿入する）
		// textEditor : TextEditor オブジェクト
		public static void IndentSelectedSection(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			// ドキュメントのすべての DocumentLine のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);

			// 選択範囲の開始行と終了行との行番号
			SelectedRange range = TextEditorUtility.GetSelectedRange(textEditor);

			for (int i = range.StartLine - 1; i < range.EndLine - 1; ++i)
			{
				list[i] = " " + list[i];
			}

			// textEditor のドキュメントとして戻す
			OutputToDocument(textEditor, list);
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の行のインデントを解除する（行頭の半角・全角スペースまたは [Tab] を一つ削除する）
		// textEditor : TextEditor オブジェクト
		public static void UnindentSelectedSection(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			// ドキュメントのすべての DocumentLine のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);

			// 選択範囲の開始行と終了行との行番号
			SelectedRange range = TextEditorUtility.GetSelectedRange(textEditor);

			for (int i = range.StartLine - 1; i < range.EndLine - 1; ++i)
			{
				string s = list[i];

				if ((s.StartsWith(" ")) || (s.StartsWith("\x3000")) || (s.StartsWith("\t")))
				{
					s = s.Substring(1); // 先頭の文字だけ除く
				}

				list[i] = s;
			}

			// textEditor のドキュメントとして戻す
			OutputToDocument(textEditor, list);
		}

		//---------------------------------------------------------------------------------------------
		// 行頭のホワイトスペースを [Tab] に変換する（行頭以外の [Tab] はそのまま）
		// text    : 操作対象のテキスト
		// tabSize : [Tab] の幅 
		public static string ConvertLeadingWhiteSpacesToTabs(string text, int tabSize)
		{
			var sb = new System.Text.StringBuilder(text.Length);
			int spaces = 0;
			int i = 0;

			for (i = 0; i < text.Length; ++i)
			{
				if (text[i] == ' ')
				{
					++spaces;

					if (spaces == tabSize)
					{
						sb.Append('\t');
						spaces = 0;
					}
				}
				else if (text[i] == '\x3000') // 全角スペース
				{
					spaces += 2;

					if (spaces == tabSize)
					{
						sb.Append('\t');
						spaces = 0;
					}
				}
				else if (text[i] == '\t')
				{
					sb.Append('\t');
					spaces = 0;
				}
				else
				{
					break;
				}
			}

			// 残りの文字を追加する
			if (i < text.Length)
			{
				sb.Append(text.Substring(i - spaces));
			}

			return sb.ToString();
		}

		//---------------------------------------------------------------------------------------------
		// 行頭の [Tab] をスペースに置換する（行頭以外の [Tab] はそのまま）
		// text    : 操作対象のテキスト
		// tabSize : [Tab] の幅 
		public static string ConvertLeadingTabsToWhiteSpaces(string text, int tabSize)
		{
			string tab = new String(' ', tabSize);
			var sb = new System.Text.StringBuilder(text.Length);

			int i = 0;

			for (i = 0; i < text.Length; ++i)
			{
				if (text[i] == ' ')
				{
					sb.Append(' ');
				}
				else if (text[i] == '\x3000') // 全角スペース
				{
					sb.Append('\x3000');
				}
				else if (text[i] == '\t')
				{
					sb.Append(tab);
				}
				else
				{
					break;
				}
			}

			// 残りの文字を追加する
			if (i < text.Length)
			{
				sb.Append(text.Substring(i));
			}

			return sb.ToString();
		}

		//---------------------------------------------------------------------------------------------
		// [Tab] を含む文字列を展開する
		// text        : 操作対象の文字列
		// tabSize : タブ幅（カラム数）
		public static string ExpandTabs(string text, int tabSize)
		{
			int index = 0;
			var sb = new System.Text.StringBuilder();

			foreach (char c in text)
			{
				if (c == '\t')
				{
					sb.Append(' ', tabSize - (index % tabSize));
					index = 0;
				}
				else
				{
					sb.Append(c);

					// 2 バイト文字のとき、インデックスを 2 つ進める
					if (System.Text.Encoding.Default.GetByteCount(new string(c, 1)) == 2)
						index += 2;
					else
						++index;
				}
			}

			return sb.ToString();
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の行番号を取得する（先頭の行の行番号は 1）
		public static SelectedRange GetSelectedRange(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
		  var textArea = textEditor.TextArea;
			int startLine = 0, endLine = 0;

			if (textArea.Selection.IsEmpty) // 選択範囲が空のとき
			{
				startLine = 1;
				endLine = textArea.Document.LineCount;
			}
			else
			{
				startLine = textArea.Document.GetLineByOffset(textArea.Selection.SurroundingSegment.Offset).LineNumber;
				endLine = textArea.Document.GetLineByOffset(textArea.Selection.SurroundingSegment.EndOffset).LineNumber;
			}

			return new SelectedRange(startLine, endLine);
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の行を昇順でソートする（選択範囲がないときは何もしない）
		// textEditor : TextEditor オブジェクト
		public static void SortSelectedSection(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			SortSelectedSection(textEditor, true);
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の行をソートする（選択範囲がないときは何もしない）
		// textEditor : TextEditor オブジェクト
		// isAscend   : 昇順（a → z）のとき true
		public static void SortSelectedSection(ICSharpCode.AvalonEdit.TextEditor textEditor, bool isAscend)
		{
			// ドキュメントのすべての DocumentLine のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);

			// 選択範囲の開始行と終了行との行番号
			SelectedRange range = TextEditorUtility.GetSelectedRange(textEditor);

			// ドキュメントの指定の範囲のテキスト行をソートする
			if (isAscend)
				list.Sort(range.StartLine - 1, range.EndLine - range.StartLine, Comparer<string>.Default);
			else
				list.Sort(range.StartLine - 1, range.EndLine - range.StartLine, new DescendingComparer());

			// textEditor のドキュメントとして戻す
			OutputToDocument(textEditor, list);
		}

		//---------------------------------------------------------------------------------------------
		// 英単語の前後にスペースを挿入する（英単語の前後にスペースがないと読みにくいから）
		public static void InsertWordSpace(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			if (textEditor == null)
				throw new ArgumentNullException("InsertWordSpace()");

			// ドキュメントのすべてのテキスト行のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);
			List<string> newList = new List<string>();
			string s = String.Empty;

			// 長さが 0 より大きな行を新しいリストに追加する
			for (int i = 0; i < list.Count; ++i)
			{
				s = list[i];

				// 半角英数字と全角文字との間にスペースを挿入する
				s = Regex.Replace(s, "([0-9a-zA-Z_])([^\x00-\xff])", "$1 $2");

				// 全角文字と半角英数字との間にスペースを挿入する
				s = Regex.Replace(s, "([^\x00-\xff])([0-9a-zA-Z_])", "$1 $2");

				newList.Add(s);
			}

			OutputToDocument(textEditor, newList);
		}

		//---------------------------------------------------------------------------------------------
		// 連続する同一行を一行にする（複数の空行は 1 行の空行にする）
		public static void UniqueLine(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			if (textEditor == null)
				throw new ArgumentNullException("UniqueLine()");

			// ドキュメントのすべてのテキスト行のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);
			List<string> newList = new List<string>();
			string s = String.Empty;

			// 長さが 0 より大きな行を新しいリストに追加する
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i] != s)
				{
					newList.Add(list[i]);
					s = list[i];
				}
			}

			OutputToDocument(textEditor, newList);
		}

		//---------------------------------------------------------------------------------------------
		// 空行（長さ 0 の行）を削除する
		public static void DeleteEmptyLine(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			if (textEditor == null)
				throw new ArgumentNullException("DeleteEmptyLine()");

			// ドキュメントのすべてのテキスト行のリスト
			List<string> list = TextEditorUtility.GetTextListForDocument(textEditor);
			List<string> newList = new List<string>();

			// 長さが 0 より大きな行を新しいリストに追加する
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].Length > 0)
					newList.Add(list[i]);
			}

			OutputToDocument(textEditor, newList);
		}

		//---------------------------------------------------------------------------------------------
		// 加工済みのリストをドキュメントに出力する private メソッド
		private static void OutputToDocument(ICSharpCode.AvalonEdit.TextEditor textEditor, List<string> list)
		{
			textEditor.Clear();
			textEditor.BeginChange();

			for (int i = 0; i < list.Count; ++i)
			{
				textEditor.AppendText(list[i] + System.Environment.NewLine);
			}

			textEditor.EndChange();
		}

		//---------------------------------------------------------------------------------------------
		// 選択範囲の DocumentLine とその行番号のリストを取得する
		public static List<DocumentLineData> GetSelectedDocumentLine(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			var list = new List<DocumentLineData>();
			var selection = textEditor.TextArea.Selection;

			if (selection == null)
				return list;

			var document = textEditor.Document;

			foreach (var segment in selection.Segments)
			{
				DocumentLine documentLine = document.GetLineByOffset(segment.StartOffset);

				for (int offset = segment.StartOffset + 1; offset <= segment.EndOffset; ++offset)
				{
					var line = document.GetLineByOffset(offset);

					if (line != documentLine)
					{
						var data = new DocumentLineData();
						data.LineNumber = documentLine.LineNumber;
						data.DocumentLine = documentLine;
						list.Add(data);
						documentLine = line;
					}
				}
			}

			return list;
		}

		//---------------------------------------------------------------------------------------------
		// キャレットの座標位置とサイズを取得する
 		// textEditor : TextEditor オブジェクト
		public static Rect GetCaretRect(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			if (textEditor == null)
				return Rect.Empty;

			var caret = textEditor.TextArea.Caret;
			VisualLine visualLine = textEditor.TextArea.TextView.GetVisualLine(caret.Line);

			if (visualLine == null)
				return Rect.Empty;

			System.Windows.Media.TextFormatting.TextLine textLine = visualLine.GetTextLine(caret.VisualColumn);
			double xPos = textLine.GetDistanceFromCharacterHit(new System.Windows.Media.TextFormatting.CharacterHit(caret.VisualColumn, 0));
			double lineTop = visualLine.GetTextLineVisualYPosition(textLine, VisualYPosition.TextTop);
			double lineBottom = visualLine.GetTextLineVisualYPosition(textLine, VisualYPosition.LineBottom);

			return new Rect(xPos, lineTop, SystemParameters.CaretWidth, lineBottom - lineTop);
		}

		//---------------------------------------------------------------------------------------------
		// 現在のキャレットの位置情報を取得する（WordWrap=true の場合にも対応）
		// textEditor : TextEditor オブジェクト
		public static TextLocation GetCaretLocation(ICSharpCode.AvalonEdit.TextEditor textEditor)
		{
			Caret caret = textEditor.TextArea.Caret;

			// テキストを折り返さないモードのとき
			if (textEditor.WordWrap == false)
			{
				return caret.Location;
			}

			//以下、テキストを折り返すモードのとき
			if (textEditor.TextArea.TextView.VisualLinesValid == false)
			{
				return caret.Location;
			}

			// 現在キャレットのある位置の VisualLine を取得する
			VisualLine visualLine = textEditor.TextArea.TextView.GetVisualLine(caret.Line);

			if (visualLine == null)
			{
				return caret.Location;
			}

			// ドキュメントの上辺から VisualLine の上辺までの距離を取得する（この場合、カラム番号は無視する）
			var p = visualLine.GetVisualPosition(0, VisualYPosition.LineTop);

			var lineHeight = textEditor.TextArea.TextView.DefaultLineHeight;
			int lineNumber = Convert.ToInt32(p.Y / lineHeight) + 1; // 現在のキャレット位置の行番号
			var textLines = visualLine.TextLines;

			int line = 0;
			int currentCaretColumn = caret.Column - 1;
			int totalLength = 0, prevLineLength = 0;

			// 現在のキャレットのある行が何行目にあるかを調べる
			foreach (var textLine in textLines)
			{
				totalLength += textLine.Length;

				if (currentCaretColumn < totalLength)
				{
					break;
				}
				else
				{
					prevLineLength = totalLength;
					++line;
				}
			}

			int column = 0;
			totalLength = 0;
			prevLineLength = 0;

			if (textLines.Count > 1) // 複数行にまたがるとき
			{
				foreach (var textLine in textLines)
				{
					totalLength += textLine.Length;

					if (currentCaretColumn < totalLength)
					{
						column = currentCaretColumn - prevLineLength;
						break;
					}
					else
						prevLineLength = totalLength;
				}
			}
			else
			{
				column = currentCaretColumn;
			}

			return new TextLocation(lineNumber + line, column + 1);
		}

		//---------------------------------------------------------------------------------------------
		// 指定のキャレット位置の語句を切り出す
		// text   : 検索対象のテキスト（TextDocument オブジェクト）
		// offset : 現在のキャレット位置（ドキュメントの先頭からの文字数） 
		public static string GetWordFromCaretPosition(string text, int offset)
		{
			// 現在キャレットのある位置の文字
			if (offset >= text.Length) offset = text.Length - 1;
			char ch = text[offset];

			// 現在キャレットのある位置の文字のタイプ
			CharType targetType = GetCharType(ch);

			int index = offset;

			// 現在のキャレット位置からうしろの文字の CharType をチェックする
			while (index < text.Length)
			{
				if (GetCharType(text[index]) == targetType) // 同じ CharType なら次の文字をチェック
					++index;
				else
					break;
			}

			int endIndex = index;
			index = offset;

			// 現在のキャレット位置から前の文字をチェックする
			while (index >= 0)
			{
				if (GetCharType(text[index]) == targetType)
					--index;
				else
					break;
			}

			int startIndex = index + 1;

			return text.Substring(startIndex, endIndex - startIndex);
		}

		//-------------------------------------------------------------------------------------
		// 指定の文字のタイプを取得する
		public static CharType GetCharType(char ch)
		{
			CharType result = CharType.ZenKanji; // 以下に含まれない場合は全角漢字とみなす

			if ((ch >= 0x3041) && (ch <= 0x3093))
				result = CharType.ZenHiragana;
			else if ((ch >= 0x30A1) && (ch <= 0x31FF))
				result = CharType.ZenKatakana;
			else if (ch == 0x3000)
				result = CharType.ZenSpace;
			else if ((ch >= 0xFF10) && (ch <= 0xFF19))
				result = CharType.ZenNumeric;
			else if ((ch >= 0xFF21 && ch <= 0xFF3A) || (ch >= 0xFF41 && ch <= 0xFF5A))
				result = CharType.ZenAlpha;
			else if ((ch >= 0x3001 && ch <= 0x303D) || (ch >= 0xFF01 && ch <= 0xFF0F) || (ch >= 0xFF3B && ch <= 0xFF40) || (ch >= 0xFF5B && ch <= 0xFF60) || (ch >= 0xFFE0 && ch <= 0xFFE6))
				result = CharType.ZenSymbol;
			else if (ch == 0x20)
				result = CharType.HanSpace;
			else if ((ch >= 0xFF61) && (ch <= 0xFF9F))
				result = CharType.HanKatakana;
			else if ((ch >= 0x30) && (ch <= 0x39))
				result = CharType.HanNumeric;
			else if ((ch >= 0x41) && (ch <= 0x7A))
				result = CharType.HanAlpha;
			else if ((ch >= 0x21) && (ch <= 0x7E))
				result = CharType.HanSymbol;
			else if (ch == 0x09)
				result = CharType.Tab;
			else if ((ch == 0x0A) || (ch == 0x0D))
				result = CharType.Invalid;

			return result;
		}
	} // end of TextEditorUtility class

	//***********************************************************************************************
	// テキストを文字種別に切り出したトークンのタイプ
	public enum CharType
	{
		ZenKanji,     // 全角漢字（0x4E00 ～ 0x9FA5）
		ZenHiragana,  // 全角ひらがな（0x3041 ～ 0x3093）
		ZenKatakana,  // 全角カタカナ（0x30A1 ～ 0x30F6）
		ZenSpace,	    // 全角スペース（0x3000）
		ZenNumeric,   // 全角数字（0xFF10 ～ 0xFF19）
		ZenAlpha,     // 全角英字（0xFF21 ～ 0xFF3A, 0xFF41 ～ 0xFF5A）
		ZenSymbol,    // 全角記号（0x3001 ～ 0x303D, 0xFF01 ～ 0xFF0F, 0xFF3B ～ 0xFF40, 0xFF5B ～ 0xFF60, 0xFFE0 ～ 0xFFE6）

		HanSpace,			// 半角スペース（0xA0）
		HanKatakana,  // 半角カタカナ（0xFF61 ～ 0xFF9F） カタカナとして使う記号を含む
		HanNumeric,		// 半角数字（0x30 ～ 0x39）
		HanAlpha,	  	// 半角英字（0x41 ～ 0x7A）アンダーラインとアクサングラーブを含む
		HanSymbol,    // 半角記号（0x21 ～ 0x7E）

		Tab,					// [Tab] 0x09
		Selected,     // 選択状態にある
		Invalid       // 不明（全角漢字と同じ扱い）
	} // end of CharType enum

	//***********************************************************************************************
	// テキストの加工のために DocumentLine とその行番号を保持するクラス
	public class DocumentLineData
	{
		public int LineNumber { get; set; }
		public DocumentLine DocumentLine { get; set; }
	} // end of DocumentLineData

	//***********************************************************************************************
	// 選択範囲の開始行および終了行の行番号を保持する
	public class SelectedRange
	{
		public int StartLine { get; set; }
		public int EndLine { get; set; }

		public SelectedRange(int start, int end)
		{
			this.StartLine = start;
			this.EndLine = end;
		}
	}

	//***********************************************************************************************
	// SortSelectedSection メソッドの降順ソートのための Comparer<T> クラス
	internal class DescendingComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			if (x == null)
			{
				if (y == null) // null 同士のときは等値
					return 0;
				else           // x が null、y が null でないとき、常に y のほうが大きい 
					return 1;
			}
			else
			{
				if (y == null) // x が null でなく、y が null のとき、常に x のほうが大きい 
					return -1;
				else           // x、y ともに null でないとき
					return x.CompareTo(y) * -1;
			}
		}
	}

} // end of emanual.Wpf.TextEditor namespace
