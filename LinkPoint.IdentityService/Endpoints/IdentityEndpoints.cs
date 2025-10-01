using LinkPoint.IdentityService.Models;
using LinkPoint.IdentityService.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace LinkPoint.IdentityService.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("/users").WithTags("User");

        group.MapPost("/register", async (
            [FromBody] RegisterRequestModel request,
            IUserRepository userRepo) =>
        {
            try
            {
                var user = await userRepo.RegisterAsync(request);
                return Results.Created($"/users/{user.UserId}", new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });

        group.MapPost("/login", async (
            [FromBody] LoginRequestModel request,
            IUserRepository userRepo) =>
        {
            try
            {
                var response = await userRepo.LoginAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Unauthorized();
            }
        });

        group.MapPut("/{userId:guid}", async (
            Guid userId,
            [FromBody] UpdateProfileRequestModel request,
            IUserRepository userRepo) =>
        {
            try
            {
                var user = await userRepo.UpdateProfileAsync(userId, request.Username, request.AvatarUrl);
                return user == null
                    ? Results.NotFound()
                    : Results.Ok(new
                    {
                        user.UserId,
                        user.Username,
                        user.Email,
                        user.AvatarUrl
                    });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
    }
}

public record UpdateProfileRequestModel(string? Username, string? AvatarUrl);