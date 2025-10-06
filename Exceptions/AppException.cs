namespace FoodOrderApi.Exceptions
{
    public class AppException : Exception
    {
        public int Code { get; set; }
        public string ErrorMessage { get; set; } = default!;

        public AppException(int code, string message)
        {
            Code = code;
            ErrorMessage = message;
        }
    }
}
