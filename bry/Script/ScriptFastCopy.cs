using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace bry
{
	public class ScriptFastCopy
	{
		private string m_FastCopy = "C:\\Bin\\FastCopy\\FastCopy.exe";

		[BryScript]
		public string pathFastCopy()
		{
			return m_FastCopy;
		}
		[BryScript]
		public bool setPathFastCopy(string p)
		{
			if (File.Exists(p) == true)
			{
				m_FastCopy = p;
				return true;
			}else
			{
				return false;
			}
		}
		private List<string> m_Files = new List<string>();
		private string Waku(string p)
		{
			p = p.Trim();
			if (p.Length == 0) return p;
			if (p[0]!='"')
			{
				p = "\"" + p;
			}
			if (p[p.Length-1] !='"')
			{
				p =  p + "\"";
			}
			return p;
		}
		[BryScript]
		public string targetFiles()
		{
			string ret = "";
			if (m_Files.Count > 0)
			{
				for (int i = 0; i < m_Files.Count; i++)
				{
					if (ret!="") ret += " ";
					ret += Waku(m_Files[i]);
				}
			}
			return ret;
		}
		// *************************************************************
		[BryScript]
		public void Clear()
		{
			m_Files.Clear();
		}
		// *************************************************************
		private int FindTarget(string p)
		{
			int ret = -1;
			if(m_Files.Count>0)
			{
				for(int i = 0; i < m_Files.Count; i++)
				{
					if (m_Files[i] ==p)
					{
						ret = i; 
						break;
					}
				}
			}
			return ret;
		}
		// *************************************************************
		[BryScript]
		public void addTarget(string p)
		{
			string n = "";
			if (File.Exists (p)==false)
			{
				if (Directory.Exists (p)==false)
				{
					return;
				}
				else
				{
					n = (new DirectoryInfo(p)).FullName;
				}
			}
			else
			{
				n = (new FileInfo(p)).FullName;
			}
			int idx = FindTarget(n);
			if (idx < 0)
			{
				m_Files.Add(n);
			}
		}
		[BryScript]
		public void addTarget(string [] ps)
		{

			if(ps.Length>0)
			{
				for(int i = 0;i<ps.Length;i++)
				{
					addTarget(ps[i]);
				}
			}
		}
		// *************************************************************
		private string m_dest_dir = "";
		[BryScript]
		public string dest_dir()
		{
			return m_dest_dir;
		}
		[BryScript]
		public void setDest_dir(string value)
		{
			if (value.Length<=1) return;
			if (value[value.Length - 1] == '\\')
				value = value.Substring(0, value.Length - 1);
			if (Directory.Exists(value))
			{
				m_dest_dir = value + "\\";
			}
			else
			{
				m_dest_dir = "";
			}
		}
		private string[] m_cmdlist = new string[]
		{
			"diff",
			"update",
			"force_copy",
			"exist_diff",
			"exist_update",
			"sync",
 			"sync_update",
			"move",
			"move_noexist",
			"delete",
			"verify",
			"verify_read",
			"verify_check"
		};
		[BryScript]
		public string cmdList()
		{
			return string.Join(",\r\n", m_cmdlist) + "\r\n";
		}
		private string[] m_optionList = new string[]
		{
			"/auto_close",
			"/force_close",
			"/open_window",
			"/estimate",
			"/estimate=FALSE",
			"/balloon",
			"/balloon=FALSE",
			"/no_ui",
			"/no_confirm_del",
			"/no_confirm_stop",
			"/no_exec",
			"/error_stop",
			"/error_stop=FALSE",
			"/bufsize=N(MB)",
			"/log",
			"/log=FALSE",
			"/logfile=aaa.log",
			"/filelog",
			"/skip_empty_dir",
			"/acl",
			"/acl=FALSE",
			"/stream",
			"/stream=FALSE"
		};
		[BryScript]
		public string optionList()
		{
			return string.Join(",\r\n", m_cmdlist) + "\r\n";
		}
		// *************************************************************
		private string m_cmd = "/cmd=diff";
		[BryScript]
		public string cmd() {  return m_cmd; }
		[BryScript]
		public void setCmd(string s) {  m_cmd = s; }
		private string m_option = "/open_window /auto_close /estimate";
		//"C:\Bin\FastCopy\FastCopy.exe" /open_window /estimate /auto_close /logfile="$O\copy.log" $MF /to="$O\"
		[BryScript]
		public string option() { return m_option; }
		[BryScript]
		public void setOption(string s) { m_option = s; }

		// *************************************************************
		public ScriptFastCopy()
		{

		}
		[BryScript]
		public bool exec()
		{
			bool ret = false;
			if(m_Files.Count <= 0) return ret;
			if(m_dest_dir=="") return ret;

			string arg = "";
			if (m_cmd == "") m_cmd = "/cmd=diff";
			arg += m_cmd;
			if (m_option!="")
			{
				arg += " " + m_option;
			}
			arg += " " + targetFiles();
			arg += " /to=" + Waku(m_dest_dir);

			ret = WIN.ProcessStart(m_FastCopy,arg);
			return ret;
		}
	}
}
