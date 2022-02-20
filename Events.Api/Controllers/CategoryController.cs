using System;
using System.Collections.Generic;
using System.Linq;
using Events.Core.Models.General;
using Events.Core.Models.Reports;
using Events.Data;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public CategoryController(AppDbContext ctx)
        {
            _ctx = ctx;
        }


        [HttpGet]
        public IList<ReportCategory> Get()
        {
            return _ctx.ReportCategories.ToList();
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody] List<ReportCategory> reportCategories)
        {
            try
            {
                reportCategories.ForEach(rc =>
                {
                    if (_ctx.ReportCategories.Find(rc.Id) == null)
                    {
                        _ctx.ReportCategories.Add(rc);
                    }
                });
                _ctx.SaveChanges();
                return Ok(SuccessResponse<ReportCategory>.build(null, 0, _ctx.ReportCategories.ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }
}