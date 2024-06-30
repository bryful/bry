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
using System.Reflection;
namespace bry
{
	public class Script
	{
		[ScriptUsage(ScriptAccess.None)]
		public InstallType[] InstallTypes = new InstallType[]
		{
			new InstallType(typeof(Int32),"int"),
			new InstallType(typeof(Single)),
			new InstallType(typeof(Double)),
			new InstallType(typeof(Array),"ArrayCS"),
			new InstallType(typeof(String),"StringCS"),
			new InstallType(typeof(Boolean),"BooleanCS"),
			new InstallType(typeof(DateTime)),
			new InstallType(typeof(Color)),
			new InstallType(typeof(Point)),
			new InstallType(typeof(Size)),
			new InstallType(typeof(Rectangle)),
			new InstallType(typeof(Padding)),
			new InstallType(typeof(UiForm)),
			new InstallType(typeof(UiControl)),
			new InstallType(typeof(UiLayout)),
			new InstallType(typeof(UiSpace)),
			new InstallType(typeof(UiBtn)),
			new InstallType(typeof(UiLabel)),
			new InstallType(typeof(UiListBox)),
			new InstallType(typeof(UiTextBox)),
			new InstallType(typeof(SizePolicy)),
			new InstallType(typeof(LayoutOrientation)),
		};
		
		public V8ScriptEngine engine = null;
		private TextBox m_outputBox = null;
		[ScriptUsage(ScriptAccess.None)]
		public TextBox OutputBox
		{
			get { return m_outputBox; }
			set
			{
				//System.Drawing.Point
				m_outputBox = value;
			}
		}
		private ScriptFile m_File = new ScriptFile();
		private ScriptFolder m_Folder = new ScriptFolder();
		private ScriptFastCopy m_FastCopy = new ScriptFastCopy();
		private UiForm m_UiForm = null;

		[BryScript]
		public object theResult = null;
		// **************************************************
		[ScriptUsage(ScriptAccess.None)]
		public Script(UiForm uiForm=null)
		{
			m_UiForm = uiForm;

		}
		// **************************************************
		[BryScript]
		public void write(object s)
		{
			if (m_outputBox != null)
			{
				m_outputBox.AppendText(objectToString(s));
			}
		}
		[BryScript]
		public void Write(object s)
		{
			write(s);
		}
		[BryScript]
		public void writeln(object s)
		{
			if (m_outputBox != null)
			{
				m_outputBox.AppendText(objectToString(s)+"\r\n");
			}
		}
		[BryScript]
		public void WriteLn(object s)
		{
			writeln(s);
		}
		[BryScript]
		public void writeLn(object s)
		{
			writeln(s);
		}
		[BryScript]
		public void cls()
		{
			if (m_outputBox != null)
			{
				m_outputBox.Text = "";
			}
		}
		[BryScript]
		public void Cls()
		{
			cls();
		}
		[BryScript]
		public void toClip(string s)
		{
			Clipboard.SetText(s);
		}
		[BryScript]
		public string fromClip()
		{
			string ret = "";
			if (Clipboard.ContainsText())
			{
				ret = Clipboard.GetText();
			}
			return ret;
		}
		[BryScript]
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
		[BryScript]
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
		[BryScript]
		public void alert(object o)
		{
			MessageBox.Show(objectToString(o));
		}
		// **************************************************
		[BryScript]
		public bool child(string cmd, string arg)
		{
			return WIN.ProcessStart(cmd, arg);
			//Process.Start("EXPLORER.EXE", m_FullName);
		}
		// **************************************************
		[BryScript]
		public bool childWait(string cmd, string arg)
		{
			return WIN.ProcessStartWait(cmd, arg);
		}
		[BryScript]
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
		[BryScript]
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
			engine.AddHostObject("dotnet", new HostTypeCollection("mscorlib", "System","System.Core"));
			engine.AddHostObject("host", new HostFunctions());


			foreach(InstallType k in InstallTypes)
			{
				if (k.Name =="")
				{
					engine.AddHostType(k.TypeName);
				}
				else
				{
					engine.AddHostType(k.Name,k.TypeName);
				}
			}

			engine.AddHostObject("App", HostItemFlags.GlobalMembers, this);
			engine.AddHostObject("File", HostItemFlags.PrivateAccess, m_File);
			engine.AddHostObject("Dir", HostItemFlags.PrivateAccess, m_Folder);
			engine.AddHostObject("FastCopy", HostItemFlags.PrivateAccess, m_FastCopy);
			if(m_UiForm != null)
			{
				engine.AddHostObject("UI", HostItemFlags.PrivateAccess, m_UiForm);
			}
			
		}
		public object toTest()
		{
			int[] aaa = new int[] { 1, 2, 3, 4, 5, 6, 7 };

			var ret = new Object[aaa.Length];
			for(int i=0; i<aaa.Length;i++)
			{
				ret[i] = aaa[i];
			}
			return ret;
			//return ScriptEngine.Current.Script.Array.from(aaa);
		}
		public SInfo[] GetSInfo()
		{
			int len = 0;
			SInfo[] list = ScriptInfo.Gets(this.GetType(), "");
			len += list.Length;
			SInfo[] list2 = ScriptInfo.Gets(m_File.GetType(), "File");
			len += list2.Length;
			SInfo[] list3 = ScriptInfo.Gets(m_Folder.GetType(), "Dir");
			len += list3.Length;
			SInfo[] list4 = ScriptInfo.Gets(m_FastCopy.GetType(), "FastCopy");
			len += list4.Length;
			SInfo[] list5 = ScriptInfo.Gets(typeof(UiForm), "UI");
			len += list5.Length;
			SInfo[] list6 = ScriptInfo.GetsEnum(typeof(SizePolicy));
			len += list6.Length;
			SInfo[] list7 = ScriptInfo.GetsEnum(typeof(LayoutOrientation));
			len += list7.Length;

			SInfo[] ret = new SInfo[len];

			int idx = 0;
			foreach(SInfo s in list)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list2)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list3)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list4)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list5)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list6)
			{
				ret[idx] = s;
				idx++;
			}
			foreach (SInfo s in list7)
			{
				ret[idx] = s;
				idx++;
			}
			return ret;
		}
		public string GetSInfoToString()
		{
			SInfo[] list = GetSInfo();
			string ret = string.Empty;
			foreach( SInfo s in list )
			{
				ret += s.ToString()+"\r\n";
			}
			return ret;
		}
	}
	
	public class InstallType
	{
		public string Name { get; set; } = "";
		public Type TypeName { get; set; } = null;
		public InstallType(Type ty,string name ="") 
		{
			Name = name;
			TypeName = ty;
		}
	}
	
}
