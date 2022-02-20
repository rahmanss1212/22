using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Authorization;
using Events.Api.Logging;
using Events.Api.Models.General;
using Events.Core.Models.General;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagment.services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewController : ControllerBase
    {
        private IUserService usersService;
        private IServiceFactory ServiceFactory;
        private readonly AppDbContext _ctx;

        public NewController(IUserService us, AppDbContext ctx, IServiceFactory serviceFactory)
        {
            usersService = us;
            _ctx = ctx;
            ServiceFactory = serviceFactory;
        }

        // GET: api/<NewsController>
        [HttpGet]
        [Authorize]
        public IList<New> Get()
        {
            return _ctx.News.Include(x => x.Urgancey).Include(x => x.Users).ToList();
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.ADD_NEWS, Value: VALUES.ADD)]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        //[ServiceFilter(typeof(LogAction))]
        //[ClaimsAuth(Type: TYPES.ADD_NEWS, Value: VALUES.ADD)]
        public async Task<IActionResult> Post([FromBody] New newNews)
        {

            try
            {
                var username = (string) HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                New news = new New();
                
                news.UsersId = user.Id;
                news.description = newNews.description;
                news.publish = false;
                news.title = newNews.title;
                news.depId = (int) user.Section.Id;
                _ctx.News.Add(news);
                await _ctx.SaveChangesAsync();
                return Ok(newNews);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        // POST api/<NewsController>
        //[HttpPost("Add")]
        //[Authorize]
        //public IActionResult POST([FromBody] New newNews)
        //{
        //    string username = (string)HttpContext.Items[Constants.UserId.ToString()];
        //    var user = usersService.GetByUsername(username);

        //    try
        //    {
        //        newNews.Users = user;
        //        _ctx.News.Add(newNews);
        //        _ctx.SaveChanges();
        //        return Ok(SuccessResponse<New>.build(null, 0, _ctx.News.Include(x => x.Urgancey).ToList()));
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(FailedResponse.Build(e.Message));
        //    }


        //}

        // PUT api/<NewsController>/5

        [HttpPut("updateNews")]
        public async Task<IActionResult> UpdateNews([FromBody] New news)
        {
            var username = (string) HttpContext.Items[Constants.UserId.ToString()];
            try
            {
                var user =await usersService.GetByUsername(username);
                New Cnew = news;

                Cnew.Id = news.Id;
                Cnew.Users = user;
                Cnew.title = news.title;
                Cnew.publish = news.publish;
                Cnew.description = news.description;
                Cnew.depId = news.depId;

                _ctx.News.Update(Cnew);


                await _ctx.SaveChangesAsync();

                return Ok(SuccessResponse<New>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                New news = _ctx.News.Find(id);

                _ctx.News.Remove(news);
                _ctx.SaveChanges();

                return Ok(SuccessResponse<New>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }
}