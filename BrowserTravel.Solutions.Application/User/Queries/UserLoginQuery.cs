using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.User.Queries;
public record UserLoginQuery(string UserName, string Password) : IRequest<UserLoginDto>;
