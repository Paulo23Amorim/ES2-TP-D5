using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Projeto_ES2.Client.Interceptors
{
    public class AuthTokenInterceptor : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        
        public AuthTokenInterceptor(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Remove surrounding quotes if they exist
                token = token.Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}