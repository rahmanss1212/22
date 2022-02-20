using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using Events.Core.Models.General;
using Events.Core.Models.StaticIP;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Files;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticIPController : ControllerBase
    {
        private readonly DbServiceImpl<StaticIP, StaticIP> staticipsServiceImpl;

        public StaticIPController(IServiceFactory serviceFactory)
        {
            staticipsServiceImpl = serviceFactory.ServicOf<StaticIP,StaticIP>();
        }
        
        [HttpPost("AddStaticIps")]
        public  async Task<IActionResult> AddStaticIps([FromBody] List<StaticIP> ips)
        {

            try
            {
                return await staticipsServiceImpl.addRange(ips) ?  Ok(SuccessResponse<APT>.build(null)) : 
                Ok(FailedResponse.Build("حدثت مشكلة أثناء الحفظ"));
                ;
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            } 
        }
        
        [HttpPost("search")]
        public  async Task<IActionResult> search([FromBody] SearchModel search)
        {

            try
            {
                ExpressionStarter<StaticIP> predicateBuilder = PredicateBuilder.False<StaticIP>();
                predicateBuilder = predicateBuilder.Or(p => p.Mobile.ToString().Equals(search.key));
                predicateBuilder = predicateBuilder.Or(p => p.Name.Equals(search.key));
                predicateBuilder = predicateBuilder.Or(p => p.Ip.Equals(search.key));

                var items = await staticipsServiceImpl.GetItems(predicateBuilder);
                return Ok(SuccessResponse<StaticIP>.build(null, 0, items));
                
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            } 
        }
        
        

        // GET api/<AttachmentController>/5
        [HttpGet]
        public async Task<IActionResult> GetStaticIPs()
        {
            try
            {
                var items =await staticipsServiceImpl.GetItems(x => x.IsActive);
                return Ok(SuccessResponse<StaticIP>.build(null,0,items));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(FailedResponse.Build(e.InnerException.Message));
            }
            
        }
    }
}
