using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentsystemBLL.Attachment_Service
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtention = {".jpg","jpeg",".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024;//5MegaByte
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Inject Object from WebHostEnvioerment
        public AttachmentService(IWebHostEnvironment webHostEnvironment )
        {
           _webHostEnvironment = webHostEnvironment;
        }
        public bool Delete(string Filename, string Foldername)
        {
            //Stpes For Delete 
            //1: Get FilePath
            //2: Check That FilePath is ExsistOr Not
        }

        public string? Upload(string Foldername, IFormFile file)
        {
            //Steps For Upload 
            //1:Check For Extentiion Of File
            //2:Check For Size Of File
            if (Foldername is null || file is null || file.Length == 0) return null;
            if (file.Length > MaxFileSize) return null;//مش عايزه يعدى MaxLength ليه Size محدد
            var extention = Path.GetExtension(file.FileName).ToLower();//To Get Extentiion From Name of File لان اسم الفايل بيكون فيه Extention باتالى انا عايز بس Extention بتاعه فقط
            if(!AllowedExtention.Contains(extention)) return null;


            //3:Get Folder Path=>المفروض اى صورة تترفع فى wwwroot/images
            var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images",Foldername);
            if (!Directory.Exists(FolderPath))
            {
                //لو مش موجود الFolder اعمله 
              Directory.CreateDirectory(FolderPath);
            }
            //Web host Envoiernment دى تقدر توصل للCurrent Excuting Assembly PLL Layer اللى تقدر تعملها Run 
            //WebhostEnvoiermnet has MoreConfigurations than Directory GetCurrent والاتنين يقدورا يوصلوا  للCurrent Excuting Assembly
            //دى طريقة تانيه للوصول الى WWWRoot/Images عايز اوصل للFolder دا  
            //this Old Way  Path.Combine(Directory.GetCurrentDirectory(),"wwwroot//images",Foldername);

            //4:Get FilePath
            //5: Create Unique Name using Guid
            var filename = Guid.NewGuid()+file.FileName;
            //6:create Stream To Upload Photo Chunks 
            //7:return Filename To Store in DB

        }
    }
}
