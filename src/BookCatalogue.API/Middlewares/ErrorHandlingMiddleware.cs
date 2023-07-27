using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Xml.Schema;
using BookCatalogue.API.Dtos.Wrappers;
using BookCatalogue.API.Exceptions;

namespace BookCatalogue.API.Middlewares;

internal sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        var errorResponse = new ErrorResponse();
        bool operationSuccessful = false;

        try
        {
            await _next(context);
            operationSuccessful = true;
        }
        catch (BookCatalogueApiException e)
        {
            _logger.LogWarning(e, e.Message);
            response.StatusCode = e.StatusCode;
            errorResponse.Message = e.Message;
            errorResponse.Errors = e.Errors;
        }
        catch (AggregateException e)
        {
            _logger.LogError(e, e.Message);
            response.StatusCode = StatusCodes.Status500InternalServerError;
            errorResponse.Message = "Multiple errors occurred. Check errors for more detail.";

            var errorMessages = e.InnerExceptions
                .Select(e => e.Message)
                .ToList();

            errorResponse.Errors = errorMessages;
        }
        catch (NotImplementedException e)
        {
            _logger.LogError(e, e.Message);
            response.StatusCode = StatusCodes.Status501NotImplemented;
            errorResponse.Message = e.Message;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            response.StatusCode = StatusCodes.Status500InternalServerError;
            errorResponse.Message = "An error occurred. Please try again later.";
        }
        finally
        {
            if (!operationSuccessful)
            {
                errorResponse.Status = response.StatusCode;

                var result = JsonSerializer.Serialize(errorResponse);

                await response.WriteAsync(result);
            }
        }
    }
}