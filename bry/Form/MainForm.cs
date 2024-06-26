using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using WeifenLuo.WinFormsUI.Docking;
using System.Diagnostics;
using System.Reflection;

namespace bry
{
	public partial class MainForm : Form
	{
		// ************************************************************************
		public static string GetProps(Type ct)
		{
			// DateTimeのプロパティ一覧を取得する
			PropertyInfo[] p = ct.GetProperties();

			string s = "";
			foreach (var a in p)
			{
				if (a.CanWrite == true)
				{
					s += "// **************************************************************\r\n";
					s += $"[Browsable(false)]\r\n";
					s += $"public new {a.PropertyType.ToString()} {a.Name}\r\n";
					s += $"{{\r\n";
					s += $"	get {{ return base.{a.Name}; }}\r\n";
					s += $"	set {{ base.{a.Name} = value; }}\r\n";
					s += "}\r\n";
				}
			}
			return s;
		}

		// ************************************************************************
		Script Script = new Script();

		// ************************************************************************
		private List<EditorForm> editors = new List<EditorForm>();
		private EditorForm ActiveEditor = null;
		private OutputForm outputForm = null;
		private RefForm refForm = null;
		private UiForm uiForm = null;
		// ************************************************************************
		private ToolStripMenuItem outputMenu = new ToolStripMenuItem();
		private ToolStripMenuItem refMenu = new ToolStripMenuItem();
		private ToolStripMenuItem uiMenu = new ToolStripMenuItem();
		// ************************************************************************
		[ScriptUsage(ScriptAccess.None)]
		public MainForm()
		{
			InitializeComponent();
			dockPanel1.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
			outputMenu.Text = "Output";
			refMenu.Text = "Reference";
			uiMenu.Text = "UI";

			outputForm = new OutputForm();
			outputForm.Text = "Output";
			outputForm.HideOnClose = true;
			Script.OutputBox = outputForm.Output;
			refForm = new RefForm();
			refForm.Text = "Reference";
			refForm.HideOnClose = true;
			
			uiForm = new UiForm();
			uiForm.Text = "UI";
			uiForm.HideOnClose = true;

			windowMenu.Click += (sender, e) =>
			{
				MakeWindowMenu();
			};
			outputMenu.Click += (sender, e) =>
			{
				if (outputForm.IsHidden)
				{
					outputForm.Show();
				}
				else
				{
					outputForm.Hide();
				}
			};
			refMenu.Click += (sender, e) =>
			{
				if (refForm.IsHidden)
				{
					refForm.Show();
				}
				else
				{
					refForm.Hide();
				}
			};
			uiMenu.Click += (sender, e) =>
			{
				if (uiForm.IsHidden)
				{
					uiForm.Show();
				}
				else
				{
					uiForm.Hide();
				}
			};
			editorFontMenu.Click += (sender, e) =>{EditorFontDialog();};
			outputFontMenu.Click += (sender, e) => { OutputFontDialog(); };
			referenceFontMenu.Click += (sender, e) => { RefFontDialog(); };
			quitMenu.Click += (sender, e) =>
			{
				Application.Exit();
			};
			newMenu.Click += (sender, e) =>
			{
				NewEditor();
			};
			closeMenu.Click += (sender, e) =>
			{
				CloseEditor();
			};
			executeMenu.Click += (sender, e) =>
			{
				Exec();
			};
			initEngeineMenu.Click += (sender, e) =>
			{
				InitEngine();
			};
			InitEngine();


			Clipboard.SetText(GetProps(typeof(UiBtn)));
		}
		// ************************************************************************
		private string m_EditorFontFamily = "源ノ角ゴシック Code JP R";
		public string EditorFontFamily
		{
			get
			{
				return m_EditorFontFamily;
			}
			set
			{
				m_EditorFontFamily = value;
				if(editors.Count>0)
				{
					for (int i = 0;i<editors.Count; i++)
					{
						
						editors[i].FontFamily = value;
					}
				}
			}
		}
		private double m_EditorFontSize = 12;
		public double EditorFontSize
		{
			get
			{
				return m_EditorFontSize;
			}
			set
			{
				m_EditorFontSize = value;
				if (editors.Count > 0)
				{
					for (int i = 0; i < editors.Count; i++)
					{

						editors[i].FontSize = value;
					}
				}
			}
		}
		// ************************************************************************
		private string[] editorTexts()
		{
			List<string> ret = new List<string>();
			if(editors.Count > 0 )
			{
				foreach( var editor in editors )
				{
					ret.Add( editor.editor.Text );
				}
			}
			return ret.ToArray();
		}
		// ************************************************************************
		private void SetEditorTexts(string[] sa)
		{
			DisposeEditors();
			if ( sa.Length > 0 )
			{
				for (int i = 0; i < sa.Length; i++)
				{
					EditorForm ef = NewEditor();
					ef.editor.Text = sa[i];
				}

			}
		}
		// ************************************************************************
		public void DisposeEditors()
		{
			if (editors.Count > 0)
			{
				for (int i = 0; i < editors.Count; i++)
				{
					editors[i].Dispose();
				}
				editors.Clear();
			}
		}
		// ************************************************************************
		public EditorForm NewEditor()
		{
			EditorForm ef = new EditorForm();
			ef.HideOnClose = true;
			
			ef.FontFamily = m_EditorFontFamily;
			ef.FontSize = m_EditorFontSize;
			ef.Text = $"Editor{editors.Count + 1}";
			editors.Add( ef );
			
			ef.Enter += (sender, e) =>
			{
				ActiveEditor = (EditorForm)sender;
			};
			for (int i = 0; i < editors.Count; i++)
			{
				editors[i].Index = i;
			}
			ef.Show(dockPanel1,DockState.Document);
			ActiveEditor = editors[editors.Count - 1];

			return ef;
		}
		public void CloseEditor()
		{
			if (ActiveEditor == null) return;

			int idx = ActiveEditor.Index;
			editors[idx].Dispose();
			editors.RemoveAt(idx);
			if (editors.Count > 0)
			{
				for (int i = 0; i < editors.Count; i++)
				{
					editors[i].Index = i;
				}
				if (idx>=editors.Count) idx = editors.Count - 1;

				editors[idx].Activate();
				ActiveEditor = editors[idx];
			}
			else
			{
				ActiveEditor = null;
			}

		}
		// ************************************************************************
		private void MakeWindowMenu()
		{
			windowMenu.DropDownItems.Clear();
			if (editors.Count > 0)
			{
				for(int i = 0;i < editors.Count;i++)
				{
					editors[i].Index = i;
					ToolStripMenuItem mi = new ToolStripMenuItem(editors[i].Text);
					mi.Checked = ! editors[i].IsHidden;
					mi.Tag = editors[i];
					mi.Click += (sender, e) =>
					{
						EditorForm ef = (EditorForm)((ToolStripMenuItem)sender).Tag;
						if (ef.IsHidden)
						{
							ef.Show();
						}
						else
						{
							ef.Hide();
						}
					};
					windowMenu.DropDownItems.Add( mi );
				}
			}
			outputMenu.Checked = ! outputForm.IsHidden;
			refMenu.Checked = !refForm.IsHidden;
			uiMenu.Checked = !uiForm.IsHidden;

			windowMenu.DropDownItems.Add(uiMenu);
			windowMenu.DropDownItems.Add(outputMenu);
			windowMenu.DropDownItems.Add(refMenu);
		}
		// ************************************************************************
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			PrefFile pf = new PrefFile(this);
			pf.Load();

			bool ok = false;
			object v;
			v = pf.GetValueStringArray("Codes", out ok);
			if (ok) 
			{
				SetEditorTexts((string[])v);
			}
			else
			{
				NewEditor();
			}

			Font of = pf.GetValueFont("OutputFont", out ok);
			if (ok)
			{
				outputForm.OutputFont = of;
			}
			Font rf = pf.GetValueFont("RefFont", out ok);
			if (ok)
			{
				refForm.RefFont = of;
			}
			


			string p = Path.Combine( pf.FileDirectory ,pf.AppName+".xml");
			LayoutLoad(p);
			v = pf.GetValueString("EditorFont", out ok);
			if (ok)
			{
				EditorFontFamily = (string)v;
			}
			v = pf.GetValueDouble("EditorFontSize", out ok);
			if (ok)
			{
				EditorFontSize = (double)v;
			}
			pf.RestoreForm();
		}
		private bool LayoutLoad(string p)
		{
			bool ret = false;
			try
			{
				DeserializeDockContent deserializeDockContent
					= new DeserializeDockContent(GetDockContent);
				dockPanel1.LoadFromXml(p, deserializeDockContent);
				ret = true;
			}
			catch (Exception ee)
			{
				uiForm.Show(dockPanel1, DockState.Document);
				if (editors.Count==0) NewEditor();
				if (editors.Count>0) 
				{
					for(int i=0; i<editors.Count; i++)
					{
						editors[i].Show(dockPanel1);
					}
				}
				outputForm.Show(dockPanel1,DockState.DockBottom);
				refForm.Show(dockPanel1,DockState.DockRight);
				uiForm.Hide();
				ret = false;
			}
			return ret;
		}
		private IDockContent GetDockContent(string persistString)
		{
			// フォームのクラス名で保存されているのでそれから判定してあげる
			if (persistString.IndexOf("EditorFrom_")==0)
			{
				string[] sa = persistString.Split('_');
				int idx = -1;
				if (int.TryParse(sa[1],out idx))
				{
					return editors[idx];
				}
			}
			else if (persistString.Equals("OutputForm"))
			{
				return outputForm;
			}
			else if (persistString.Equals("RefForm"))
			{
				return refForm;
			}
			else if (persistString.Equals("UiForm"))
			{
				return uiForm;
			}
			return null;
		}
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PrefFile pf = new PrefFile(this);

			pf.SetValue("Codes", editorTexts());
			pf.SetValue("OutputFont", outputForm.OutputFont);
			pf.SetValue("RefFont", refForm.RefFont);
			pf.SetValue("EditorFont", m_EditorFontFamily);
			pf.SetValue("EditorFontSize", m_EditorFontSize);
			pf.StoreForm();
			pf.Save();

			string p = Path.Combine(pf.FileDirectory, pf.AppName + ".xml");
			dockPanel1.SaveAsXml(p);


			DisposeEditors();
			if (outputForm != null)
			{
				outputForm.Dispose();
			}
			if (refForm != null)
			{
				refForm.Dispose();
			}
		}
		// ************************************************************************
		public void Exec()
		{
			if(ActiveEditor != null)
			{
				Script.ExecuteCode(ActiveEditor.editor.Text);
			}
		}
		// ************************************************************************
		public void InitEngine()
		{
			Script.Init(uiForm);
		}
		// ************************************************************************
		public void EditorFontDialog()
		{
			using(FontDialog dlg = new FontDialog())
			{
				dlg.Font = new Font(m_EditorFontFamily,(float)m_EditorFontSize);
				if (dlg.ShowDialog()== DialogResult.OK)
				{
					EditorFontFamily = dlg.Font.Name;
					EditorFontSize = (double)dlg.Font.Size;
				}
			}
		}
		public void OutputFontDialog()
		{
			using (FontDialog dlg = new FontDialog())
			{
				dlg.Font = outputForm.OutputFont;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					outputForm.OutputFont = dlg.Font;
				}
			}
		}
		public void RefFontDialog()
		{
			using (FontDialog dlg = new FontDialog())
			{
				dlg.Font = refForm.RefFont;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					refForm.RefFont = dlg.Font;
				}
			}
		}
		// ************************************************************************

	}
}
