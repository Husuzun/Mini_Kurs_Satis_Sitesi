namespace Mini_Kurs_Satis_Sitesi.Web.Services
{
    public class IdentityServerService(HttpClient client)
    {
        public async Task GetCourses()
        {
            var response = await client.GetAsync("/api/Course");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
