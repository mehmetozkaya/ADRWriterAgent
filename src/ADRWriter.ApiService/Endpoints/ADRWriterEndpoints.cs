using System.Text.Json;
using ADRWriter.ApiService.Services;

namespace ADRWriter.ApiService.Endpoints;

public record WriteRequest(string Context);

public static class ADRWriterEndpoints
{
    public static void MapADRWriterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/write");

        group.MapPost("/", async (WriteRequest request, ADRWriterService service, HttpResponse response, CancellationToken ct) =>
        {
            response.ContentType = "application/json";

            await response.WriteAsync("[", ct);
            bool first = true;

            await foreach (var chunk in service.WriteStreamingAsync(request.Context, ct))
            {
                if (!first)
                    await response.WriteAsync(",", ct);

                await response.WriteAsync(JsonSerializer.Serialize(chunk), ct);
                await response.Body.FlushAsync(ct);
                first = false;
            }

            await response.WriteAsync("]", ct);
            await response.Body.FlushAsync(ct);
        })
        .WithName("WriteADR");
    }
}
