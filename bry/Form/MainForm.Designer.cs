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
			this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.scriptMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.executeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.initEngineMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.windowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editorMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.outputMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.refMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.fontMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.quitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.scriptMenu,
            this.windowMenu});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(910, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitMenu});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "File";
			// 
			// editMenu
			// 
			this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontMenu});
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new System.Drawing.Size(39, 20);
			this.editMenu.Text = "Edit";
			// 
			// scriptMenu
			// 
			this.scriptMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeMenu,
            this.initEngineMenu});
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
			// initEngineMenu
			// 
			this.initEngineMenu.Name = "initEngineMenu";
			this.initEngineMenu.Size = new System.Drawing.Size(154, 22);
			this.initEngineMenu.Text = "InitEngine";
			// 
			// windowMenu
			// 
			this.windowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorMenu,
            this.outputMenu,
            this.refMenu});
			this.windowMenu.Name = "windowMenu";
			this.windowMenu.Size = new System.Drawing.Size(63, 20);
			this.windowMenu.Text = "Window";
			// 
			// editorMenu
			// 
			this.editorMenu.Name = "editorMenu";
			this.editorMenu.Size = new System.Drawing.Size(126, 22);
			this.editorMenu.Text = "Editor";
			// 
			// outputMenu
			// 
			this.outputMenu.Name = "outputMenu";
			this.outputMenu.Size = new System.Drawing.Size(126, 22);
			this.outputMenu.Text = "Output";
			// 
			// refMenu
			// 
			this.refMenu.Name = "refMenu";
			this.refMenu.Size = new System.Drawing.Size(126, 22);
			this.refMenu.Text = "Reference";
			// 
			// dockPanel1
			// 
			this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel1.Location = new System.Drawing.Point(0, 24);
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.Size = new System.Drawing.Size(910, 537);
			this.dockPanel1.TabIndex = 1;
			// 
			// fontMenu
			// 
			this.fontMenu.Name = "fontMenu";
			this.fontMenu.Size = new System.Drawing.Size(180, 22);
			this.fontMenu.Text = "Font";
			// 
			// quitMenu
			// 
			this.quitMenu.Name = "quitMenu";
			this.quitMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.quitMenu.Size = new System.Drawing.Size(180, 22);
			this.quitMenu.Text = "Quit";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(910, 561);
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
		private System.Windows.Forms.ToolStripMenuItem scriptMenu;
		private System.Windows.Forms.ToolStripMenuItem executeMenu;
		private System.Windows.Forms.ToolStripMenuItem initEngineMenu;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
		private System.Windows.Forms.ToolStripMenuItem windowMenu;
		private System.Windows.Forms.ToolStripMenuItem editorMenu;
		private System.Windows.Forms.ToolStripMenuItem outputMenu;
		private System.Windows.Forms.ToolStripMenuItem refMenu;
		private System.Windows.Forms.ToolStripMenuItem fontMenu;
		private System.Windows.Forms.ToolStripMenuItem quitMenu;
	}
}

