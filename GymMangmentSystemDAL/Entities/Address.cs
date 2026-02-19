using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentSystemDAL.Entities
{
    [Owned]
    public class Address
    {
        public int BuildingNumber {  get; set; }
        public string Street {  get; set; }
        public string City {  get; set; }
    }
}
