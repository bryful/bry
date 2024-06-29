using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace bry
{
	public class HelpList : Control
	{
		private string[] defItems = new string[]
		{
			"write(s)",
			"writeln(s)",
			"cls()",
			"toClip(s)",
			"fromClip()",
			"length(ary)",
			"alert(s)",
			"child(c,a)",
			"childWait(c,a)",
			"executeFile(f)",
			"F.getName(p)",
			"F.getNameWithoutExt(p)",
			"F.getExt(p)",
			"F.getDirectory(p)",
			"F.exists(p)",
			"F.writeText(p)",
			"F.readText(p)",
			"F.rename(s,d)",
			"F.delete(d)",
			"D.getName(p)",
			"D.getNameWithoutExt(p)",
			"D.getExt(p)",
			"D.getDirectory(p)",
			"D.exists(p)",
			"D.rename(s,d)",
			"D.delete(d)",
			"D.current()",
			"D.getFiles(p)",
			"D.getDirectories(p)",
			"FastCopy.pathFastCopy()",
			"FastCopy.setPathFastCopy()",
			"FastCopy.targetFiles()",
			"FastCopy.Clear()",
			"FastCopy.addTarget(p)",
			"FastCopy.addTarget(ary)",
			"FastCopy.dest_dir()",
			"FastCopy.setDest_dir(p)",
			"FastCopy.cmdList()",
			"FastCopy.optionList()",
			"FastCopy.cmd()",
			"FastCopy.setCmd(s)",
			"FastCopy.option()",
			"FastCopy.setOption(s)",
			"FastCopy.exec()"
		};
		private List<SInfo> m_Items = new List<SInfo>();
		private ListBox m_ListBox = new ListBox();
		private TextBox m_TextBox = new TextBox();
		private Button m_Button = new Button();

		public new Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				m_Button.Font = value;
				m_TextBox.Font = value;
				m_ListBox.Font = value;
				ChkSize();
			}
		}
		public HelpList()
		{
			DoubleBuffered = true;
			m_TextBox.BorderStyle = BorderStyle.FixedSingle;
			m_Button.FlatStyle = FlatStyle.Flat;
			m_Button.Text = "X";
			m_ListBox.HorizontalScrollbar = true;
			this.Size = new Size(150, 300);
			Font = base.Font;
			ChkSize();
			this.Controls.Add(m_TextBox);
			this.Controls.Add(m_Button);
			this.Controls.Add(m_ListBox);
		}
		public void ChkSize()
		{
			int x = Margin.Left;
			int y = Margin.Top;
			int w = this.Width -(Margin.Left + Margin.Right);
			int h = this.Height - (Margin.Top + Margin.Bottom);
			m_TextBox.Location = new Point(x,y);
			m_TextBox.Size = new Size(w-20, 21);
			m_Button.Location=new Point(m_TextBox.Right+3,y);
			m_Button.Size = new Size(17 , m_TextBox.Height);
			m_ListBox.Location = new Point(x,m_TextBox.Bottom+3);
			m_ListBox.Size = new Size(w, h - m_TextBox.Bottom - 3);
			m_TextBox.TextChanged += (sender, e) =>
			{
				FindWord(m_TextBox.Text);
			};
			m_Button.Click += (sender, e) => { m_TextBox.Text = ""; };
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkSize();
		}
		private void FindWord(string s)
		{
			m_ListBox.Items.Clear();
			if (m_Items.Count <= 0) return;
			s = s.Trim();
			if (s == "")
			{
				m_ListBox.Items.AddRange(m_Items.ToArray());
				return;
			}

			m_ListBox.SuspendLayout();
			foreach(SInfo s2 in m_Items)
			{
				if (s2.Name.IndexOf(s, StringComparison.OrdinalIgnoreCase)>=0)
				{
					m_ListBox.Items.Add(s2.ToString());
				}
			}
			m_ListBox.ResumeLayout();
		}

		public void SetItems(SInfo[] a)
		{
			m_Items.Clear();
			m_TextBox.Text = "";
			m_ListBox.Items.Clear();
			m_Items.AddRange(a);

			m_ListBox.SuspendLayout();
			foreach(var s in m_Items)
			{
				m_ListBox.Items.Add(s.ToString());
			}
			m_ListBox.ResumeLayout();
		}
	}
}
