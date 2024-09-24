using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public static class Utils
	{
		public static bool IsVaildInput(int min, int max, int input)
		{
			if (min <= input || input < max) { return true; }
			else { return false; }
		}
	}
}
