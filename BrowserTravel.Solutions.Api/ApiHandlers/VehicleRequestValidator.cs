using BrowserTravel.Solutions.Application.Vehicle.Commands;
using FluentValidation;

namespace BrowserTravel.Solutions.Api.ApiHandlers;

public class VehicleRequestValidator : AbstractValidator<VehicleRegisterCommand>
{
    const int _min_length = 8;

    // Constructor de la clase PersonRequestValidator
    public VehicleRequestValidator()
    {
        // Regla de validación para el campo Nid (Número de Identificación)
        //RuleFor(x => x.Nid)
        //    .NotEmpty().WithMessage(x => Domain.DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(nameof(x.Nid)))
        //    .MinimumLength(_min_length).WithMessage(x => Domain.DomainErrors.Errors.GenericError.WithBadCharacterNumber(nameof(x.Nid), _min_length, x.Nid.Length));

        //// Regla de validación para el campo FirstName (Nombre)
        //RuleFor(x => x.FirstName)
        //    .NotEmpty().WithMessage(x => Domain.DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(nameof(x.FirstName)));

        //// Regla de validación para el campo LastName (Apellido)
        //RuleFor(x => x.LastName)
        //    .NotEmpty().WithMessage(x => Domain.DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(nameof(x.LastName)));
    }
}
