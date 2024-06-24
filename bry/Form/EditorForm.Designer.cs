namespace bry
{
	partial class EditorForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			ICSharpCode.AvalonEdit.Document.TextDocument textDocument1 = new ICSharpCode.AvalonEdit.Document.TextDocument();
			System.ComponentModel.Design.ServiceContainer serviceContainer1 = new System.ComponentModel.Design.ServiceContainer();
			ICSharpCode.AvalonEdit.Document.UndoStack undoStack1 = new ICSharpCode.AvalonEdit.Document.UndoStack();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
			this.aEdit1 = new bry.AEdit();
			this.SuspendLayout();
			// 
			// aEdit1
			// 
			this.aEdit1.ColumnRulerPosition = 80;
			this.aEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
			textDocument1.FileName = null;
			textDocument1.ServiceProvider = serviceContainer1;
			textDocument1.Text = "aEdit1";
			undoStack1.SizeLimit = 2147483647;
			textDocument1.UndoStack = undoStack1;
			this.aEdit1.Document = textDocument1;
			this.aEdit1.HighlightCurrentLine = false;
			this.aEdit1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
			this.aEdit1.IndentationSize = 4;
			this.aEdit1.Location = new System.Drawing.Point(0, 0);
			this.aEdit1.Name = "aEdit1";
			this.aEdit1.Options = ((ICSharpCode.AvalonEdit.TextEditorOptions)(resources.GetObject("aEdit1.Options")));
			this.aEdit1.ShowBoxForControlCharacters = true;
			this.aEdit1.ShowColumnRuler = false;
			this.aEdit1.ShowEndOfLine = false;
			this.aEdit1.ShowLineNumbers = true;
			this.aEdit1.ShowSpaces = false;
			this.aEdit1.ShowTabs = true;
			this.aEdit1.Size = new System.Drawing.Size(747, 383);
			this.aEdit1.TabIndex = 0;
			this.aEdit1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
			// 
			// EditorForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(747, 383);
			this.Controls.Add(this.aEdit1);
			this.Name = "EditorForm";
			this.TabText = "";
			this.ResumeLayout(false);

		}

		#endregion

		private AEdit aEdit1;
	}
}
