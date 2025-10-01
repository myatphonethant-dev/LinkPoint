using LinkPoint.IdentityService.Models;
using LinkPoint.IdentityService.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkPoint.IdentityService.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var usersGroup = app.MapGroup("/users").WithTags("User");

        usersGroup.MapPost("/register", async ([FromBody] RegisterRequestModel request, IUserRepository repo) =>
        {
            var user = await repo.RegisterAsync(request);
            return Results.Created($"/users/{user.UserId}", new { user.UserId, user.Username, user.Email, user.CreatedAt });
        })
        .WithOpenApi();

        usersGroup.MapPost("/login", async ([FromBody] LoginRequestModel request, IUserRepository repo) =>
        {
            try
            {
                var response = await repo.LoginAsync(request);
                return Results.Ok(response);
            }
            catch
            {
                return Results.Unauthorized();
            }
        })
        .WithOpenApi();

        usersGroup.MapPut("/{userId:guid}", [Authorize] async (Guid userId, [FromBody] UpdateProfileRequestModel dto, IUserRepository repo, HttpContext ctx) =>
        {
            var uidClaim = ctx.User.FindFirst("uid")?.Value;
            if (uidClaim == null || uidClaim != userId.ToString())
                return Results.Forbid();

            var user = await repo.UpdateProfileAsync(userId, dto.Username, dto.AvatarUrl);
            return user == null ? Results.NotFound() : Results.Ok(new { user.UserId, user.Username, user.Email, user.AvatarUrl });
        })
        .RequireAuthorization()
        .WithOpenApi();
    }
}