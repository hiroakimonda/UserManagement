

namespace WinFormsApp_DotNet
{
    public class User
    {
        public string id { get; set; } = "";
        public string familyName { get; set; } = "";
        public string name { get; set; } = "";
        public int sex { get; set; }          // ← 数値に変更
        public int age { get; set; }
        public int birthday { get; set; }     // ← 数値に変更
        public string address { get; set; } = "";
        public string? note { get; set; }
    }

}