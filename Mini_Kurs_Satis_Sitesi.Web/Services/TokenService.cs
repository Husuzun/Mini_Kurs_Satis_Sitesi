namespace Mini_Kurs_Satis_Sitesi.Web.Services
{
    public class TokenService
    {
        private readonly HttpClient _client;
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        
        public TokenService(HttpClient client)
        {
            _client = client;
        }

        public void Set(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public RefreshTokenRepsonse GetAccessTokenByRefreshToken(string refreshToken)
        {
            var response = _client.PostAsJsonAsync<RefreshTokenRequestDto>("/api/Auth/CreateTokenByRefreshToken", new RefreshTokenRequestDto(refreshToken)).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadFromJsonAsync<ResponseDto<RefreshTokenRepsonse>>().Result;
                return content!.Data!;
            }
            return null;
        }
    }
}
