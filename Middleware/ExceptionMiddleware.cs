namespace GoodHamburguer.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try { await next(context); }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex.Message);
                context.Response.StatusCode = 422;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro inesperado");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = "Erro interno no servidor." });
            }
        }
    }
}
