using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Analytic_Service_View_Model
{
    public class HomeAnalyticsViewModel
    {
        public int TotalMembers {  get; set; }
        public int ActiveMembers {  get; set; }
        public int TotalTrainers {  get; set; }
        public int UpComingSessions {  get; set; }
        public int OnGoingSessions {  get; set; }
        public int CompletedSessions {  get; set; }
    }
}
