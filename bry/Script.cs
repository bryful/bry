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
		public ScriptFile m_scriptFile = new ScriptFile();
		public ScriptFolder m_scriptFolder = new ScriptFolder();
		// **************************************************
		public Script()
		{
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
		[ScriptUsage(ScriptAccess.None)]
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
		public void Init()
		{
			if (engine != null) engine.Dispose();
			engine = new V8ScriptEngine();
			//engine.AddHostObject("dotnet", new HostTypeCollection("mscorlib", "System.Core"));

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
			engine.AddHostTypes(new Type[]
			{
				typeof(Enumerable),
				typeof(int),
				typeof(Int32),
				typeof(String),
				typeof(String[]),
				typeof(Array),
				typeof(Boolean),
				typeof(bool),
				typeof(Point),
				typeof(Size),
				typeof(Padding),
				typeof(Rectangle),
				typeof(Color),
				typeof(DateTime),
				typeof(CSForm)
			});
			*/
			engine.AddHostObject("app", HostItemFlags.GlobalMembers, this);
			engine.AddHostObject("File", HostItemFlags.DirectAccess, m_scriptFile);
			engine.AddHostObject("Folder", HostItemFlags.DirectAccess, m_scriptFolder);
		}

	}
}
