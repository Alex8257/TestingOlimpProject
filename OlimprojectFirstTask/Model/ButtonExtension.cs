using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    internal static class ButtonExtension
    {
        internal static void SetNameOnBtn(UIApplication application)
        {
            List<RibbonPanel> panels = application.GetRibbonPanels(Constants.NAME_TAB);
            RibbonPanel ribbonPanel = null;
            foreach (RibbonPanel p in panels)
            {
                if (p.Name == Constants.NAME_PANEL)
                {
                    ribbonPanel = p;
                }
            }

            var thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            var ribbonItem = ribbonPanel.GetItems().FirstOrDefault(x => x.Name == Constants.NAME_BTN);

            if(ribbonItem == null)
            {
                return;
            }

            var btn = ribbonItem as PushButton;

            var config = Config.Read();
            if (config.IsActiveAutoInput)
            {
                btn.ItemText = Constants.NAME_BTN_ON;
            }
            else
            {
                btn.ItemText = Constants.NAME_BTN_OFF;
            }
        }
    }
}
