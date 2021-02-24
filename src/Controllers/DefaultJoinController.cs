using Atomus.Database;
using Atomus.Service;
using Atomus.Windows.Controls.Join.Models;
using System.Threading.Tasks;

namespace Atomus.Windows.Controls.Join.Controllers
{
    internal static class DefaultJoinController
    {
        internal static async Task<IResponse> SaveAsync(this ICore core, DefaultJoinSaveModel save)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet { ServiceName = core.GetAttribute("ServiceName") };
            serviceDataSet["JOIN"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["JOIN"].CommandText = core.GetAttribute("ProcedureJoin");
            serviceDataSet["JOIN"].AddParameter("@EMAIL", DbType.NVarChar, 100);
            serviceDataSet["JOIN"].AddParameter("@ACCESS_NUMBER", DbType.NVarChar, 4000);
            serviceDataSet["JOIN"].AddParameter("@NICKNAME", DbType.NVarChar, 50);
            serviceDataSet["JOIN"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["JOIN"].NewRow();
            serviceDataSet["JOIN"].SetValue("@EMAIL", save.EMAIL);
            serviceDataSet["JOIN"].SetValue("@ACCESS_NUMBER", save.ACCESS_NUMBER);
            serviceDataSet["JOIN"].SetValue("@NICKNAME", save.NICKNAME);

            return await core.ServiceRequestAsync(serviceDataSet);
        }

        internal static async Task<IResponse> SavePasswordChangeAsync(this ICore core, DefaultJoinSaveModel save)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet { ServiceName = core.GetAttribute("ServiceName") };
            serviceDataSet["JOIN"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["JOIN"].CommandText = core.GetAttribute("ProcedurePasswordChange");
            serviceDataSet["JOIN"].AddParameter("@EMAIL", DbType.NVarChar, 100);
            serviceDataSet["JOIN"].AddParameter("@ACCESS_NUMBER", DbType.NVarChar, 4000);
            serviceDataSet["JOIN"].AddParameter("@NEW_ACCESS_NUMBER", DbType.NVarChar, 4000);
            serviceDataSet["JOIN"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["JOIN"].NewRow();
            serviceDataSet["JOIN"].SetValue("@EMAIL", save.EMAIL);
            serviceDataSet["JOIN"].SetValue("@ACCESS_NUMBER", save.ACCESS_NUMBER);
            serviceDataSet["JOIN"].SetValue("@NEW_ACCESS_NUMBER", save.NEW_ACCESS_NUMBER);

            return await core.ServiceRequestAsync(serviceDataSet);
        }
    }
}
