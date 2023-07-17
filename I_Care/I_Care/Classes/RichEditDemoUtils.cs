using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;


namespace I_Care.Classes
{
	public class RichEditDemoUtils
	{
		public static void HideFileTab(ASPxRichEdit richedit)
		{
			richedit.CreateDefaultRibbonTabs(true);
			RibbonTab fileTab = richedit.RibbonTabs[0];
			richedit.RibbonTabs.Remove(fileTab);
		}
	}
}