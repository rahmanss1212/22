using System;
using System.IO;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Events.Service.Files
{
    public interface IFileHandler
    {
        String UploadFile(Attachment file);
        Task<FileStreamResult> DownloadFile(long file);
        Task<bool> DeleteAttachment(long id);
    }

    public class FileHandler : IFileHandler
    {

        private IWebHostEnvironment hostingEnvironment;
        private DbServiceImpl<Attachment, Attachment> AttachmentService;
        private string _uploadsRootFolder;

        public FileHandler(IServiceFactory serviceFactory, IWebHostEnvironment hostingEnvironment)
        {
            AttachmentService = serviceFactory.ServicOf<Attachment, Attachment>();
            _uploadsRootFolder = Path.Combine(hostingEnvironment.ContentRootPath, "uploads");
            this.hostingEnvironment = hostingEnvironment;
        }


        public String UploadFile(Attachment file)
        {
            try
            {

                Random random = new Random();
                int length = 5;
                var rString = "";
                for (var i = 0; i < length; i++)
                {
                    rString += ((char) (random.Next(1, 26) + 64)).ToString().ToLower();
                }

                rString += DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" +
                           DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + ""
                           + DateTime.Now.Millisecond;

                if (!Directory.Exists(_uploadsRootFolder))
                {
                    Directory.CreateDirectory(_uploadsRootFolder);
                }

                var filePath = Path.Combine(_uploadsRootFolder, rString + "." + file.Extension);
                File.WriteAllBytes(filePath, file.Content);
                return rString;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<FileStreamResult> DownloadFile(long file)
        {
            Attachment attachment = await AttachmentService.Find(file);

            try
            {

                var filePath = Path.Combine(_uploadsRootFolder, attachment.Url + "." + attachment.Extension);

                var stream = File.OpenRead(filePath);
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/octet-stream");
                return fileStreamResult;

            }
            catch (Exception exception)
            {

                throw new FileNotFoundException();
            }
        }

        public async Task<bool> DeleteAttachment(long id)
        {
            var attachment = await AttachmentService.Find(id);
            var path = Path.Combine(_uploadsRootFolder, attachment.Url + "." + attachment.Extension);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    await AttachmentService.Delete(attachment);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

            return false;

        }


    }
}