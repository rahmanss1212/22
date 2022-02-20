using Events.Core.Models.General;
using Events.Core.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Service.Mappers
{
    public class GeneralReportMapper : IMapper<GeneralReport, GeneralReportDataView>
    {
        public GeneralReportDataView Convert(GeneralReport entity)
        {
            var view = new GeneralReportDataView
            {
                Id = entity.Id,
                CreatedByName = entity.CreatedBy.FullName,
                Title = entity.Title,
                Content = entity.Content,
                CategoryTitle = entity.ReportCategory?.Title,
                CreatedDate = entity.CreatedDate,
                
            };
            return view;
        }
    }
}