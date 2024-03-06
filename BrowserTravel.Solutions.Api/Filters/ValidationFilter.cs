using FluentValidation;
using System.Reflection;
using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Api.Filters;

// Atributo de validación personalizado para marcar los parámetros que deben ser validados
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidateAttribute : Attribute
{
}

// Clase que proporciona un filtro de validación para endpoints
public static class ValidationFilter
{
    // Método que crea un delegado de filtro de validación para un endpoint
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        // Obtener descriptores de validación para el método utilizando los atributos [Validate]
        IEnumerable<ValidationDescriptor> validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        // Si hay descriptores de validación, devolver un delegado de validación; de lo contrario, pasar al siguiente filtro
        if (validationDescriptors.Any())
        {
            return invocationContext => ValidateAsync(validationDescriptors, invocationContext, next);
        }

        return invocationContext => next(invocationContext);
    }

    // Método asincrónico que realiza la validación de los argumentos del endpoint
    private static async ValueTask<object?> ValidateAsync(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (ValidationDescriptor descriptor in validationDescriptors)
        {
            // Obtener el argumento del contexto de invocación
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is not null)
            {
                // Validar el argumento utilizando el validador correspondiente
                var validationResult = await descriptor.Validator.ValidateAsync(
                    new ValidationContext<object>(argument)
                );

                // Si la validación no es exitosa, lanzar una excepción con el mensaje de error
                if (!validationResult.IsValid)
                {
                    throw new CoreException(validationResult.ToString());
                    // También puedes manejar el resultado de validación de otras maneras, como devolver un problema de validación HTTP
                    // return Results.ValidationProblem(validationResult.ToDictionary(),
                    //    statusCode: (int)HttpStatusCode.UnprocessableEntity);
                }
            }
        }

        // Pasar al siguiente filtro en la cadena de filtros de endpoint
        return await next.Invoke(invocationContext);
    }

    // Método que obtiene los descriptores de validación para los parámetros marcados con [Validate]
    static IEnumerable<ValidationDescriptor> GetValidators(MethodBase methodInfo, IServiceProvider serviceProvider)
    {
        ParameterInfo[] parameters = methodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
            {
                // Construir el tipo del validador utilizando el tipo de parámetro
                Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                // Obtener el servicio del validador del proveedor de servicios
                IValidator? validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };
                }
            }
        }
    }

    // Clase interna que representa un descriptor de validación
    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }
}
