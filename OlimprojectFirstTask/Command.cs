using Autodesk.Revit.Attributes;
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
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var config = Config.Read();
            config.IsActiveAutoInput = !config.IsActiveAutoInput;
            config.Save();

            ButtonExtension.SetNameOnBtn(commandData.Application);
            return Result.Succeeded;
        }
    }
}
