using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.View_Models.Session_View_Model
{
    public class SessionViewModel
    {
        public int Id {  get; set; }
        public string CategoryName { get; set; } = null!;//in Anthor table => Make Query to Get this
        public string Description { get; set; } = null!;
        public string TrainerName {  get; set; } = null!;//in Anthor table => Make Query to Get this 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity {  get; set; }//in the same Table 
        public int AvailableSlots {  get; set; }
        //===========================================================================
        #region Calculated Proprty== Computed based on anthor property
        public string TimeDisplay => $"{StartDate:hh:mm tt}-{EndDate:hh:mm tt}";
            
        public string DateDisplay=> $"{StartDate:MMM dd, yyyy}";
        public TimeSpan Duration =>EndDate-StartDate;//Take Time only Not Date كمان
        public string Status { get 
            {
                if (StartDate>DateTime.Now)
                {
                    return "UpComing";
                }
                else if (StartDate < DateTime.Now&&EndDate>=DateTime.Now)
                {
                    return "OnGoing";
                }
                else
                {
                    //EndDate<=DateTime.Now
                    return "Completed";
                }
            }
        }
        #endregion
    }
}
