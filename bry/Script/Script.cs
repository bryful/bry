using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;

namespace bry
{
	public class Script
	{
		public V8ScriptEngine engine = null;
		private TextBox m_outputBox = null;
		[ScriptUsage(ScriptAccess.None)]
		public TextBox OutputBox
		{
			get { return m_outputBox; }
			set
			{
				m_outputBox = value;
			}
		}
		private ScriptFile m_File = new ScriptFile();
		private ScriptFolder m_Folder = new ScriptFolder();
		private ScriptFastCopy m_FastCopy = new ScriptFastCopy();
		private UiForm m_UiForm = null;
		// **************************************************
		[ScriptUsage(ScriptAccess.None)]
		public Script(UiForm uiForm=null)
		{
			m_UiForm = uiForm;
		}
		// **************************************************
		public void write(object s)
		{
			if (m_outputBox != null)
			{
				m_outputBox.AppendText(objectToString(s));
			}
		}
		public void Write(object s)
		{
			write(s);
		}
		public void writeln(object s)
		{
			if (m_outputBox != null)
			{
				m_outputBox.AppendText(objectToString(s)+"\r\n");
			}
		}
		public void WriteLn(object s)
		{
			writeln(s);
		}
		public void writeLn(object s)
		{
			writeln(s);
		}
		public void cls()
		{
			if (m_outputBox != null)
			{
				m_outputBox.Text = "";
			}
		}
		public void Cls()
		{
			cls();
		}
		public void toClip(string s)
		{
			Clipboard.SetText(s);
		}
		public string fromClip()
		{
			string ret = "";
			if (Clipboard.ContainsText())
			{
				ret = Clipboard.GetText();
			}
			return ret;
		}
		public int length(object o)
		{
			int ret = -1;
			if ((o != null)&& (o is Array))
			{
				ret = ((Array)o).Length;
			}
			return ret;
		}

		
		// **************************************************
		public string toHex2(byte ub)
		{

			string ret = $"{ub:X2}".ToUpper();
			return ret;
		}
		public string toHex2(long l)
		{
			byte ub = (byte)(l &0xFF);
			string ret = $"{ub:X2}".ToUpper();
			return ret;
		}
		public string toHex(int l,int len=2,bool sw=false)
		{
			if (len < 1) len = 1; else if (len > 4) len = 4;
			string ret = "";

			int l2 = l;

			if (sw == false)
			{
				for (int i = len - 1; i >= 0; i--)
				{
					byte b = (byte)((l2 >> (i * 8)) & 0xFF);
					ret += toHex2(b);
				}
			}
			else
			{
				for (int i = 0; i <len; i++)
				{
					byte b = (byte)((l2 >> (i * 8)) & 0xFF);
					ret += toHex2(b);
				}
			}
			return ret;
		}
		// **************************************************
		public void alert(object o)
		{
			MessageBox.Show(objectToString(o));
		}
		// **************************************************
		public bool child(string cmd, string arg)
		{
			return WIN.ProcessStart(cmd, arg);
			//Process.Start("EXPLORER.EXE", m_FullName);
		}
		// **************************************************
		public bool childWait(string cmd, string arg)
		{
			return WIN.ProcessStartWait(cmd, arg);
		}
		public void executeFile(string p)
		{
			Process.Start("EXPLORER.EXE", p);
		}
		// **************************************************
		// **************************************************
		[ScriptUsage(ScriptAccess.None)]
		public void ExecuteCode(string s)
		{
			if (engine == null) Init();
			try
			{
				engine.Execute(s);
			}
			catch (Exception e) when (e is IScriptEngineException ex)
			{
			
				writeln(ex.ErrorDetails);
			}
		}
		// **************************************************
		static public string ScriptObjectStr(Object so)
		{
			string ret = "";
			if (so == null) return "null";
			if (so is string)
			{
				ret = "\"" + (string)so + "\"";
			}
			else if (so is Boolean)
			{
				ret = ((Boolean)so).ToString().ToLower();
			}
			else if (so is ScriptObject)
			{
				ScriptObject soo = (ScriptObject)so;
				if (soo.GetType().ToString().IndexOf("Array") >= 0)
				{
					foreach (int idx in soo.PropertyIndices)
					{
						if (ret != "") ret += ",";
						ret += ScriptObjectStr(soo.GetProperty(idx));
					}
					ret = "[" + ret + "]";
				}
				else
				{
					foreach (string n in soo.PropertyNames)
					{
						if (ret != "") ret += ",";
						object aa = soo.GetProperty(n);
						string nm = ScriptObjectStr(aa);
						ret += $"{n}:{nm}";
					}
					ret = "{" + ret + "}";

				}
			}
			else
			{
				ret = objectToString(so);
			}
			return ret;
		}
		// ************************************************************************
		static public string objectToString(object obj)
		{
			string ret = "";

			if (obj == null)
			{
				ret = "null";
			}
			else if (obj is string)
			{
				ret = (string)obj;
			}
			else if (obj is ScriptObject)
			{
				ret = ScriptObjectStr((ScriptObject)obj);
			}
			else if (obj is bool)
			{
				ret = obj.ToString().ToLower();
			}
			else if (obj is Array)
			{
				foreach (object o1 in (Array)obj)
				{
					if (ret != "") ret += ",";
					if (o1 == null)
					{
						ret += "null";
					}
					else
					{
						ret += objectToString(o1);
					}
				}
				ret = "[" + ret + "]";
			}
			else
			{
				try
				{
					ret = obj.ToString();

				}
				catch (Exception ex)
				{
					ret = ex.ToString();
				}
			}
			if (ret == null) { ret = "null"; }
			return ret;
		}
		// ************************************************************************
		[ScriptUsage(ScriptAccess.None)]
		public void Init(UiForm uif = null)
		{
            if (uif!=null)
            {
				m_UiForm = uif;
            }
            if (engine != null) engine.Dispose();
			engine = new V8ScriptEngine();
			engine.AddHostObject("dotnet", new HostTypeCollection("mscorlib", "System.Core"));

			/*
			var typeCollection = new HostTypeCollection(
				"mscorlib",
				"System",
				"System",
				"System.Core",
				"System.Drawing",
				"System.IO",
				"System.Collections",
				"System.Windows.Forms");

			engine.AddHostObject("dotnet", typeCollection);
			*/
			engine.AddHostTypes(new Type[]
			{
				typeof(Enumerable),
				typeof(int),
				typeof(Int32),
				typeof(double),
				typeof(float),
				typeof(String),
				typeof(String[]),
				typeof(Array),
				typeof(Boolean),
				typeof(bool),
				typeof(bool[]),
				typeof(DateTime),
				typeof(Point),
				typeof(Color),
				typeof(Size),
				typeof(Rectangle),
				typeof(Padding),
				typeof(UiForm),
				typeof(UiControl),
				typeof(UiHLayout),
				typeof(UiVLayout),
				typeof(UiSpace),
				typeof(UiBtn),
				typeof(UiLabel),
				typeof(SizePolicy),
				typeof(StringAlignment),

			});
			engine.AddHostObject("App", HostItemFlags.GlobalMembers, this);
			engine.AddHostObject("F", HostItemFlags.PrivateAccess, m_File);
			engine.AddHostObject("D", HostItemFlags.PrivateAccess, m_Folder);
			engine.AddHostObject("FastCopy", HostItemFlags.PrivateAccess, m_FastCopy);

			if(m_UiForm != null)
			{
				engine.AddHostObject("UI", HostItemFlags.PrivateAccess, m_UiForm);
			}
		}

	}
}
