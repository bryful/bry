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
	public class UiListBox : UiControl
	{
		public event EventHandler SelectedIndexChanged;
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(this, e);
			}
		}
		protected ListBox m_ListBox = new ListBox();
		public ListBox ListBox
		{
			get { return m_ListBox;}
		}
		public ListBox.ObjectCollection Items
		{
			get { return m_ListBox.Items; }
		}
		public int SelectedIndex
		{
			get { return (int)m_ListBox.SelectedIndex; }
			set
			{
				m_ListBox.SelectedIndex = value;
			}
		}
		public ListBox.SelectedIndexCollection SelectedIndices
		{
			get { return m_ListBox.SelectedIndices; }
		}
		public object SelectedItem
		{
			get { return m_ListBox.SelectedItem; }
			set
			{
				m_ListBox.SelectedItem = value;
			}
		}
		public ListBox.SelectedObjectCollection SelectedItems
		{
			get { return m_ListBox.SelectedItems; }
		}
		public System.Windows.Forms.SelectionMode SelectionMode
		{
			get { return m_ListBox.SelectionMode; }
			set
			{
				m_ListBox.SelectionMode = value;
			}
		}
		public bool Sorted
		{
			get { return m_ListBox.Sorted; }
			set
			{
				m_ListBox.Sorted = value;
			}
		}
		public int TopIndex
		{
			get { return m_ListBox.TopIndex; }
			set
			{
				m_ListBox.TopIndex = value;
			}
		}
		public int ColumnWidth
		{
			get { return m_ListBox.ColumnWidth; }
			set
			{
				m_ListBox.ColumnWidth = value;
			}
		}
		public ListBox.IntegerCollection CustomTabOffsets
		{
			get { return m_ListBox.CustomTabOffsets; }
		}
		public int HorizontalExtent
		{
			get { return m_ListBox.HorizontalExtent; }
			set
			{
				m_ListBox.HorizontalExtent = value;
			}
		}
		public bool HorizontalScrollbar
		{
			get { return m_ListBox.HorizontalScrollbar; }
			set
			{
				m_ListBox.HorizontalScrollbar = value;
			}
		}
		public bool IntegralHeight
		{
			get { return m_ListBox.IntegralHeight; }
			set
			{
				m_ListBox.IntegralHeight = value;
			}
		}
		public int ItemHeight
		{
			get { return m_ListBox.ItemHeight; }
			set
			{
				m_ListBox.ItemHeight = value;
			}
		}
		public bool MultiColumn
		{
			get { return m_ListBox.MultiColumn; }
			set
			{
				m_ListBox.MultiColumn = value;
			}
		}
		public bool ScrollAlwaysVisible
		{
			get { return m_ListBox.ScrollAlwaysVisible; }
			set
			{
				m_ListBox.ScrollAlwaysVisible = value;
			}
		}
		[Browsable(true)]
		public new System.Drawing.Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				m_ListBox.Font = value;
				ChkSize();
			}
		}
		// **************************************************************
		[Category("UI"), Browsable(true)]
		public new float FontSize
		{
			get { return base.FontSize; }
			set
			{
				base.FontSize = value;
				m_ListBox.Font = base.Font;
				ChkSize();
				this.Invalidate();
			}
		}
		public UiListBox()
		{
			m_ListBox.Size = new Size(100, 25);
			m_ListBox.Location = new Point(Margin.Left, Margin.Top);
			base.Size = new Size(
				m_ListBox.Width + Margin.Left + Margin.Right,
				m_ListBox.Height + Margin.Top + Margin.Bottom
				);
			this.Controls.Add(m_ListBox);
			m_ListBox.SelectedIndexChanged+=(sender,e)=>
			{
				OnSelectedIndexChanged(e);
			};
			ChkSize();
		}
		private void ChkSize()
		{
			m_ListBox.Size = new Size(
				this.Width - (Margin.Left + Margin.Right),
				this.Height - (Margin.Top + Margin.Bottom)
				);
			m_ListBox.Location = new Point(
				Margin.Left, Margin.Top);
		}
		protected override void OnResize(EventArgs e)
		{
			ChkSize();
			base.OnResize(e);
		}
	}
}
