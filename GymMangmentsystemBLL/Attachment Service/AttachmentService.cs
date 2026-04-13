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
        private readonly string[] AllowedExtention = { ".jpg", "jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024;//5MegaByte
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Inject Object from WebHostEnvioerment
        public AttachmentService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public bool Delete(string Filename, string Foldername)
        {
            try
            {
                //Stpes For Delete 
                //1: Get FilePath
                if (string.IsNullOrEmpty(Filename) || string.IsNullOrEmpty(Foldername))
                    return false;
                var fullpath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Foldername, Filename);
                //2: Check That FilePath is ExsistOr Not
                if (File.Exists(fullpath))
                {
                    File.Delete(fullpath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File  because {ex}");
                return false;
            }
        }

        public string? Upload(string Foldername, IFormFile file)
        {
            try
            {
                //Steps For Upload 
                //1:Check For Extentiion Of File
                //2:Check For Size Of File
                if (Foldername is null || file is null || file.Length == 0) return null;
                if (file.Length > MaxFileSize) return null;//مش عايزه يعدى MaxLength ليه Size محدد
                var extention = Path.GetExtension(file.FileName).ToLower();//To Get Extentiion From Name of File لان اسم الفايل بيكون فيه Extention باتالى انا عايز بس Extention بتاعه فقط
                if (!AllowedExtention.Contains(extention)) return null;


                //3:Get Folder Path=>المفروض اى صورة تترفع فى wwwroot/images
                var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Foldername);
                if (!Directory.Exists(FolderPath))
                {
                    //لو مش موجود الFolder اعمله 
                    Directory.CreateDirectory(FolderPath);
                }
                //Web host Envoiernment دى تقدر توصل للCurrent Excuting Assembly PLL Layer اللى تقدر تعملها Run 
                //WebhostEnvoiermnet has MoreConfigurations than Directory GetCurrent والاتنين يقدورا يوصلوا  للCurrent Excuting Assembly
                //دى طريقة تانيه للوصول الى WWWRoot/Images عايز اوصل للFolder دا  
                //this Old Way  Path.Combine(Directory.GetCurrentDirectory(),"wwwroot//images",Foldername);


                //4: Create Unique Name using Guid
                var filename = Guid.NewGuid().ToString() + extention;

                //5:Get FilePath
                var filepath = Path.Combine(FolderPath, filename);

                //6:create Stream To Upload Photo Chunks 
                using var filestream = new FileStream(filepath, FileMode.Create);//المفروض عندى حاجة اسمها FileModel يعنى فاتح Stream عشان اية 
                                                                                 //عشان اعمل Upload
                                                                                 //FileMode.CreateNew=> انا بمسح الفايل Path اللى موجود او مش موجود وبعمله من اول وجديد يعنى بيعمله حتى لو موجود او مش موجود 
                                                                                 //FileMode.Create=> لو Path موجود بيعمل override وبس انما لو مش موجود بيعمله من الاول 
                file.CopyTo(filestream);//عشان هنا بيحول File اللى رفعته الى  Chunks عشان يترفع على server
                //FileStream Not Managed By CLR لازم انا اقفله بادى  

                //7:return Filename To Store in DB
                return filename;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File in Folder {Foldername}: because {ex}");
                return null;
            }

        }
    }
}
