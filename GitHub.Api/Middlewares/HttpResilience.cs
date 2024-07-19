using Polly.Registry;

namespace GitHub.Api.Middlewares
{
    public class HttpResilience : DelegatingHandler
    {
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public HttpResilience(ResiliencePipelineProvider<string> pipelineProvider)
        {
            _pipelineProvider = pipelineProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var pipeline = _pipelineProvider.GetPipeline("resilience");

                return await pipeline.ExecuteAsync(async ct => await base.SendAsync(request, ct));
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
