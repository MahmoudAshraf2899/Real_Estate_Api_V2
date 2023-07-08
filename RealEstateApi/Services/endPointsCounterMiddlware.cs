namespace RealEstateApi.Services
{
    /// <summary>
    /// This Class Act like middleware to count how much each end point has been called by Client
    /// </summary>
    public class endPointsCounterMiddlware : IMiddleware
    {
         
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //Todo : Log The Request (Serilog)
            
            //Call The Next Middleware in pipeline
            await next(context);
            
        }
    }
}
