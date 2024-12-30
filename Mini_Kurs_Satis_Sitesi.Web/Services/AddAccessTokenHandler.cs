using System.Net;
using System.Net.Http.Headers;

namespace Mini_Kurs_Satis_Sitesi.Web.Services
{
    public class AddAccessTokenHandler(TokenService tokenService):DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = tokenService.AccessToken;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

              
            var response =  await base.SendAsync(request, cancellationToken);

            if(response.StatusCode == HttpStatusCode.Unauthorized)   
            {
                var refreshToken = tokenService.RefreshToken;

                var responseAsRefreshToken = tokenService.GetAccessTokenByRefreshToken(refreshToken);

                if(responseAsRefreshToken == null)
                {
                    throw new UnauthorizedAccessException();
                }

                tokenService.Set(responseAsRefreshToken.AccessToken, responseAsRefreshToken.RefreshToken);
                
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", responseAsRefreshToken.AccessToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
