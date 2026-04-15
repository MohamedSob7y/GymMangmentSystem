using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Attachment_Service
{
    public interface IAttachmentService
    {
        string? Upload(string Foldername,IFormFile file);//Take Foldername + File نفسه بس مش عارف نوعه اية 
        //Use Datatype Accept any File From Type Form

        bool Delete(string Filename,string Foldername);
        //IForm مستخدمة فى Upload لانه لسة انا مش عارف نوعه اية قبل التخزين انما لما يتخزن خلاص فى Delete بيكون نوعه string 
    }
}
