using Domain.Exceptions;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Falha de validação na requisição {Path}", context.Request.Path);
                await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Erro de validação", new
                {
                    errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                });
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Regra de domínio violada na requisição {Path}", context.Request.Path);
                await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado na requisição {Path}", context.Request.Path);
                await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.");
            }
        }

        private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string title, object? extra = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var body = new Dictionary<string, object?>
            {
                ["status"] = (int)statusCode,
                ["title"] = title
            };

            if (extra != null)
            {
                foreach (var prop in extra.GetType().GetProperties())
                    body[prop.Name] = prop.GetValue(extra);
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
