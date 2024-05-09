using System.Net;

namespace SoframiPaylas.WebUI.ExternalService.Handler;
public class RetryHandler : DelegatingHandler
{
    private readonly int _maxRetries = 3;
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(2);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        for (int i = 0; i < _maxRetries; i++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else if (response.StatusCode >= (HttpStatusCode)500 && response.StatusCode <= (HttpStatusCode)599)
                {
                    Console.WriteLine($"Retry {i + 1} for {request.RequestUri}");
                    await Task.Delay(_delay, cancellationToken);
                }
                else
                {
                    return response;
                }
            }
            catch (HttpRequestException) when (i < _maxRetries - 1)
            {
                Console.WriteLine($"Exception on request, retrying {i + 1}");
                await Task.Delay(_delay, cancellationToken);
            }
        }

        // Max retries exceeded
        throw new HttpRequestException("Maximum retries exceeded for the request.");
    }
}
