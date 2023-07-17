using DevExpress.Web.ASPxRichEdit;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace I_Care {
    public class RibbonCustomizationDemoOptions {
        public RibbonCustomizationDemoOptions() {
            RibbonMode = RichEditRibbonMode.Auto;
            ShowStatusBar = true;

        }
        const string RibbonCustomizationDemoOptionsKey = "RibbonCustomizationDemoOptions";
        public static RibbonCustomizationDemoOptions Current {
            get {
                if(HttpContext.Current.Session[RibbonCustomizationDemoOptionsKey] == null)
                    HttpContext.Current.Session[RibbonCustomizationDemoOptionsKey] = new RibbonCustomizationDemoOptions();
                return (RibbonCustomizationDemoOptions)HttpContext.Current.Session[RibbonCustomizationDemoOptionsKey];
            }
            set { HttpContext.Current.Session[RibbonCustomizationDemoOptionsKey] = value; }
        }
        [Display(Name = "RibbonMode")]
        public RichEditRibbonMode RibbonMode { get; set; }
        [Display(Name = "ShowStatusBar")]
        public bool ShowStatusBar { get; set; }
    }
}
