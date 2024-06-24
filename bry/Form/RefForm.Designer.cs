namespace bry
{
	partial class RefForm
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
			this.refList = new System.Windows.Forms.ListBox();
			this.findBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// refList
			// 
			this.refList.Font = new System.Drawing.Font("源ノ角ゴシック Code JP R", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.refList.FormattingEnabled = true;
			this.refList.ItemHeight = 23;
			this.refList.Items.AddRange(new object[] {
            "cls()",
            "write(s)",
            "writeln(s)",
            "toHex(value,4,false)",
            "alert(s)"});
			this.refList.Location = new System.Drawing.Point(9, 39);
			this.refList.Name = "refList";
			this.refList.Size = new System.Drawing.Size(234, 234);
			this.refList.TabIndex = 0;
			// 
			// findBox
			// 
			this.findBox.Font = new System.Drawing.Font("源ノ角ゴシック Code JP R", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.findBox.Location = new System.Drawing.Point(9, 2);
			this.findBox.Name = "findBox";
			this.findBox.Size = new System.Drawing.Size(234, 31);
			this.findBox.TabIndex = 1;
			// 
			// RefForm
			// 
			this.ClientSize = new System.Drawing.Size(269, 299);
			this.Controls.Add(this.findBox);
			this.Controls.Add(this.refList);
			this.Name = "RefForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox refList;
		private System.Windows.Forms.TextBox findBox;
	}
}
