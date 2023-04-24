using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    internal class AutoImportParameter
    {
        internal static void UpdateParameter(UpdaterData data)
        {
            try
            {
                var doc = data.GetDocument();

                foreach (var id in data.GetModifiedElementIds())
                {
                    var element = doc.GetElement(id);
                    SetLenthToParameter(doc, element);
                }

                foreach (var id in data.GetAddedElementIds())
                {
                    var element = doc.GetElement(id);
                    SetLenthToParameter(doc, element);
                }
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }


        private static void SetLenthToParameter(Document doc, Element element)
        {
            var parameter = element.LookupParameter(Constants.NAME_PARAMETER_FOR_INPUT);
            if (parameter == null) return;

            var parameterLength = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            if (parameterLength == null) return;

            var value = parameterLength.AsValueString();

            parameter.Set(value);
        }
    }
}
