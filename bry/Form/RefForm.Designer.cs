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
			this.helpList1 = new bry.HelpList();
			this.SuspendLayout();
			// 
			// helpList1
			// 
			this.helpList1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpList1.Font = new System.Drawing.Font("源ノ角ゴシック Code JP R", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.helpList1.Location = new System.Drawing.Point(0, 0);
			this.helpList1.Name = "helpList1";
			this.helpList1.Size = new System.Drawing.Size(428, 411);
			this.helpList1.TabIndex = 0;
			this.helpList1.Text = "helpList1";
			// 
			// RefForm
			// 
			this.ClientSize = new System.Drawing.Size(428, 411);
			this.Controls.Add(this.helpList1);
			this.Name = "RefForm";
			this.ResumeLayout(false);

		}

		#endregion

		private HelpList helpList1;
	}
}
