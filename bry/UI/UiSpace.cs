using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bry
{
	public class UiSpace : UiControl
	{
		private int m_Space = 8;
		public int Space
		{
			get { return m_Space; }
			set 
			{
				if(m_Space!=value)
				{
					m_Space = value;
					OnSizePolicyChanged(new EventArgs());
				}
			}
		}
		public UiSpace() 
		{
		}
	}
}
