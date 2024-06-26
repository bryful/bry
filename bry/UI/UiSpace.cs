using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Windows;


namespace bry
{
	public class UiSpace : UiControl
	{
		[ScriptUsage(ScriptAccess.None)]
		public UiSpace() 
		{
			SizePolicyHor = SizePolicy.Expanding;
			SizePolicyVer = SizePolicy.Expanding;
		}
	}
}
