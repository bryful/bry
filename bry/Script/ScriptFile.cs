using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
namespace bry
{
	public class ScriptFile
	{
		public string getName(string p)
		{
			FileInfo fi = new FileInfo(p);
			return fi.Name;
		}
		public string getNameWithoutExt(string p)
		{
			string ret = "";
			FileInfo fi = new FileInfo(p);
			int idx = fi.Name.LastIndexOf('.');
			if (idx > 0)
			{
				ret = fi.Name.Substring(0, idx);
			}
			return ret;
		}
		public string getExt(string p)
		{
			string ret = "";
			FileInfo fi = new FileInfo(p);
			int idx = fi.Name.LastIndexOf('.');
			if (idx > 0)
			{
				ret = fi.Name.Substring(idx);
			}
			return ret;
		}
		public string getDirectory(string p)
		{
			string ret = "";
			FileInfo fi = new FileInfo(p);
			ret = fi.Directory.FullName;
			return ret;
		}
		public bool exists(string path)
		{
			FileInfo fi =  new FileInfo(path);
			return fi.Exists;
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
		public bool rename(string s,string d)
		{
			bool ret = false;
			if ((s == "") || (d == "") || (s == d)) return ret;

			try
			{
				FileInfo fis = new FileInfo(s);
				if (fis.Exists == false) return ret;
				FileInfo fid = new FileInfo(d);
				if (fid.Exists == true) return ret;
				if(fis.FullName == fid.FullName) return ret;
				fis.MoveTo(fid.FullName);
				ret = fid.Exists;
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		public bool delete(string s)
		{
			bool ret = false;
			if (s == "") return ret;

			try
			{
				if (File.Exists(s) == false) return ret;
				// エラーダイアログのみ表示する
				FileSystem.DeleteFile(s,
					UIOption.OnlyErrorDialogs,
					RecycleOption.SendToRecycleBin);
				ret = (File.Exists(s) == false);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}

		public ScriptFile() 
		{
		}
	}
}
