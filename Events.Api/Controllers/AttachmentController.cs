using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using Events.Core.Models.General;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Files;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly DbServiceImpl<Attachment, Attachment> attachmentService;
        private IFileHandler fileHandler;
        
        public AttachmentController(IServiceFactory serviceFactory, IFileHandler fileHandler)
        {
            attachmentService = serviceFactory.ServicOf<Attachment,Attachment>();
            this.fileHandler = fileHandler;
        }

        // GET api/<AttachmentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachment(long id)
        {
           
                Attachment attachment = await attachmentService.Find(id);
                if (attachment.Is64base)
                    return Ok(SuccessResponse<Attachment>.build(attachment, 0));
                else
                    return await fileHandler.DownloadFile(id);
        }
    }
}
