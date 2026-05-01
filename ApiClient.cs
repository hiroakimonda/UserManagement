using System.Text;
using System.Text.Json;

public class ApiClient
{
    // ▼ GetUserListAsync（一覧取得）
    public static async Task<List<User>> GetUserListAsync(string id)
    {
        using var client = new HttpClient();

        string url = $"http://192.168.82.44:8090/GetUserList?id={id}";
        var response = await client.GetAsync(url);
        string json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<UserListResponse>(json);

        return result.resultData;
    }

    // ▼ AddUserAsync（ユーザー登録）
    public static async Task<string> AddUserAsync(User user)
    {
        using var client = new HttpClient();

        string url = "http://192.168.82.44:8090/AddUser";

        string json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);
        string resultJson = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<UserResponse>(resultJson);

        return result.resultCd;  // "0000" が成功
    }
}
