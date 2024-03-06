using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BrowserTravel.Solutions.Application.User.Commands;

public record UserRegisterCommand(string UserName, string Password) : IRequest<IdentityResult>;
