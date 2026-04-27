using System;
using System.Text;
using System.Text.Json;
using UserManagement;

class Program
{
    static HttpClient client = new HttpClient();

    // ============================
    // Main（メニュー）
    // ============================
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("=== メニュー ===");
            Console.WriteLine("1: 検索（一覧表示）");
            Console.WriteLine("2: 追加");
            Console.WriteLine("3: 更新");
            Console.WriteLine("4: 削除");
            Console.WriteLine("0: 終了");
            Console.Write("番号を選択してください: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await GetUserList();
                    break;

                case "2":
                    await AddUserFromInput();
                    break;

                case "3":
                    await UpdateUserFromInput();
                    break;

                case "4":
                    Console.Write("削除するIDを入力（0で中断）: ");
                    string deleteId = Console.ReadLine();
                    if (deleteId == "0") break;
                    await DeleteUser(deleteId);
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("無効な入力です。");
                    break;
            }

            Console.WriteLine();
        }
    }

    // ============================
    // 一覧取得（検索）
    // ============================
    static async Task GetUserList()
    {
        var url = "http://192.168.82.44:8090/GetUserList";
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(json);

        Console.WriteLine("=== Users ===");

        Console.WriteLine(
            PadRightJP("ID", 10) +
            PadRightJP("姓", 20) +
            PadRightJP("名", 20) +
            PadRightJP("性別", 8) +
            PadRightJP("年齢", 8) +
            PadRightJP("誕生日", 12)
        );

        foreach (var user in apiResponse.resultData)
        {
            if (string.IsNullOrWhiteSpace(user.id) &&
                string.IsNullOrWhiteSpace(user.familyName) &&
                string.IsNullOrWhiteSpace(user.name))
            {
                continue;
            }

            string sexStr = user.sex switch
            {
                1 => "男",
                2 => "女",
                3 => "その他",
                _ => "不明"
            };

            string birthdayStr = DateTime.TryParseExact(
                user.birthday.ToString(),
                "yyyyMMdd",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime bd
            ) ? bd.ToString("yyyy/MM/dd") : "不明";

            Console.WriteLine(
                PadRightJP(user.id, 10) +
                PadRightJP(user.familyName, 20) +
                PadRightJP(user.name, 20) +
                PadRightJP(sexStr, 8) +
                PadRightJP(user.age + "歳", 8) +
                PadRightJP(birthdayStr, 12)
            );
        }
    }

    // ============================
    // 追加（POST）
    // ============================
    static async Task AddUserFromInput()
    {
        Console.Write("ID: ");
        string id = Console.ReadLine();

        Console.Write("姓: ");
        string family = Console.ReadLine();

        Console.Write("名: ");
        string name = Console.ReadLine();

        Console.Write("性別 (1:男 2:女 3:その他): ");
        int sex = int.Parse(Console.ReadLine());

        Console.Write("年齢: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("誕生日 (yyyyMMdd): ");
        string birthdayInput = Console.ReadLine();

        // 入力チェック（8桁の数字かどうか）
        if (birthdayInput.Length != 8 || !birthdayInput.All(char.IsDigit))
        {
            Console.WriteLine("誕生日は yyyyMMdd の8桁数字で入力してください。例: 20250601");
            return;
        }

        int birthday = int.Parse(birthdayInput);



        Console.Write("住所: ");
        string address = Console.ReadLine();

        Console.Write("メモ: ");
        string note = Console.ReadLine();

        var newUser = new
        {
            id,
            familyName = family,
            name,
            sex,
            age,
            birthday,
            address,
            note
        };

        var json = JsonSerializer.Serialize(newUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://192.168.82.44:8090/AddUser", content);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    // ============================
    // 更新（PUT）
    // ============================
    static async Task UpdateUserFromInput()
    {
        Console.WriteLine("=== 更新（0で中断） ===");

        Console.Write("更新するID: ");
        string id = Console.ReadLine();
        if (id == "0") return;

        Console.Write("新しい姓: ");
        string family = Console.ReadLine();
        if (family == "0") return;

        Console.Write("新しい名: ");
        string name = Console.ReadLine();
        if (name == "0") return;

        Console.Write("性別 (1:男 2:女 3:その他): ");
        string sexInput = Console.ReadLine();
        if (sexInput == "0") return;
        int sex = int.Parse(sexInput);

        Console.Write("年齢: ");
        string ageInput = Console.ReadLine();
        if (ageInput == "0") return;
        int age = int.Parse(ageInput);

        Console.Write("誕生日 (yyyyMMdd): ");
        string birthdayInput = Console.ReadLine();
        if (birthdayInput == "0") return;

        if (birthdayInput.Length != 8 || !birthdayInput.All(char.IsDigit))
        {
            Console.WriteLine("誕生日は yyyyMMdd の8桁数字で入力してください。例: 20250601");
            return;
        }
        int birthday = int.Parse(birthdayInput);

        Console.Write("住所: ");
        string address = Console.ReadLine();
        if (address == "0") return;

        Console.Write("メモ: ");
        string note = Console.ReadLine();
        if (note == "0") return;

        var updateUser = new
        {
            id,
            familyName = family,
            name,
            sex,
            age,
            birthday,
            address,
            note
        };

        var json = JsonSerializer.Serialize(updateUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync("http://192.168.82.44:8090/UpdateUser", content);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }


    // ============================
    // 削除（DELETE）
    // ============================
    static async Task DeleteUser(string id)
    {
        var response = await client.DeleteAsync($"http://192.168.82.44:8090/DeleteUser?id={id}");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    // ============================
    // 全角対応 PadRight
    // ============================
    static string PadRightJP(string text, int totalWidth)
    {
        int length = 0;
        foreach (char c in text)
        {
            length += (c >= 0x80) ? 2 : 1;
        }

        int pad = totalWidth - length;
        return pad > 0 ? text + new string(' ', pad) : text;
    }
}
