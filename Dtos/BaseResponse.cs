namespace CustomerAPI.Dtos
{
    public abstract class BaseResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}
