using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ADRWriter.Web.ApiClients;

public class ADRWriterApiClient(HttpClient httpClient)
{
    public async IAsyncEnumerable<string> WriteADRStreamingAsync(
        string context,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/write")
        {
            Content = JsonContent.Create(new { Context = context })
        };

        var response = await httpClient.SendAsync(
            requestMessage,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await foreach (var chunk in JsonSerializer.DeserializeAsyncEnumerable<string>(stream, cancellationToken: cancellationToken))
        {
            if (chunk is not null)
            {
                yield return chunk;
            }
        }
    }
}
