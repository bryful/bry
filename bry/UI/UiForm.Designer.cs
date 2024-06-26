namespace bry
{
	partial class UiForm
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
			this.uiHLayout1 = new bry.UiHLayout();
			this.SuspendLayout();
			// 
			// uiHLayout1
			// 
			this.uiHLayout1.Alignment = System.Drawing.StringAlignment.Near;
			this.uiHLayout1.BackColor = System.Drawing.Color.Transparent;
			this.uiHLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uiHLayout1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
			this.uiHLayout1.FontSize = 9F;
			this.uiHLayout1.IsDrawLayout = false;
			this.uiHLayout1.LineAlignment = System.Drawing.StringAlignment.Near;
			this.uiHLayout1.Location = new System.Drawing.Point(0, 0);
			this.uiHLayout1.MainColor = System.Drawing.Color.LightGray;
			this.uiHLayout1.Name = "uiHLayout1";
			this.uiHLayout1.Size = new System.Drawing.Size(596, 328);
			this.uiHLayout1.SizePolicyHor = bry.SizePolicy.Fixed;
			this.uiHLayout1.SizePolicyVer = bry.SizePolicy.Fixed;
			this.uiHLayout1.TabIndex = 0;
			this.uiHLayout1.Text = "uiHLayout1";
			// 
			// UiForm
			// 
			this.ClientSize = new System.Drawing.Size(596, 328);
			this.Controls.Add(this.uiHLayout1);
			this.Name = "UiForm";
			this.ResumeLayout(false);

		}

		#endregion

		private UiHLayout uiHLayout1;
	}
}
