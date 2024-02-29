using System;    
using System.IO;    
using System.Web;    
using Microsoft.AspNetCore.Mvc;
    
namespace GooBitAPI.Controllers;  

    public class FileUploadController : Controller    
    {
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> index(IFormFile file)
        {
            var folderName = Path.Combine("wwwroot","uploadFiles");
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileSavePath = Path.Combine(uploadsFolder, fileName);

            using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewBag.Message = fileName + " upload successfully";

            return View();
        }    
    }    
        

