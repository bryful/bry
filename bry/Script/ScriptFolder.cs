using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.VisualBasic.FileIO;

namespace bry
{
	public class ScriptFolder
	{
		[BryScript]
		public string getName(string p)
		{
			FileInfo fi = new FileInfo(p);
			return fi.Name;
		}
		[BryScript]
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
		private int NumStart(string s)
		{
			int ret = -1;
			int cnt = s.Length;
			if (cnt != 0)
			{
				for (int i = cnt - 1; i >= 0; i++)
				{
					char c = s[i];
					if (c >= '0' && c <= '9')
					{

					}
					else
					{
						if (i == cnt - 1)
						{
							break;
						}
						else
						{
							ret = i + 1;
							break;
						}
					}
				}
			}
			return ret;
		}
		[BryScript]
		public string getFrame(string p)
		{
			string ret = "";
			FileInfo fi = new FileInfo(p);
			int idx = fi.Name.LastIndexOf('.');
			if (idx > 0)
			{
				ret = fi.Name.Substring(0, idx);
				idx = NumStart(ret);
				if (idx >= 0)
				{
					ret = ret.Substring(idx);
				}
				else
				{
					ret = "";
				}
			}
			return ret;
		}
		[BryScript]
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
		[BryScript]
		public string getDirectory(string p)
		{
			string ret = "";
			FileInfo fi = new FileInfo(p);
			ret = fi.Directory.FullName;
			return ret;
		}
		private static string GetSafeTempName(string outputFilePath)
		{
			outputFilePath += "_";
			while (File.Exists(outputFilePath))
			{
				outputFilePath += "_";
			}
			return outputFilePath;
		}
		[BryScript]
		public bool rename(string s, string d)
		{
			bool ret = false;
			if ((s == "") || (d == "") || (s == d)) return ret;

			try
			{
				DirectoryInfo fis = new DirectoryInfo(s);
				if (fis.Exists == false) return ret;
				DirectoryInfo fid = new DirectoryInfo(d);
				if (fid.Exists == true) return ret;
				if (fis.FullName == fid.FullName) return ret;
				if (string.Compare(fis.Name, fid.Name, StringComparison.OrdinalIgnoreCase)==0)
				{
					string temp = GetSafeTempName(fid.FullName);
					fis.MoveTo(temp);
					Directory.Move(temp,fid.FullName);
				}
				else
				{
					fis.MoveTo(fid.FullName);
				}
				ret = fid.Exists;
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		[BryScript]
		public bool delete(string s)
		{
			bool ret = false;
			if (s == "") return ret;

			try
			{
				if(Directory.Exists(s)==false) return ret;
				// エラーダイアログのみ表示する
				FileSystem.DeleteFile(s, 
					UIOption.OnlyErrorDialogs, 
					RecycleOption.SendToRecycleBin);
				/*
				DirectoryInfo fis = new DirectoryInfo(s);
				if (fis.Exists == false) return ret;
				fis.Delete(true);
				*/
				ret = (Directory.Exists(s) == false);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		[BryScript]
		public string current()
		{
			return Directory.GetCurrentDirectory();

		}
		[BryScript]
		public void setCurrent(string p)
		{
			Directory.SetCurrentDirectory(p);
		}
		[BryScript]
		public object getFiles(string p)
		{
			List<object> ret = new List<object>();
			if (p == "") p = ".\\";
			var di = new DirectoryInfo(p);
			var files = di.EnumerateFiles("*", System.IO.SearchOption.TopDirectoryOnly);
			foreach (var file in files)
			{
				ret.Add((object)file.Name);
			}
			return ScriptEngine.Current.Script.Array.from(ret.ToArray());
		}
		[BryScript]
		public object getDirectories(string p)
		{
			List<object> ret = new List<object>();

			var di = new DirectoryInfo(p);
			var files = di.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly);
			foreach (var file in files)
			{
				ret.Add((object)file.Name);
			}
			return ScriptEngine.Current.Script.Array.from(ret.ToArray());
		}
		[BryScript]
		public bool exists(string path)
		{
			return Directory.Exists(path);
		}
		// ***************************************************
		[BryScript]
		public string folderSelectDialog(string s)
		{
			string ret = null;
			using (OpenFileDialog ofd = new OpenFileDialog() 
			{ FileName = "SelectFolder", Filter = "Folder|.", CheckFileExists = false })
			{
				ofd.Title = "Select Folder";
				if(s!="")
				{
					if (Directory.Exists(s))
					{
						ofd.InitialDirectory = s;
					}
				}
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					ret = Path.GetDirectoryName(ofd.FileName);
				}
			}
			return ret;
		}
		public string folderSelectDialog()
		{
			return folderSelectDialog("");
		}
	}

}
