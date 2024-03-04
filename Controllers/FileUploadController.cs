using System;    
using System.IO;    
using System.Web;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;

namespace GooBitAPI.Controllers;  

    public class FileUploadController : Controller    
    {
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> index(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("imageFile", "Please select an image file to upload.");
                return View();
            }

           
            var folderName = Path.Combine("wwwroot","uploadFiles");
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach(var file in files)
            {
                Guid newuuid = Guid.NewGuid();
                string newfilename = newuuid.ToString();
                string ext = System.IO.Path.GetExtension(file.FileName);
                newfilename = newuuid.ToString() + ext;
                string fileSavePath = Path.Combine(uploadsFolder, newfilename);
                
                using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                {
                     await file.CopyToAsync(stream);
                }
                
            ViewBag.Message += string.Format("<b>{0}<b> uploaded successfully. <br/>", newfilename);
            }
            return View();
        }    
    }    
        

