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

namespace bry
{
	public class UiControl :Control
	{
		// ***************************************************
		public event EventHandler SizePolicyChanged;
		protected virtual void OnSizePolicyChanged(EventArgs e)
		{
			SizePolicyChanged?.Invoke(this, e);
		}
		// ***************************************************
		private SizePolicy m_SizePolicyHor = SizePolicy.Fixed;
		public SizePolicy SizePolicyHor
		{
			get { return m_SizePolicyHor; }
			set 
			{
				bool b = m_SizePolicyHor != value;
				m_SizePolicyHor = value;
				if(b) OnSizePolicyChanged(new EventArgs());
			}
		}
		private SizePolicy m_SizePolicyVer = SizePolicy.Fixed;
		public SizePolicy SizePolicyVer
		{
			get { return m_SizePolicyVer; }
			set
			{
				bool b = m_SizePolicyVer != value;
				m_SizePolicyVer = value;
				if (b) OnSizePolicyChanged(new EventArgs());
			}
		}
		public UiControl() 
		{
			this.Size = new System.Drawing.Size(100,100);
		}
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
					if ((cc is Panel) || (cc is GroupBox) || (cc is UiLayout))
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
			set { base.Margin = value; }
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
			set { base.Padding = value; }
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
		Fixed,
		Expanding,
	}
	public enum LayoutOrient
	{
		Horizontal,
		Vertical,
		Grid
	}


}
