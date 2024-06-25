using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.VisualBasic.FileIO;
namespace bry
{
	public class ScriptFolder
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
		private static string GetSafeTempName(string outputFilePath)
		{
			outputFilePath += "_";
			while (File.Exists(outputFilePath))
			{
				outputFilePath += "_";
			}
			return outputFilePath;
		}
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
		public string current()
		{
			return Directory.GetCurrentDirectory();

		}
		public ItemsInfo getFiles(string p)
		{
			List<string> ret = new List<string>();
			if (p == "") p = ".\\";
			var di = new DirectoryInfo(p);
			var files =di.EnumerateFiles("*", System.IO.SearchOption.TopDirectoryOnly);
			foreach ( var file in files)
			{
				ret.Add(file.Name);
			}
			ItemsInfo result = new ItemsInfo(ret.ToArray());
			return result;
		}
		public ItemsInfo getDirectories(string p)
		{
			List<string> ret = new List<string>();

			var di = new DirectoryInfo(p);
			var files = di.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly);
			foreach (var file in files)
			{
				ret.Add(file.Name);
			}
			ItemsInfo result = new ItemsInfo(ret.ToArray());
			return result;
		}
		public bool exists(string path)
		{
			return Directory.Exists(path);
		}
	}
	public class ItemsInfo
	{
		public string [] names;
		public int count;
		public ItemsInfo(string[] files)
		{
			this.names = files;
			this.count = files.Length;
		}
		public string[] keys = new string[] { "names", "count" };
		public int keyCount = 2;
		public int Test(int a,int b) { return a + b; }

	}

}
