using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public record RoleAssignsToUserCommand(string UserId, IEnumerable<string> Roles) : IRequest<IdentityResult>;
