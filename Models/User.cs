using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace UserModel
{
    public class User
    {
        // Static fields
        public static string CaculateHash(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hashedPassword.Append(bytes[i].ToString("x2"));
            }
            return hashedPassword.ToString();
        }
        //
        public int id { get; private set; }
        public string username { get; private set; }
        public string firstName { get; private set; }
        public string lastName { get; private set; }
        public int noteCount { get; private set; }
        [JsonIgnore]
        public string hashedPassword { get; private set; }
        public User(int id, string username, string firstName, string lastName, int noteCount, string hashedPassword)
        {
            this.id = id;
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.noteCount = noteCount;
            this.hashedPassword = hashedPassword;
        }
        public int IncreaseNoteCount()
        {
            return ++this.noteCount;
        }
        public string GetInfos()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public Login(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}