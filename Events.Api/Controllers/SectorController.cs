using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Core.Models.General;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private DbServiceImpl<Organization, Organization> OrganizationService;
        public SectorController(AppDbContext ctx,IServiceFactory factory)
        {
            _ctx = ctx;
            OrganizationService = factory.ServicOf<Organization, Organization>();
            
        }


        [HttpGet]
        [Authorize]
        public IList<Sector> Get()
        {
            return _ctx.Sectors.ToList();
        }
        [HttpGet("getContactList/{id}")]
        [Authorize]
        public async  Task<IActionResult> Get(long id)
        {
            try
            {
                var org =await _ctx.OrganizationContacts.Where(x => x.OrganizationId == id).ToListAsync();
                return Ok(SuccessResponse<OrganizationContact>.build(null,0,org));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
             
        }
        
        

        [HttpPost("AddSectors")]
        public IActionResult AddSectors([FromBody] List<Sector> newSector)
        {
            try
            {
                newSector.ForEach(st =>
                {
                    if (_ctx.Sectors.Find(st.Id) == null)
                    {
                        _ctx.Sectors.Add(st);
                    }
                });
                _ctx.SaveChanges();
                return Ok(SuccessResponse<Sector>.build(null, 0, _ctx.Sectors.Include(x => x.Organizations).ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddOrganization")]
        [Authorize]
        public IActionResult Post([FromBody] List<Organization> newOrg)
        {
            try
            {
                newOrg.ForEach(org =>
                {
                    if (_ctx.Organizations.Where(sec => sec.Orgname == org.Orgname
                       && sec.Sector.Id == org.Sector.Id).SingleOrDefault() == null)
                    {
                        org.Sector = _ctx.Sectors.Find(org.Sector.Id);
                        _ctx.Organizations.Add(org);
                }
                });
                _ctx.SaveChanges();
                long id = _ctx.Organizations.Max(s => s.Id);
                return Ok(SuccessResponse<Organization>.build(null, id, _ctx.Organizations.Include(x => x.Users).ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("GetOrganizationContacts")]
        [Authorize]
        public IList<OrganizationContact> GetOrganizationContacts()
        {
            return _ctx.OrganizationContacts.Include(x => x.Organization).ToList();
        }
        [HttpGet("GetOrganizationDomain")]
        [Authorize]
        public IList<OrganizationDomain> GetOrganizationDomain()
        {
            return _ctx.OrganizationDomains.ToList();
        }

        [HttpGet("GetOrganizationIp")]
        [Authorize]
        public IList<OrganizationIps> GetOrganizationIp()
        {
            return _ctx.OrganizationIp.ToList();
        }


        //[HttpGet("{id}")]
        //[Authorize]
        //public async Task<ActionResult<OrganizationContact>> GetContact(long id)
        //{
        //    var contact = await _ctx.OrganizationContacts.FindAsync(id);

        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    return contact;
        //}

        [HttpPost("AddOrgContact")]
        [Authorize]
        public IActionResult POST([FromBody] OrganizationContact contact)
        {
            try
            {
                _ctx.OrganizationContacts.Add(contact);
                _ctx.SaveChanges();
                return Ok(SuccessResponse<OrganizationContact>.build(null, 0, _ctx.OrganizationContacts.Include(x => x.Organization).ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddOrgIp")]
        [Authorize]
        public IActionResult AddOrgIp([FromBody] OrganizationIps ip)
        {
            try
            {
                _ctx.OrganizationIp.Add(ip);
                _ctx.SaveChanges();
                return Ok(SuccessResponse<OrganizationIps>.build(null, 0, _ctx.OrganizationIp.ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddOrgDomain")]
        [Authorize]
        public IActionResult AddOrgDomain([FromBody] OrganizationDomain domain)
        {
            try
            {
                _ctx.OrganizationDomains.Add(domain);
                _ctx.SaveChanges();
                return Ok(SuccessResponse<OrganizationDomain>.build(null, 0, _ctx.OrganizationDomains.ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

    }
}
