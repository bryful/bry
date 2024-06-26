using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
namespace bry
{
	public class UiControl :Control
	{
		
		// ***************************************************
		private SizePolicy m_SizePolicyHor = SizePolicy.Fixed;
		[Category("UI"), Browsable(true)]
		public SizePolicy SizePolicyHor
		{
			get { return m_SizePolicyHor; }
			set 
			{
				bool b = m_SizePolicyHor != value;
				m_SizePolicyHor = value;
				if (b)
				{
					//OnSizePolicyChanged(new EventArgs());
					ChkParentLayout();
				}
			}
		}
		private SizePolicy m_SizePolicyVer = SizePolicy.Fixed;
		[Category("UI"), Browsable(true)]
		public SizePolicy SizePolicyVer
		{
			get { return m_SizePolicyVer; }
			set
			{
				bool b = m_SizePolicyVer != value;
				m_SizePolicyVer = value;
				if (b)
				{
					//OnSizePolicyChanged(new EventArgs());
					ChkParentLayout();
				}
			}
		}
		// **************************************************************
		private System.Drawing.Color m_MainColor = Color.LightGray;
		[Category("Color"), Browsable(true)]
		public System.Drawing.Color MainColor
		{
			get { return m_MainColor; }
			set
			{
				m_MainColor = value;
				this.Invalidate();
			}
		}

		// **************************************************************
		public Color ColorMul(Color c,float per)
		{
			float r = (float)c.R * per/100;
			if (r < 0) r = 0; else if (r > 255) r = 255;
			float g = (float)c.G * per/100;
			if (g < 0) g = 0; else if (g > 255) g = 255;
			float b = (float)c.B * per/100;
			if (b < 0) b = 0; else if (b > 255) b = 255;

			return Color.FromArgb((int)r,(int)g,(int)b);
		}
		// **************************************************************
		[Category("UI"), Browsable(true)]
		public float FontSize
		{
			get { return base.Font.Size; }
			set
			{
				base.Font = 
					new Font(base.Font.Name,value,base.Font.Style);
				this.Invalidate();
			}
		}
		// **************************************************************
		protected StringFormat m_StringFormat = new StringFormat();
		[Category("UI"), Browsable(true)]
		public StringAlignment Alignment
		{
			get { return m_StringFormat.Alignment; }
			set 
			{ 
				m_StringFormat.Alignment =value; 
				this.Invalidate();
			}
		}
		[Category("UI"), Browsable(true)]
		public StringAlignment LineAlignment
		{
			get { return m_StringFormat.LineAlignment; }
			set
			{
				m_StringFormat.LineAlignment = value;
				this.Invalidate();
			}
		}
		// **************************************************************
		[ScriptUsage(ScriptAccess.None)]
		public UiControl() 
		{
			base.Font = new Font("源ノ角ゴシック Code JP R" , 9);
			this.Size = new System.Drawing.Size(100,100);
			m_StringFormat.Alignment = StringAlignment.Center;
			m_StringFormat.LineAlignment = StringAlignment.Center;
			this.SetStyle(
ControlStyles.DoubleBuffer |
ControlStyles.UserPaint |
ControlStyles.AllPaintingInWmPaint |
ControlStyles.SupportsTransparentBackColor,
true);
			this.UpdateStyles();
			base.BackColor = Color.Transparent;
		}
		// **************************************************************
		public void ClearControls()
		{
			ClearSub(this.Controls);
			this.Controls.Clear();
		}
		private void ClearSub(Control.ControlCollection c)
		{
			if (c.Count > 0)
			{
				for (int i = c.Count - 1; i >= 0; i--)
				{
					Control cc = c[i];
					if ((cc is Panel) || (cc is GroupBox) || (cc is UiHLayout))
					{
						ClearSub(cc.Controls);
					}
					cc.Dispose();
					c.RemoveAt(i);
				}
				c.Clear();
			}
		}
		// **************************************************************
		public UiHLayout AddHLayout()
		{
			UiHLayout ly = new UiHLayout();
			this.Controls.Add(ly);
			return ly;
		}
		public UiVLayout AddVLayout()
		{
			this.SuspendLayout();
			this.ResumeLayout();
			//this.Controls.Count
			UiVLayout ly = new UiVLayout();
			this.Controls.Add(ly);
			return ly;
		}
		// **************************************************************
		// **************************************************************

		protected bool NowChkLayout = false;
		[ScriptUsage(ScriptAccess.None)]
		protected virtual void ChkLayout()
		{
		}
		public void layouter()
		{
			ChkLayout();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkParentLayout();
		}
		public void ChkParentLayout()
		{
			if ((this.Parent != null) && (this.Parent is UiControl))
			{
				((UiControl)this.Parent).ChkLayout();
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			using(SolidBrush sb = new SolidBrush(Color.Transparent))
			{
				Graphics g = e.Graphics;
				g.FillRectangle(sb, new Rectangle(0, 0, Width, Height));
			}
		}
		#region Prop1
		[Browsable(false)]
		public new System.String AccessibleDefaultActionDescription
		{
			get { return base.AccessibleDefaultActionDescription; }
			set { base.AccessibleDefaultActionDescription = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.String AccessibleDescription
		{
			get { return base.AccessibleDescription; }
			set { base.AccessibleDescription = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.String AccessibleName
		{
			get { return base.AccessibleName; }
			set { base.AccessibleName = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.AccessibleRole AccessibleRole
		{
			get { return base.AccessibleRole; }
			set { base.AccessibleRole = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean AllowDrop
		{
			get { return base.AllowDrop; }
			set { base.AllowDrop = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.AnchorStyles Anchor
		{
			get { return base.Anchor; }
			set { base.Anchor = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean AutoSize
		{
			get { return base.AutoSize; }
			set { base.AutoSize = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Point AutoScrollOffset
		{
			get { return base.AutoScrollOffset; }
			set { base.AutoScrollOffset = value; }
		}
		// **************************************************************
		[Category("Color"),Browsable(true)]
		public new System.Drawing.Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.ImageLayout BackgroundImageLayout
		{
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.BindingContext BindingContext
		{
			get { return base.BindingContext; }
			set { base.BindingContext = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Rectangle Bounds
		{
			get { return base.Bounds; }
			set { base.Bounds = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean Capture
		{
			get { return base.Capture; }
			set { base.Capture = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean CausesValidation
		{
			get { return base.CausesValidation; }
			set { base.CausesValidation = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Size ClientSize
		{
			get { return base.ClientSize; }
			set { base.ClientSize = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.ContextMenu ContextMenu
		{
			get { return base.ContextMenu; }
			set { base.ContextMenu = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.ContextMenuStrip ContextMenuStrip
		{
			get { return base.ContextMenuStrip; }
			set { base.ContextMenuStrip = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.Cursor Cursor
		{
			get { return base.Cursor; }
			set { base.Cursor = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.DockStyle Dock
		{
			get { return base.Dock; }
			set { base.Dock = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean Enabled
		{
			get { return base.Enabled; }
			set { base.Enabled = value; }
		}
		// **************************************************************
		[ Browsable(true)]
		public new System.Drawing.Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}
		// **************************************************************
		[Category("Color"),Browsable(true)]
		public new System.Drawing.Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Int32 Height
		{
			get { return base.Height; }
			set { base.Height = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean IsAccessible
		{
			get { return base.IsAccessible; }
			set { base.IsAccessible = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Int32 Left
		{
			get { return base.Left; }
			set { base.Left = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Point Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Windows.Forms.Padding Margin
		{
			get { return base.Margin; }
			set { base.Margin = value; this.Invalidate(); }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Drawing.Size MaximumSize
		{
			get { return base.MaximumSize; }
			set { base.MaximumSize = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Drawing.Size MinimumSize
		{
			get { return base.MinimumSize; }
			set { base.MinimumSize = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.String Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.Control Parent
		{
			get { return base.Parent; }
			set { base.Parent = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Drawing.Region Region
		{
			get { return base.Region; }
			set { base.Region = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.RightToLeft RightToLeft
		{
			get { return base.RightToLeft; }
			set { base.RightToLeft = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.ComponentModel.ISite Site
		{
			get { return base.Site; }
			set { base.Site = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Drawing.Size Size
		{
			get { return base.Size; }
			set { base.Size = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Int32 TabIndex
		{
			get { return base.TabIndex; }
			set { base.TabIndex = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Boolean TabStop
		{
			get { return base.TabStop; }
			set { base.TabStop = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Object Tag
		{
			get { return base.Tag; }
			set { base.Tag = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.String Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Int32 Top
		{
			get { return base.Top; }
			set { base.Top = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean UseWaitCursor
		{
			get { return base.UseWaitCursor; }
			set { base.UseWaitCursor = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Boolean Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Int32 Width
		{
			get { return base.Width; }
			set { base.Width = value; }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.IWindowTarget WindowTarget
		{
			get { return base.WindowTarget; }
			set { base.WindowTarget = value; }
		}
		// **************************************************************
		[Browsable(true)]
		public new System.Windows.Forms.Padding Padding
		{
			get { return base.Padding; }
			set { base.Padding = value;this.Invalidate(); }
		}
		// **************************************************************
		[Browsable(false)]
		public new System.Windows.Forms.ImeMode ImeMode
		{
			get { return base.ImeMode; }
			set { base.ImeMode = value; }
		}

		#endregion

	}
	public enum SizePolicy
	{
		Fixed = 0,
		Expanding = 1,
	}
	
}
