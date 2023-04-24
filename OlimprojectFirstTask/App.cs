using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OlimprojectFirstTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask
{
    public class App : IExternalApplication
    {
        private UIControlledApplication _app;
        public Result OnStartup(UIControlledApplication application)
        {
            _app = application;
            var result = Result.Succeeded;
            try
            {
                application.ControlledApplication.DocumentOpened+= new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(Application_DocumentOpened);
                application.ControlledApplication.DocumentCreated+= new EventHandler<Autodesk.Revit.DB.Events.DocumentCreatedEventArgs>(Application_DocumentCreated);
                CreateButton(application);

                var wallUpdater = new WallUpdater(application.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(wallUpdater);
                ElementCategoryFilter filterWallUpdater = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                UpdaterRegistry.AddTrigger(wallUpdater.GetUpdaterId(), filterWallUpdater, Element.GetChangeTypeElementAddition());
                UpdaterRegistry.AddTrigger(wallUpdater.GetUpdaterId(), filterWallUpdater, Element.GetChangeTypeGeometry());

                var structuralFramingUpdater = new StructuralFramingUpdater(application.ActiveAddInId);
                UpdaterRegistry.RegisterUpdater(structuralFramingUpdater);
                ElementCategoryFilter filterStructuralFramingUpdater = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
                UpdaterRegistry.AddTrigger(structuralFramingUpdater.GetUpdaterId(), filterStructuralFramingUpdater, Element.GetChangeTypeElementAddition());
                UpdaterRegistry.AddTrigger(structuralFramingUpdater.GetUpdaterId(), filterStructuralFramingUpdater, Element.GetChangeTypeGeometry());

            }
            catch 
            {
                result = Result.Failed;
            }
            return result;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentOpened -= Application_DocumentOpened;
            application.ControlledApplication.DocumentCreated -= Application_DocumentCreated;

            var wallUpdater = new WallUpdater(application.ActiveAddInId);
            UpdaterRegistry.UnregisterUpdater(wallUpdater.GetUpdaterId());

            var structuralFramingUpdater = new StructuralFramingUpdater(application.ActiveAddInId);
            UpdaterRegistry.UnregisterUpdater(structuralFramingUpdater.GetUpdaterId());

            return Result.Succeeded;
        }

        private void Application_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs args)
        {
            ParameterCreator.CreateParameterIfNotExist(args.Document);
        }

        private void Application_DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs args)
        {
            ParameterCreator.CreateParameterIfNotExist(args.Document);
        }

        private void CreateButton(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = null;
            try
            {
                application.CreateRibbonTab(Constants.NAME_TAB);
            }
            catch { }

            try
            {
                RibbonPanel panel = application.CreateRibbonPanel(Constants.NAME_TAB, Constants.NAME_PANEL);
            }
            catch { }

            List<RibbonPanel> panels = application.GetRibbonPanels(Constants.NAME_TAB);
            foreach (RibbonPanel p in panels)
            {
                if (p.Name == Constants.NAME_PANEL)
                {
                    ribbonPanel = p;
                }
            }

            var thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            var btn = ribbonPanel.AddItem(
                new PushButtonData(Constants.NAME_BTN,
                "example",
                thisAssemblyPath,
                "OlimprojectFirstTask.Command")) as PushButton;

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
