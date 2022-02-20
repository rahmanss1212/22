using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Notifications;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Events.Service.Service.DataServices
{
    public class NewService //: DbServiceImpl<New, New>
    {
       // public NewService(AppDbContext ctx) : base(ctx) { }

        //public override IOrderedQueryable<New> GetQuery()
        //=>context.News
        //        .Include(x => x.Urgancey)
        //        .Include(x=> x.Users)
        //        .OrderByDescending(x=> x.Id);
        

        //public override IOrderedQueryable<New> GetViewQuery()
        //=> context.News.OrderBy(x => x.Id);
    }
}
