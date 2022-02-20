namespace Events.Core.Models.Services
{
    public class DbResponse<T>
    {
       
            public string Message { get; set; }
            public T Entity { get; set; }
            public bool IsSuccess { get; set; }
        
    }
}