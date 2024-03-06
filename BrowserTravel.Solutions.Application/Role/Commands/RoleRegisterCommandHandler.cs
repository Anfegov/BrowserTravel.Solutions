using MediatR;
using Microsoft.AspNetCore.Identity;
using BrowserTravel.Solutions.Domain.DomainErrors;
using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public class RoleRegisterCommandHandler : IRequestHandler<RoleRegisterCommand, IdentityResult>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRegisterCommandHandler(RoleManager<IdentityRole> roleManager) =>
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

    public async Task<IdentityResult> Handle(RoleRegisterCommand request, CancellationToken cancellationToken)
    {
        var roleExist = await _roleManager.RoleExistsAsync(request.RoleName.ToLower());

        if (!roleExist)
        {
            return await _roleManager.CreateAsync(new IdentityRole(request.RoleName));
        }

        throw new CoreException(Errors.RoleError.RoleAlreadyExists());
    }
}
