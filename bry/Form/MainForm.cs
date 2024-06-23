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
namespace bry
{
	public partial class MainForm : Form
	{
		Script Script = new Script();
		private EditorForm editorForm = null;
		private OutputForm outputForm = null;
		private RefForm refForm = null;

		// ************************************************************************
		// ************************************************************************
		[ScriptUsage(ScriptAccess.None)]
		public MainForm()
		{
			InitializeComponent();
			dockPanel1.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
			editorForm = new EditorForm();
			editorForm.Text = "Editor";
			editorForm.HideOnClose = true;
			//editorForm.Show(dockPanel1);

			outputForm = new OutputForm();
			outputForm.Text = "Output";
			outputForm.HideOnClose = true;
			//outputForm.Show(dockPanel1);
			//outputForm.Hide();

			refForm = new RefForm();
			refForm.Text = "Reference";
			refForm.HideOnClose = true;
			//refForm.Show(dockPanel1);
			//refForm.Hide();

			Script.OutputBox = outputForm.Output;
			executeMenu.Click += (sender, e) => { Exec(); };
			initEngineMenu.Click += (sender, e) => { InitEngine(); };

			editorMenu.Click += (sender, e) =>
			{
				if (editorForm.IsHidden)
				{
					editorForm.Show();
				}
				else
				{
					editorForm.Hide();
				}
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
			fontMenu.Click += (sender, e) =>
			{
				FontSettingDialog();
			};
			quitMenu.Click += (sender, e) =>
			{
				Application.Exit();
			};
		}
		// ************************************************************************
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			PrefFile pf = new PrefFile(this);
			pf.Load();
			pf.RestoreForm();

			bool ok = false;
			object v;
			v = pf.GetValueString("Code", out ok);
			if (ok) { editorForm.editor.Text = (string)v; }

			string nf = pf.GetValueString("FontFamily", out ok);
			if (ok == false)
			{
				nf = this.Font.Name;
			}
			double sf = pf.GetValueDouble("FontSize", out ok);
			if (ok == false)
			{
				sf = 12;
			}
			editorForm.Font = new Font(nf, (float)sf);
			outputForm.Font = editorForm.Font;
			refForm.Font = editorForm.Font;
			string p = Path.Combine( pf.FileDirectory ,pf.AppName+".xml");
			LayoutLoad(p);

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
				editorForm.Show(dockPanel1);
				outputForm.Show(dockPanel1);
				refForm.Show(dockPanel1);
				ret = false;
			}
			return ret;
		}
		private IDockContent GetDockContent(string persistString)
		{
			// フォームのクラス名で保存されているのでそれから判定してあげる
			if (persistString.Equals(typeof(EditorForm).ToString()))
			{
				return editorForm;
			}
			else if (persistString.Equals(typeof(OutputForm).ToString()))
			{
				return outputForm;
			}
			else if (persistString.Equals(typeof(RefForm).ToString()))
			{
				return refForm;
			}
			return null;
		}
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PrefFile pf = new PrefFile(this);
			pf.SetValue("Code", editorForm.editor.Text);
			pf.SetValue("FontFamily", editorForm.Font.Name);
			pf.SetValue("FontSize", (double)editorForm.Font.Size);
			pf.StoreForm();
			//pf.SetValue("Border", splitContainer1.SplitterDistance);
			pf.Save();

			string p = Path.Combine(pf.FileDirectory, pf.AppName + ".xml");
			dockPanel1.SaveAsXml(p);

			if (editorForm != null)
			{
				editorForm.Dispose();
			}
			if(outputForm != null)
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
			Script.ExecuteCode(editorForm.editor.Text);
		}
		// ************************************************************************
		public void InitEngine()
		{
			Script.Init();
		}
		// ************************************************************************
		public void FontSettingDialog()
		{
			using(FontDialog dlg = new FontDialog())
			{
				dlg.Font = editorForm.Font;
				if (dlg.ShowDialog()== DialogResult.OK)
				{
					editorForm.Font = dlg.Font;
					outputForm.Font = dlg.Font;
					refForm.Font = dlg.Font;
				}
			}
		}
		// ************************************************************************

	}
}
