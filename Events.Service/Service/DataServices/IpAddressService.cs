using Events.Api.Models.Incidents;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class IpAddressService //: DbServiceImpl<IpAddress,IpAddress>
    {
        //public IpAddressService(AppDbContext ctx) : base(ctx) { }
        //public override IOrderedQueryable<IpAddress> GetQuery()
        //{
        //    return context.IpAddress
        //        .Include(x => x.OwnerDetail)
        //        .Include(x => x.Dest)
        //        .Include(x => x.Source)
        //        .OrderBy(x => x.Id);
        //}

        //public override IOrderedQueryable<IpAddress> GetViewQuery()
        //{
        //    return context.IpAddress
        //        .Include(x => x.OwnerDetail)
        //        .Include(x => x.Dest)
        //        .Include(x => x.Source)
        //        .OrderBy(x => x.Id);
        //}
    }
}
