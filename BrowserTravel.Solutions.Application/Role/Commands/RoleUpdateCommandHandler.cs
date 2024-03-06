using MediatR;
using Microsoft.AspNetCore.Identity;
using BrowserTravel.Solutions.Domain.DomainErrors;
using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public class RoleUpdateCommandHandler : IRequestHandler<RoleUpdateCommand, Unit>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleUpdateCommandHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);

        if (role != null)
        {
            role.Name = request.NewRoleName;
            await _roleManager.UpdateAsync(role);
            return Unit.Value;
        }

        throw new CoreException(Errors.GenericError.NotFound("Rol"));
    }
}
