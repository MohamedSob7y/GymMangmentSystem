using GymMangmentsystemBLL.View_Models.Analytic_Service_View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Services.Interface
{
    public interface IAnalyticsService
    {
        HomeAnalyticsViewModel GetHomeAnalyticsService();
    }
}
