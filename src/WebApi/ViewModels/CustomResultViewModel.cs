namespace WebApi.ViewModels
{
    public class CustomResultViewModel
    {
        public CustomResultViewModel(object? data,ErrorViewModel? error )
        {
                Data = data;
                Error = error;
        }
        public object? Data { get; set; }
        public ErrorViewModel? Error { get; set; }
    }
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public string? InnerMessage { get; set; }
    }
}
