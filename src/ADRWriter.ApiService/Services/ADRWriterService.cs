using Microsoft.Agents.AI;

namespace ADRWriter.ApiService.Services;

public class ADRWriterService(
    [FromKeyedServices("ADRWriterAgent")] AIAgent adrWriterAgent)
{
    public async IAsyncEnumerable<string> WriteStreamingAsync(
        string context, 
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var update in adrWriterAgent.RunStreamingAsync(context, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrEmpty(update.Text))
            {
                yield return update.Text;
            }
        }        
    }
}
