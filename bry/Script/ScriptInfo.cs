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
using System.Reflection;

namespace bry
{
	static public class ScriptInfo
	{
		static public List<SInfo> GetsList(Type ct, string cat = "",bool IsGlobal=false)
		{
			List<SInfo> ret = new List<SInfo>();
			MemberInfo[] m = ct.GetMembers();

			foreach (MemberInfo mi in m)
			{

				if (mi.Name.IndexOf(".") == 0) continue;
				if (mi.Name.IndexOf("get_") == 0) continue;
				if (mi.Name.IndexOf("set_") == 0) continue;
				if ((mi.Name == "Equals") || (mi.Name == "GetHashCode")
					|| (mi.Name == "GetType")
					|| (mi.Name == "ToString")) continue;
				BryScriptAttribute MyAttribute =
			   (BryScriptAttribute)Attribute.GetCustomAttribute(mi, typeof(BryScriptAttribute));
				if (MyAttribute == null) continue;

				SInfo si = new SInfo(mi, cat);
				si.IsGlobal = IsGlobal;
				if ((si.Name != "") && (si.Kind != SInfoKind.None))
				{
					ret.Add(si);
				}
			}
			ret.Sort((a, b) => string.Compare(a.Name, b.Name));
			return ret;
		}
		static public SInfo[] Gets(Type ct, string cat = "", bool IsGlobal = false)
		{
			return GetsList(ct, cat,IsGlobal).ToArray();
		}
		static public List<SInfo> GetsEnumList(Type ct)
		{
			string[] sa = Enum.GetNames(ct);
			List<SInfo> ret = new List<SInfo>();
			for (var i = 0; i < sa.Length; i++)
			{
				SInfo si = new SInfo(sa[i], SInfoKind.Enum, ct.Name);
				si.IsGlobal = true;
				ret.Add (si);
			}
			return ret;
		}
		static public SInfo[] GetsEnum(Type ct)
		{
			return GetsEnumList(ct).ToArray();
		}
	}

	public class SInfo
	{
		public string Category = "";
		public string Name = "";
		public SInfoKind Kind = SInfoKind.None;
		public bool IsAtr = false;
		public bool IsGlobal = false;
		public override string ToString()
		{
			string ret = "";

			string[] r = Enum.GetNames(typeof(SInfoKind));
			string ca = "";
			if((Category!="")&&(Category!="____")) ca = Category+".";
			ret = $"{r[(int)Kind]} {ca}{Name}";
			return ret;
		}
		public string Code
		{
			get
			{
				string s = Category;
				if (s != "") s = s + ".";
				s += Name;
				if (Kind==SInfoKind.Method)
				{
					s += "()";
				}
				return s;
			}
		}
		public SInfo(string name, SInfoKind kind, string cat = "")
		{
			Name = name;
			Kind = kind;
			Category = cat;
		}
		public SInfo(MemberInfo mi, string cat = "")
		{
			BryScriptAttribute MyAttribute =
		   (BryScriptAttribute)Attribute.GetCustomAttribute(mi, typeof(BryScriptAttribute));
			if (MyAttribute != null) 
			{
				IsAtr = true;
			}
			Name = mi.Name;
			Category = cat;
			switch (mi.MemberType)
			{
				case MemberTypes.Event:
					Kind = SInfoKind.Event;
					break;
				case MemberTypes.Property:
					Kind = SInfoKind.Property;
					break;
				case MemberTypes.Method:
					Kind = SInfoKind.Method;
					break;
				case MemberTypes.Field:
					Kind = SInfoKind.Field;
					break;
				default:
					Kind = SInfoKind.None;
					break;
			}
		}
	}
	public enum SInfoKind
	{
		None = 0,
		Property, 
		Method, 
		Event,
		Field,
		Enum
	}
	[AttributeUsage(AttributeTargets.All)]
	public class BryScriptAttribute : Attribute
	{
		private string name;

		public BryScriptAttribute()
		{
		}
		public BryScriptAttribute(string name)
		{
			this.name = name;
		}

		public virtual string Name
		{
			get { return name; }
		}
	}
}
