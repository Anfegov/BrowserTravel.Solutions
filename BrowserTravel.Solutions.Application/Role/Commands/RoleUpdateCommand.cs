using MediatR;

namespace BrowserTravel.Solutions.Application.Role.Commands;

public record RoleUpdateCommand(string RoleId,string NewRoleName) : IRequest<Unit>;
