using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

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
			editorForm.Show(dockPanel1);

			outputForm = new OutputForm();
			outputForm.Text = "Output";
			outputForm.HideOnClose = true;
			outputForm.Show(dockPanel1);

			refForm = new RefForm();
			refForm.Text = "Reference";
			refForm.HideOnClose = true;
			refForm.Show(dockPanel1);

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
		}
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PrefFile pf = new PrefFile(this);
			pf.SetValue("Code", editorForm.editor.Text);
			pf.StoreForm();
			//pf.SetValue("Border", splitContainer1.SplitterDistance);
			pf.Save();

			if(editorForm != null)
			{
				editorForm.Dispose();
			}
			if(outputForm != null)
			{
				outputForm.Dispose();
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

		// ************************************************************************

	}
}
