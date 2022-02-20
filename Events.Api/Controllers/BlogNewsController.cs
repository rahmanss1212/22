using Events.Core.Models.General;
using Events.Core.Models.NewsBlog;
using Events.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagment.services;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogNewsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly IUserService _userService;

        public BlogNewsController(AppDbContext ctx, IUserService userService)
        {
            _ctx = ctx;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IList<BlogNews>> GetBlogNews()
        => await _ctx.BlogNews.Include(x => x.CreatedBy).Include(x => x.NewsCategory).ToListAsync();
        

        [HttpPost("AddBlogNews")]
        public async Task<IActionResult> AddBlogNews([FromBody] List<BlogNews> newBlogNews)
        {
            try
            {
                newBlogNews.ForEach(async bn =>
                {
                    if (await _ctx.BlogNews.FindAsync(bn.Id) == null)
                    {
                        await _ctx.BlogNews.AddAsync(bn);
                    }
                });
               await _ctx.SaveChangesAsync();
                return Ok(SuccessResponse<BlogNews>.build(null, 0, await _ctx.BlogNews.Include(x => x.NewsCategory).ToListAsync()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

    }
}
