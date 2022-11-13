using System.IdentityModel.Tokens.Jwt;
class AppController
{
    public static LinkedList<NoteModel.Note> noteList = new LinkedList<NoteModel.Note>();
    public static LinkedList<UserModel.User> userList = new LinkedList<UserModel.User>();
    public static string? connectString { get; private set; }
    public static string? secretKey { get; private set; }
    public static void AppInit(string configureConnect, string configureKey)
    {
        connectString = configureConnect;
        secretKey = configureKey;
        InitUserList();
        InitNoteList();
    }
    public static string? ReadTokenUsername(string bearerJwt)
    {
        string? result = null;
        string[] jwt = bearerJwt.Split(' ');
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt[1]);
        var tokenS = jsonToken as JwtSecurityToken;
        if (tokenS == null) return result;
        result = tokenS.Payload["username"].ToString();
        return result;
    }
    public static int ReadTokenUserID(string bearerJwt)
    {
        int result = 0;
        string[] jwt = bearerJwt.Split(' ');
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt[1]);
        var tokenS = jsonToken as JwtSecurityToken;
        if (tokenS == null) return result;
        string? s = tokenS.Payload["id"].ToString();
        if (s == null) return result;
        else result = int.Parse(s);
        return result;
    }
    public static UserModel.User? FindUser(int id)
    {
        foreach (var user in userList)
        {
            if (user.id == id) return user;
        }
        // Call Database latter
        return null;
    }
    public static void HandleIncreaseNoteCount(int userID)
    {
        var user = FindUser(userID);
        if (user == null) return;
        int count = user.IncreaseNoteCount();
        string queryString = $"UPDATE user SET note_count = {count} WHERE id = {userID};";
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand command;
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectString;
            command = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            conn.Open();
            MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { }
            conn.Close();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static void InitUserList()
    {
        string queryString = "SELECT * FROM user";
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand command;
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectString;
            command = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            conn.Open();
            MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string username = reader.GetString(1);
                string firstName = reader.GetString(2);
                string lastName = reader.GetString(3);
                int noteCount = reader.GetInt32(4);
                string hashedPassword = reader.GetString(5);
                var user = new UserModel.User(id, username, firstName, lastName, noteCount, hashedPassword);
                userList.AddLast(user);
            }
            conn.Close();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static void InitNoteList()
    {
        NoteModel.Note? note = null;
        string queryString = "SELECT * FROM note";
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand command;
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectString;
            command = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            conn.Open();
            MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int ownerID = reader.GetInt32(1);
                string title = reader.GetString(2);
                string content = reader.GetString(3);
                long createDate = reader.GetInt64(4);
                long lastModifyDate = reader.GetInt64(5);
                note = new NoteModel.Note(id, ownerID, title, content, createDate, lastModifyDate);
                noteList.AddLast(note);
            }
            conn.Close();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static NoteModel.Note? SaveNote(int userID, string title, string content)
    {
        NoteModel.Note? note = null;
        var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        string queryString = $"INSERT INTO note(owner_id, title, content, create_date, last_modified) VALUE({userID}, '{title}', '{content}', {time}, {time})";
        string queryString1 = "SELECT * FROM note ORDER BY id DESC LIMIT 1;";
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand command;
        MySql.Data.MySqlClient.MySqlCommand command1;
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectString;
            command = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            command1 = new MySql.Data.MySqlClient.MySqlCommand(queryString1, conn);
            conn.Open();
            MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { }
            conn.Close();
            conn.Open();
            MySql.Data.MySqlClient.MySqlDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                int id = reader1.GetInt32(0);
                int ownerID = reader1.GetInt32(1);
                string titles = reader1.GetString(2);
                string contents = reader1.GetString(3);
                long createDate = reader1.GetInt64(4);
                long lastModifyDate = reader1.GetInt64(5);
                note = new NoteModel.Note(id, ownerID, titles, contents, createDate, lastModifyDate);
                noteList.AddLast(note);
            }
            conn.Close();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex);
        }
        return note;
    }
}