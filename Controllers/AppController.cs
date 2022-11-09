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
    }
    public static string ReadToken(string bearerJwt)
    {
        string result = "";
        string[] jwt = bearerJwt.Split(' ');
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt[1]);
        var tokenS = jsonToken as JwtSecurityToken;
        if (tokenS == null) return result;
        result = tokenS.Payload["username"].ToString();
        return result;
    }
    public static UserModel.User? FindUser(string username)
    {
        foreach (var user in userList)
        {
            if (user.username == username) return user;
        }
        // Call Database latter
        return null;
    }
    private static void InitUserList()
    {
        var user = new UserModel.User(1, "hoaian_admin", "Hoài Ân", "Lê", 0, "2884e06923c662661afeb064265eda9624a6257818589d33651c83760b84d2e3");
        userList.AddLast(user);
    }
}