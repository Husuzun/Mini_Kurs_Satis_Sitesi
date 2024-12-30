namespace Mini_Kurs_Satis_Sitesi.Web.Services
{
    public class RefreshTokenRepsonse
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiration { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiration { get; set; } = default!;
    }

        public class ResponseDto<T>
    {
            public T? Data { get; set; }
            public int StatusCode { get; set; }
            public object? Error { get; set; }
    }
}
