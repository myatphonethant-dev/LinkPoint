using LinkPoint.PartnerService.Models;
using LinkPoint.PartnerService.Services;

namespace LinkPoint.PartnerService.Endpoints;

public static class PartnerEndpoints
{
    private static string DateFormat = "yyyy-MM-dd HH:mm:ss:fff tt";

    public static void MapPartnerEndpoints(this IEndpointRouteBuilder app)
    {
        var partnersGroup = app.MapGroup("/partners").WithTags("Partner");

        partnersGroup.MapGet("/", async (IPartnerService service) =>
        {
            var partners = await service.GetAll();
            var result = partners
                .OrderByDescending(p => p.PartnerId)
                .Select(p => new
                {
                    p.PartnerId,
                    p.Name,
                    p.Email,
                    p.Phone,
                    CreatedAt = p.CreatedAt.ToString(DateFormat)
                });
            return Results.Ok(partners);
        })
        .WithOpenApi(op => { op.Summary = "Get All Partners"; return op; });

        partnersGroup.MapGet("/{id}", async (IPartnerService service, string id) =>
        {
            var partner = await service.GetById(id)!;
            if (partner is null) return Results.NotFound();

            return Results.Ok(new
            {
                partner.PartnerId,
                partner.Name,
                partner.Email,
                partner.Phone,
                CreatedAt = partner.CreatedAt.ToString(DateFormat)
            });
        })
        .WithOpenApi(op => { op.Summary = "Get Partner By PartnerId"; return op; });

        partnersGroup.MapPost("/", async (IPartnerService service, Tbl_Partner partner) =>
        {
            partner.PartnerId = Guid.NewGuid().ToString();
            await service.Create(partner);
            return Results.Created($"/partners/{partner.PartnerId}", partner);
        })
        .WithOpenApi(op => { op.Summary = "Create Partner"; return op; });

        partnersGroup.MapPut("/{id}", async (IPartnerService service, string id, Tbl_Partner update) =>
        {
            await service.Update(id, update);
            return Results.Ok(update);
        })
        .WithOpenApi(op => { op.Summary = "Update Partner"; return op; });

        partnersGroup.MapDelete("/{id}", async (IPartnerService service, string id) =>
        {
            await service.Delete(id);
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Delete Partner"; return op; });
    }
}