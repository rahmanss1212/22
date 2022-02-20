using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Events.Api.Authorization;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Core.Models.APTs;
using Events.Core.Models.General;
using Events.Data;
using Events.Service.Files;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagment.services;
using APT = Events.Api.Models.APTs.APT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AptsController : ControllerBase
    {

        private readonly IServiceFactory _serviceFactory;
        private DbServiceImpl<APT, AptView> _aptService;
        private DbServiceImpl<Status, Status> _statusService;
        private IFileHandler fileHandler;
        private readonly IUserService _usersService;

        public AptsController(IServiceFactory service,IUserService usersService, IFileHandler fileHandlerl)
        {
            _serviceFactory = service;
            _aptService = _serviceFactory.ServicOf<APT, AptView>();
            _statusService = _serviceFactory.ServicOf<Status, Status>();
            _usersService = usersService;
            this.fileHandler = fileHandlerl;
        }
        // GET: api/<AptsController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() {
            try
            {
                
                List<AptView> lists= await _aptService.GetItems(x => x.StatusId == APT_STATUS.OPEN);

                return Ok(SuccessResponse<AptView>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
           

    }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] APT apt)
        {

            try
            { 
                
                foreach (var att in apt.Attachments)
                {

                    string v = fileHandler.UploadFile(att.Attachment);
                    att.Attachment.Is64base = false;
                    att.Attachment.Url = v;
                    att.Attachment.Content = null;
                }
                
                
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await _usersService.GetByUsername(username);
                var status =APT_STATUS.OPEN;

                apt.Targeted = apt.Targeted.Select(tc => new TargetedCountry() { CountryId =  tc.Country.Id }).ToList(); ;
                apt.Origin = apt.Origin.Select(oc => new OriginCountry() { CountryId = oc.Country.Id }).ToList();
                apt.TargetSectorNames = apt.TargetSectorNames.Select(oc => new TargetedSector() { SectorId = oc.Sector.Id }).ToList();
                apt.CreatedById = user.Id;
                apt.StatusId = status;
                apt.Contents.ForEach(x => x.CreatedById = user.Id);
                var addedApt = await _aptService.AddItem(apt);


                return Ok(SuccessResponse<APT>.build(addedApt.Entity, addedApt.Entity.Id));
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        // PUT api/<AptsController>/5
        [HttpPut]
        [ClaimsAuth(Type: TYPES.APT, Value: VALUES.UPDATE)]
        public async Task<IActionResult> Put([FromBody] APT newAPT)
        {

            try{
                // define un-known type thats pointing to apt
                APT aPT = await _aptService.UpdateEntity(newAPT);
                return Ok(SuccessResponse<APT>.build(aPT, aPT.Id));
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            } 

        }

        [HttpPost("AddContents")]
        [Authorize]
        public  async Task<IActionResult> AddContent([FromBody] APT value)
        {

            try{
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                APT apt =await _aptService.Find(value.Id);
                Content content = value.Contents[0];
                content.createdDate = DateTime.Now;
                content.CreatedBy =await _usersService.GetByUsername(username);

                if (apt.Contents == null)
                    apt.Contents = new List<Content>();
                apt.Contents.Add(content);
                await _aptService.UpdateEntity(apt);
                return Ok(SuccessResponse<APT>.build(apt, apt.Id));
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            } 
        }

        [HttpGet("GetContents")]
        [Authorize]
        public async Task<IActionResult> GetContents(long id)
        {
            try
            {
                APT apt1 =await _aptService.Find(id);
                return Ok(SuccessResponse<APT>.build(apt1,id));
            }
            catch(Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        
        [HttpGet("GetAptById")]
        [Authorize]
        public async Task<IActionResult> GetAptById(long id)
        {
            try
            {
                APT apt1 =await _aptService.Find(id);
                return Ok(SuccessResponse<APT>.build(apt1,id));
            }
            catch(Exception e)
            {

                return Ok(FailedResponse.Build(e.Message));
            }
        }


    }
}
