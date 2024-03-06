using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BrowserTravel.Solutions.Application.User.Commands;

public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, IdentityResult>
{
    private readonly UserManager<IdentityUser> _userManager;
    public UserRegisterCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = request.UserName.ToLower() };
        var result = await _userManager.CreateAsync(user, request.Password);
        return result;

    }
}
