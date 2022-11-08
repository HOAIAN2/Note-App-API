using Microsoft.Extensions;
class AppController
{
    public static LinkedList<NoteModel.Note> noteList = new LinkedList<NoteModel.Note>();
    public static LinkedList<UserModel.User> userList = new LinkedList<UserModel.User>();

    public static void InitUserList()
    {
        var user = new UserModel.User(1, "hoaian_admin", "Hoài Ân", "Lê", 0, "2884e06923c662661afeb064265eda9624a6257818589d33651c83760b84d2e3");
        userList.AddLast(user);
    }
}