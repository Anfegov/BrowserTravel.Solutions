using MediatR;
using Microsoft.AspNetCore.Identity;
using BrowserTravel.Solutions.Domain.DomainErrors;
using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public class RoleAssignsToUserCommandHandler : IRequestHandler<RoleAssignsToUserCommand, IdentityResult>
{
    private readonly UserManager<IdentityUser> _userManager;

    public RoleAssignsToUserCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(RoleAssignsToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            throw new CoreException(Errors.GenericError.NotFound("Rol"));
        }

        var result = await _userManager.AddToRolesAsync(user, request.Roles);

        return result;
    }
}
