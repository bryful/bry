namespace bry
{
	partial class MainForm
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.newMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.quitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editorFontMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.outputFontMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.referenceFontMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.windowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.scriptMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.executeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.initEngeineMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.windowMenu,
            this.scriptMenu});
			this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(658, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenu,
            this.openToolStripMenuItem,
            this.closeMenu,
            this.quitMenu});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "File";
			// 
			// newMenu
			// 
			this.newMenu.Name = "newMenu";
			this.newMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newMenu.Size = new System.Drawing.Size(146, 22);
			this.newMenu.Text = "New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "Open";
			// 
			// closeMenu
			// 
			this.closeMenu.Name = "closeMenu";
			this.closeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.closeMenu.Size = new System.Drawing.Size(146, 22);
			this.closeMenu.Text = "Close";
			// 
			// quitMenu
			// 
			this.quitMenu.Name = "quitMenu";
			this.quitMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.quitMenu.Size = new System.Drawing.Size(146, 22);
			this.quitMenu.Text = "Quit";
			// 
			// editMenu
			// 
			this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorFontMenu,
            this.outputFontMenu,
            this.referenceFontMenu});
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new System.Drawing.Size(39, 20);
			this.editMenu.Text = "Edit";
			// 
			// editorFontMenu
			// 
			this.editorFontMenu.Name = "editorFontMenu";
			this.editorFontMenu.Size = new System.Drawing.Size(180, 22);
			this.editorFontMenu.Text = "EditorFont";
			// 
			// outputFontMenu
			// 
			this.outputFontMenu.Name = "outputFontMenu";
			this.outputFontMenu.Size = new System.Drawing.Size(180, 22);
			this.outputFontMenu.Text = "OutputFont";
			// 
			// referenceFontMenu
			// 
			this.referenceFontMenu.Name = "referenceFontMenu";
			this.referenceFontMenu.Size = new System.Drawing.Size(180, 22);
			this.referenceFontMenu.Text = "ReferenceFont";
			// 
			// windowMenu
			// 
			this.windowMenu.Name = "windowMenu";
			this.windowMenu.Size = new System.Drawing.Size(63, 20);
			this.windowMenu.Text = "Window";
			// 
			// scriptMenu
			// 
			this.scriptMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeMenu,
            this.initEngeineMenu});
			this.scriptMenu.Name = "scriptMenu";
			this.scriptMenu.Size = new System.Drawing.Size(49, 20);
			this.scriptMenu.Text = "Script";
			// 
			// executeMenu
			// 
			this.executeMenu.Name = "executeMenu";
			this.executeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.executeMenu.Size = new System.Drawing.Size(154, 22);
			this.executeMenu.Text = "Execute";
			// 
			// initEngeineMenu
			// 
			this.initEngeineMenu.Name = "initEngeineMenu";
			this.initEngeineMenu.Size = new System.Drawing.Size(154, 22);
			this.initEngeineMenu.Text = "InitEngeine";
			// 
			// dockPanel1
			// 
			this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel1.Location = new System.Drawing.Point(0, 24);
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.Size = new System.Drawing.Size(658, 327);
			this.dockPanel1.TabIndex = 1;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 329);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(658, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(658, 351);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.dockPanel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "JavaScript V8 console";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileMenu;
		private System.Windows.Forms.ToolStripMenuItem editMenu;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
		private System.Windows.Forms.ToolStripMenuItem windowMenu;
		private System.Windows.Forms.ToolStripMenuItem editorFontMenu;
		private System.Windows.Forms.ToolStripMenuItem quitMenu;
		private System.Windows.Forms.ToolStripMenuItem newMenu;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem scriptMenu;
		private System.Windows.Forms.ToolStripMenuItem executeMenu;
		private System.Windows.Forms.ToolStripMenuItem initEngeineMenu;
		private System.Windows.Forms.ToolStripMenuItem outputFontMenu;
		private System.Windows.Forms.ToolStripMenuItem referenceFontMenu;
		private System.Windows.Forms.ToolStripMenuItem closeMenu;
		private System.Windows.Forms.StatusStrip statusStrip1;
	}
}

