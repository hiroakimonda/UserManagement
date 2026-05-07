using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace UserManagement
{
    public  class Program
    {
        static List<User> users = new List<User>();
        
        public static async Task LoadUsersFromServer()
        {
            using (var client = new HttpClient())
            {
                string url = "http://192.168.82.44:8090/GetUserList";

                try
                {
                    var json = await client.GetStringAsync(url);

                    var response = JsonSerializer.Deserialize<ApiResponse>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                    users = response.resultData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("サーバからデータを取得できませんでした。");
                    Console.WriteLine("エラー内容:");
                    Console.WriteLine(ex.ToString());
                    Console.ReadKey();
                }
            }
        }

        static async Task Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("        User Management System       ");
                Console.WriteLine("====================================");

                Console.WriteLine("[ ユーザー操作 ]");
                Console.WriteLine("  1. ユーザー一覧");
                Console.WriteLine("  2. ユーザー登録");
                Console.WriteLine("  3. ユーザー更新");
                Console.WriteLine("  4. ユーザー削除\n");

                Console.WriteLine("[ ファイル操作 ]");
                Console.WriteLine("  5. JSON保存");
                Console.WriteLine("  6. JSON読み込み");
                Console.WriteLine("  7. CSV保存");
                Console.WriteLine("  8. CSV読み込み\n");

                Console.WriteLine("[ システム ]");
                Console.WriteLine("  0. 終了\n");

                Console.Write("番号を選択してください: ");
                string choice = Console.ReadLine();




                switch (choice)
                {

                    case "1":
                        await LoadUsersFromServer();
                        ShowUsers();
                        break;


                    case "2":
                        RegisterUser();
                        break;

                    case "3":
                        EditUser();
                        break;

                    case "4":
                        DeleteUser();
                        break;

                    case "5":
                        SaveToJson();
                        break;

                    case "6":
                        LoadFromJson();
                        break;

                    case "7":
                        SaveToCsv();
                        break;

                    case "8":
                        LoadFromCsv();
                        break;

                    case "0":
                        return;
                    default:
                        Console.WriteLine("無効な入力です。");
                        Console.ReadKey();
                        break;
                }
            }
        }


        //一覧
        public static void ShowUsers()
        {
            Console.Clear();
            Console.WriteLine("=== ユーザー一覧 ===");

            if (users.Count == 0)
            {
                Console.WriteLine("ユーザーが登録されていません。");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.id}");
                    Console.WriteLine($"名前: {user.familyName} {user.name}");
                    Console.WriteLine($"年齢: {user.age}");
                    Console.WriteLine($"住所: {user.address}");
                    Console.WriteLine($"備考: {user.note}");
                    Console.WriteLine("------------------------------");
                }
            }

            Console.WriteLine("\n続けるには何かキーを押してください...");
            Console.ReadKey();
        }

        // 登録機能
        static void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("=== ユーザー登録 ===");

            User user = new User();

            Console.Write("ID: ");
            user.id = Console.ReadLine();

            Console.Write("姓: ");
            user.familyName = Console.ReadLine();

            Console.Write("名: ");
            user.name = Console.ReadLine();

            Console.Write("年齢: ");
            user.age = int.Parse(Console.ReadLine());

            Console.Write("住所: ");
            user.address = Console.ReadLine();

            Console.Write("備考: ");
            user.note = Console.ReadLine();

            users.Add(user);

            Console.WriteLine("\n登録が完了しました。");
            Console.ReadKey();
        }
        //更新・編集
        static void EditUser()
        {
            Console.Clear();
            Console.WriteLine("=== ユーザー更新 ===");
            Console.Write("更新するユーザーのIDを入力してください: ");
            string targetId = Console.ReadLine();

            // 該当ユーザーを検索
            User user = users.Find(u => u.id == targetId);
            if (user == null)
            {
                Console.WriteLine("指定されたIDのユーザーが見つかりません。");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\n現在の情報：");
            Console.WriteLine($"姓: {user.familyName}");
            Console.WriteLine($"名: {user.name}");
            Console.WriteLine($"年齢: {user.age}");
            Console.WriteLine($"住所: {user.address}");
            Console.WriteLine($"備考: {user.note}");
            Console.WriteLine("\n--- 新しい情報を入力してください（空欄なら変更しない） ---");
            Console.Write("姓: ");
            string newFamily = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFamily))
                user.familyName = newFamily;
            Console.Write("名: ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                user.name = newName;
            Console.Write("年齢: ");
            string newAge = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAge))
                user.age = int.Parse(newAge);
            Console.Write("住所: ");
            string newAddress = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAddress))
                user.address = newAddress;
            Console.Write("備考: ");
            string newNote = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newNote))
                user.note = newNote;
            Console.WriteLine("\n更新が完了しました。");
            Console.ReadKey();
        }
        //削除
        static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("=== ユーザー削除 ===");
            Console.Write("削除するユーザーのIDを入力してください: ");
            string targetId = Console.ReadLine();
            // 該当ユーザーを検索
            User user = users.Find(u => u.id == targetId);
            if (user == null)
            {
                Console.WriteLine("指定されたIDのユーザーが見つかりません。");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\n以下のユーザーを削除します：");
            Console.WriteLine($"ID: {user.id}");
            Console.WriteLine($"名前: {user.familyName} {user.name}");
            Console.WriteLine($"年齢: {user.age}");
            Console.WriteLine($"住所: {user.address}");
            Console.WriteLine($"備考: {user.note}");
            Console.Write("\n本当に削除しますか？ (y/n): ");
            string confirm = Console.ReadLine();
            if (confirm.ToLower() == "y")
            {
                users.Remove(user);
                Console.WriteLine("\n削除が完了しました。");
            }
            else
            {
                Console.WriteLine("\n削除をキャンセルしました。");
            }
            Console.ReadKey();
        }
        //保存
        static void SaveToJson()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("users.json", json);
            Console.WriteLine("JSONファイルに保存しました。");
            Console.ReadKey();
        }
        static void LoadFromJson()
        {
            if (!File.Exists("users.json"))
            {
                Console.WriteLine("JSONファイルが存在しません。");
                Console.ReadKey();
                return;
            }
            string json = File.ReadAllText("users.json");
            users = JsonSerializer.Deserialize<List<User>>(json);
            Console.WriteLine("JSONファイルから読み込みました。");
            Console.ReadKey();
        }
        static void SaveToCsv()
        {
            using (var writer = new StreamWriter("users.csv"))
            {
                writer.WriteLine("id,familyName,name,age,address,note");

                foreach (var user in users)
                {
                    writer.WriteLine($"{user.id},{user.familyName},{user.name},{user.age},{user.address},{user.note}");
                }
            }

            Console.WriteLine("CSVファイルに保存しました。");
            Console.ReadKey();
        }
        static void LoadFromCsv()
        {
            if (!File.Exists("users.csv"))
            {
                Console.WriteLine("CSVファイルが存在しません。");
                Console.ReadKey();
                return;
            }

            users.Clear();

            using (var reader = new StreamReader("users.csv"))
            {
                // 1行目はヘッダーなので読み飛ばす
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var parts = line.Split(',');

                    users.Add(new User
                    {
                        id = parts[0],
                        familyName = parts[1],
                        name = parts[2],
                        age = int.Parse(parts[3]),
                        address = parts[4],
                        note = parts[5]
                    });
                }
            }

            Console.WriteLine("CSVファイルから読み込みました。");
            Console.ReadKey();
        }
    }
}
