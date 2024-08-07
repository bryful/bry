﻿using System;
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
		[BryScript]
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
		[BryScript]
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
		private Rectangle m_TrueClientRect = new Rectangle();
		public Rectangle TrueClientRect
		{
			get { return m_TrueClientRect; }
		}
		private void ChkTrueClientRect()
		{
			int w = this.ClientRectangle.Width;
			int h = this.ClientRectangle.Height;
			int l = this.ClientRectangle.Left;
			int t = this.ClientRectangle.Top;
			// Margin補正
			w -= (this.Margin.Left + this.Margin.Right);
			h -= (this.Margin.Top + this.Margin.Bottom);
			l += (this.Margin.Left);
			t += (this.Margin.Top);
			/*
			w -= (this.Padding.Left + this.Padding.Right);
			h -= (this.Padding.Top + this.Padding.Bottom);
			l += (this.Padding.Left);
			t += (this.Padding.Top);
			*/
			m_TrueClientRect = new Rectangle(l, t, w, h);
		}
		// **************************************************************
		private System.Drawing.Color m_MainColor = Color.LightGray;
		[BryScript]
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
		[BryScript]
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
		[BryScript]
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
		[BryScript]
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
		[BryScript]
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
			ChkTrueClientRect();
		}
		// **************************************************************
		

		protected bool NowChkLayout = false;
		[ScriptUsage(ScriptAccess.None)]
		protected virtual void ChkLayout()
		{
		}
		public void callChkLayout()
		{
			ChkLayout();
			/*
			if(this.Controls.Count > 0)
			{
				for(int i = 0; i < this.Controls.Count; i++)
				{
					if (this.Controls[i] is UiLayout)
					{
						UiLayout c = (UiLayout)this.Controls[i];
						c.callChkLayout();
					}
				}
			}
			*/
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkTrueClientRect();
			
		}
		public void ChkParentLayout()
		{
			if ((this.Parent != null) && (this.Parent is UiLayout))
			{
				((UiLayout)this.Parent).ChkLayout();
			}
		}
		// ****************************************************************
		protected override void OnPaint(PaintEventArgs e)
		{
			using(SolidBrush sb = new SolidBrush(Color.Transparent))
			{
				Graphics g = e.Graphics;
				g.FillRectangle(sb, new Rectangle(0, 0, Width, Height));
			}
		}
		// ****************************************************************
		public void SetFillSize()
		{
			if(this.Parent == null) return;
			if((this.Parent is UiControl) || (this.Parent is UiForm))
			{
				Rectangle r = ((UiControl)this.Parent).TrueClientRect;
				this.Location = r.Location;
				this.Size = r.Size;
			}
		
		}
		public void SetHorFill()
		{
			if (this.Parent == null) return;
			if ((this.Parent is UiControl) || (this.Parent is UiForm))
			{
				Rectangle r = ((UiControl)this.Parent).TrueClientRect;
				this.Left = r.Left;
				this.Width = r.Width;
			}

		}
		public void SetHorCenter()
		{
			if (this.Parent == null) return;
			if ((this.Parent is UiControl) || (this.Parent is UiForm))
			{
				Rectangle r = ((UiControl)this.Parent).TrueClientRect;
				if (this.Width > r.Width) this.Width = r.Width;
				this.Left = (r.Width - this.Width) /2 + r.Left;
			}

		}
		public void SetVerFill()
		{
			if (this.Parent == null) return;
			if ((this.Parent is UiControl) || (this.Parent is UiForm))
			{
				Rectangle r = ((UiControl)this.Parent).TrueClientRect;
				this.Top = r.Top;
				this.Height = r.Height;
			}

		}
		public void SetVerCenter()
		{
			if (this.Parent == null) return;
			if ((this.Parent is UiControl) && (this.Parent is UiForm))
			{
				Rectangle r = ((UiControl)this.Parent).TrueClientRect;
				if (this.Height>r.Height) this.Height = r.Height;
				this.Top = (r.Height - this.Height)/2 + r.Top;
			}

		}

		// ****************************************************************
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
		[BryScript]
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
		[BryScript]
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
		[BryScript]
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
		[BryScript]
		[Browsable(false)]
		public new System.Boolean Enabled
		{
			get { return base.Enabled; }
			set { base.Enabled = value; }
		}
		// **************************************************************
		[BryScript]
		[Browsable(true)]
		public new System.Drawing.Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}
		// **************************************************************
		[BryScript]
		[Category("Color"),Browsable(true)]
		public new System.Drawing.Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}
		// **************************************************************
		[BryScript]
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
		[BryScript]
		[Browsable(false)]
		public new System.Int32 Left
		{
			get { return base.Left; }
			set { base.Left = value; }
		}
		// **************************************************************
		[BryScript]
		[Browsable(false)]
		public new System.Drawing.Point Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}
		// **************************************************************
		[BryScript]
		[Browsable(true)]
		public new System.Windows.Forms.Padding Margin
		{
			get { return base.Margin; }
			set { base.Margin = value; this.Invalidate(); }
		}
		// **************************************************************
		[BryScript]
		[Browsable(true)]
		public new System.Drawing.Size MaximumSize
		{
			get { return base.MaximumSize; }
			set { base.MaximumSize = value; }
		}
		// **************************************************************
		[BryScript]
		[Browsable(true)]
		public new System.Drawing.Size MinimumSize
		{
			get { return base.MinimumSize; }
			set { base.MinimumSize = value; }
		}
		// **************************************************************
		[BryScript]
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
		[BryScript]
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
		[BryScript]
		[Browsable(true)]
		public new System.Object Tag
		{
			get { return base.Tag; }
			set { base.Tag = value; }
		}
		// **************************************************************
		[BryScript]
		[Browsable(true)]
		public new System.String Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}
		// **************************************************************
		[BryScript]
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
		[BryScript]
		[Browsable(false)]
		public new System.Boolean Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}
		// **************************************************************
		[BryScript]
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
		[BryScript]
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
		// ****************************************************************

	}
	public enum SizePolicy
	{
		Fixed = 0,
		Expanding = 1,
	}
	public enum LayoutOrientation
	{
		Vertical = 0,
		Horizon = 1,
	}
}
