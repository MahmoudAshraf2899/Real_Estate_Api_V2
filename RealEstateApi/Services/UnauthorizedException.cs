namespace RealEstateApi.Services
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
            StatusCode = 401;
        }

        public int StatusCode { get; }
    }
}
