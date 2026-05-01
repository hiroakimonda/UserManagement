public class User
{
    public string id { get; set; }
    public string familyName { get; set; }
    public string name { get; set; }
    public int sex { get; set; }
    public int age { get; set; }
    public string birthday { get; set; }   // ← 修正ポイント
    public string address { get; set; }
    public string note { get; set; }
}
