using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SharedPool.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problemDetails = new ValidationProblemDetails(
                    validationException.Errors.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                                              .ToDictionary(g => g.Key, g => g.ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Doğrulama Hatası",
                    Detail = "Lütfen gönderdiğiniz verileri kontrol edin."
                };

                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true; // Hatayı yönettik, akışı kes.
            }

            // Eğer başka bir hata türüyse 500 Internal Server Error dönebiliriz.
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Sunucu Hatası",
                Detail = "Beklenmeyen bir hata oluştu."
            }, cancellationToken);

            return true;
        }
    }
}
