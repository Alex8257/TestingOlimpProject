using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    internal class StructuralFramingUpdater : IUpdater
    {
        AddInId addinID = null;
        UpdaterId _updaterID = null;

        public StructuralFramingUpdater(AddInId id)
        {
            addinID = id;
            _updaterID = new UpdaterId(addinID, new Guid("91863372-5C1B-473F-893E-0F070828E86A"));
        }

        public void Execute(UpdaterData data)
        {
            var config = Config.Read();
            if(config.IsActiveAutoInput)
            {
                AutoImportParameter.UpdateParameter(data);
            }
        }

        public string GetAdditionalInformation()
        {
            return "Автоматическое обновление длины каркаса несущего";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Structure;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterID;
        }

        public string GetUpdaterName()
        {
            return "Автоматическое обновление длины каркаса несущего";
        }
    }
}
