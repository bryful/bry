using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace bry
{
	public class ScriptFile
	{
		public bool Exists(string path)
		{
			return File.Exists(path);
		}
		public bool writeText(string p,string s)
		{
			bool ret = false;
			try
			{
				if (File.Exists(p))
				{
					File.Delete(p);
				}
				File.WriteAllText(p,s);
				ret = File.Exists(p);
			}catch
			{
				ret = false;
			}
			return ret;

		}
		public string readText(string p)
		{
			string ret = null;
			try
			{
				if (File.Exists(p))
				{
					ret = File.ReadAllText(p);
				}
			}
			catch
			{
				ret = null;
			}
			return ret;
		}
		public ScriptFile() 
		{
		}
	}
}
