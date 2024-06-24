using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bry
{
	public class ScriptFolder
	{
		public bool Exists(string path)
		{
			return Directory.Exists(path);
		}
	}
}
