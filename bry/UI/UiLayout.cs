using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bry
{
	public class UiLayout : UiControl
	{
		private LayoutOrient m_LayoutKind = LayoutOrient.Horizontal;
		public LayoutOrient LayoutKind
		{ 
			get { return m_LayoutKind; }
			set 
			{
				if (m_LayoutKind != value)
				{
					m_LayoutKind = value;
					OnSizePolicyChanged(new EventArgs());
				}
			}
		}
		public UiLayout() 
		{
			this.Dock = System.Windows.Forms.DockStyle.Fill;
		}
	}
}
