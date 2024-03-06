using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public record RoleRegisterCommand(string RoleName) : IRequest<IdentityResult>;
