using Microsoft.Office.Interop.Excel;
using System;

namespace OpenBots.Commands.Microsoft.Library
{
    public static class ExcelHelper
    {
		public static string GetLastIndexOfNonEmptyCell(this Application app, Range sourceRange, Range startPoint)
		{
			Range rng = sourceRange.Cells.Find(
				What: "*",
				After: startPoint,
				LookIn: XlFindLookIn.xlFormulas,
				LookAt: XlLookAt.xlPart,
				SearchDirection: XlSearchDirection.xlPrevious,
				MatchCase: false);
			if (rng == null)
				return "";
			return rng.Address[false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing];
		}
	}
}
