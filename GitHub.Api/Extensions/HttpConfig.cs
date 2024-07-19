using GitHub.Api.Middlewares;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace GitHub.Api.Extensions
{
    public static class HttpConfig
    {
        public static IServiceCollection AddHttpConfig(this IServiceCollection services)
        {
            services.AddResiliencePipeline("resilience",
                pipelineBuilder =>
                {
                    pipelineBuilder.AddRetry(new RetryStrategyOptions
                    {
                        ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                        Delay = TimeSpan.FromSeconds(2),
                        MaxRetryAttempts = 3,
                        BackoffType = DelayBackoffType.Constant,
                        UseJitter = true
                    })
                    .AddTimeout(TimeSpan.FromSeconds(20));

                    pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
                    {
                        ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                        FailureRatio = 0.5,
                        BreakDuration = TimeSpan.FromSeconds(10),
                        OnOpened = static args =>
                        {
                            return ValueTask.CompletedTask;
                        }
                    });
                }
            );

            services.AddHttpClient(GitHubSettings.GitHubApi, client =>
            {
                client.BaseAddress = new Uri(GitHubSettings.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            })
                .AddHttpMessageHandler<HttpResilience>();

            return services;
        }
    }
}
