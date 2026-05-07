using Newtonsoft.Json;

namespace UserManagement
{
    public class ApiClient
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "https://localhost:5001";

        public ApiClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<UserListResponse> GetUsersAsync()
        {
            var response = await httpClient.GetAsync($"{baseUrl}/users");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserListResponse>(json);
        }
    }

}