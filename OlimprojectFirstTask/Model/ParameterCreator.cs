using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    internal static class ParameterCreator
    {
        internal static void CreateParameterIfNotExist(Document doc)
        {
            var app =doc.Application;

            var categorySet = app.Create.NewCategorySet();
            categorySet.Insert(doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls));
            categorySet.Insert(doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming));

            var originalFile = app.SharedParametersFilename;
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var tempFile = Path.Combine(assemblyFolder, Constants.NAME_FILE_SHARED_PARAMETER);

            try
            {
                app.SharedParametersFilename = tempFile;

                var sharedParameterFile = app.OpenSharedParameterFile();

                foreach (var dg in sharedParameterFile.Groups)
                {
                    if (dg.Name == Constants.NAME_GROUP)
                    {
                        var externalDefinition = dg.Definitions.get_Item(Constants.NAME_PARAMETER_FOR_INPUT) as ExternalDefinition;

                        if (externalDefinition == null) continue;
                        using (var t = new Transaction(doc))
                        {
                            t.Start("Add Shared Parameters");
                            var newIB = app.Create.NewInstanceBinding(categorySet);
                            doc.ParameterBindings.Insert(externalDefinition, newIB, BuiltInParameterGroup.PG_TEXT);
                            t.Commit();
                        }
                    }
                }
            }
            catch { }
            finally
            {
                app.SharedParametersFilename = originalFile;
            }
        }
    }
}
