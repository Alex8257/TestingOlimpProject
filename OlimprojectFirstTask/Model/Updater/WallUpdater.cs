using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlimprojectFirstTask.Model
{
    internal class WallUpdater : IUpdater
    {
        AddInId addinID = null;
        UpdaterId _updaterID = null;

        public WallUpdater(AddInId id)
        {
            addinID = id;
            _updaterID = new UpdaterId(addinID, new Guid("DAAE5955-68B6-4FC9-B2E9-622A54C30B60"));
        }
        public void Execute(UpdaterData data)
        {
            var config = Config.Read();
            if (config.IsActiveAutoInput)
            {
                AutoImportParameter.UpdateParameter(data);
            }
        }

        public string GetAdditionalInformation()
        {
            return "Автоматическое обновление длины стены";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FloorsRoofsStructuralWalls;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterID;
        }

        public string GetUpdaterName()
        {
            return "Автоматическое обновление длины стены";
        }
    }
}
