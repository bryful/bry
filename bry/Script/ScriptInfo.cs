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
		static public SInfo[] Gets(Type ct, string cat = "")
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

				SInfo si = new SInfo(mi,cat);
				if ((si.Name != "")&&(si.Kind != SInfoKind.None))
				{
					ret.Add(si);
				}
			}
			return ret.ToArray();
		}
	}

	public class SInfo
	{
		public string Category = "";
		public string Name = "";
		public SInfoKind Kind = SInfoKind.None;
		public bool isAtr = false;
		public new string ToString()
		{
			string ret = "";

			string[] r = Enum.GetNames(typeof(SInfoKind));
			string ca = "";
			if(Category!="") ca = Category+".";
			ret = $"{r[(int)Kind]} {ca}{Name}";
			return ret;
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
				isAtr = true;
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
		Field
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
