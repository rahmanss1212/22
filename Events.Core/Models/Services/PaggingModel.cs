namespace Events.Service.Service.DataServices
{
    public class PaggingModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int SkippedPages => (PageNumber * PageSize) - PageSize;
    }
}