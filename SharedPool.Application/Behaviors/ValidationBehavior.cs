using FluentValidation;
using MediatR;

namespace SharedPool.Application.Behaviors
{
    // IPipelineBehavior: MediatR'ın middleware'idir.
    // IPipelineBehavior: MediatR'ın middleware'idir.
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Tüm validator'ları asenkron çalıştır (Örn: Hem CreateUserValidator hem de başka bir custom validator varsa)
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    // FluentValidation'ın kendi exception sınıfını fırlatıyoruz. 
                    // API katmanında Global Exception Handler ile bunu yakalayıp 400 Bad Request döneceğiz.
                    throw new ValidationException(failures);
                }
            }

            // Hata yoksa sıradaki adıma (Handler'a) geç
            return await next();
        }
    }
}
