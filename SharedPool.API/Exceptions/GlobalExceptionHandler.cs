using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedPool.Domain.Exceptions;

namespace SharedPool.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // 1. GİRDİ DOĞRULAMA HATALARI (FluentValidation)
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

            // 2. İŞ KURALI HATALARI (Bizim eklediğimiz kısım)
            if (exception is BusinessException businessException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "İş Kuralı İhlali",
                    Detail = businessException.Message
                }, cancellationToken);

                return true; // Hatayı yönettik, akışı kes.
            }

            // 3. BEKLENMEYEN DİĞER TÜM HATALAR (Catch-All - 500)
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
