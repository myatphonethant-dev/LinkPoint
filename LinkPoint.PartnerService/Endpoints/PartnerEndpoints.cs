using LinkPoint.PartnerService.Models;
using LinkPoint.PartnerService.Services;

namespace LinkPoint.PartnerService.Endpoints;

public static class PartnerEndpoints
{
    private static string DateFormat = "yyyy-MM-dd HH:mm:ss:fff tt";

    public static void MapPartnerEndpoints(this IEndpointRouteBuilder app)
    {
        var partnersGroup = app.MapGroup("/partners").WithTags("Partner");

        partnersGroup.MapGet("/", (IPartnerService service) =>
        {
            var partners = service.GetAll()
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

        partnersGroup.MapGet("/{id}", (IPartnerService service, string id) =>
        {
            var p = service.GetById(id);
            if (p == null) return Results.NotFound();

            return Results.Ok(new
            {
                p.PartnerId,
                p.Name,
                p.Email,
                p.Phone,
                CreatedAt = p.CreatedAt.ToString(DateFormat)
            });
        })
        .WithOpenApi(op => { op.Summary = "Get Partner By PartnerId"; return op; });

        partnersGroup.MapPost("/", (IPartnerService service, Tbl_Partner partner) =>
        {
            partner.PartnerId = Guid.NewGuid().ToString();
            service.Create(partner);
            return Results.Created($"/partners/{partner.PartnerId}", partner);
        })
        .WithOpenApi(op => { op.Summary = "Create Partner"; return op; });

        partnersGroup.MapPut("/{id}", (IPartnerService service, string id, Tbl_Partner update) =>
        {
            service.Update(id, update);
            return Results.Ok(update);
        })
        .WithOpenApi(op => { op.Summary = "Update Partner"; return op; });

        partnersGroup.MapDelete("/{id}", (IPartnerService service, string id) =>
        {
            service.Delete(id);
            return Results.NoContent();
        })
        //.WithName("Delete Partner")
        .WithOpenApi(op => { op.Summary = "Delete Partner"; return op; });
    }
}