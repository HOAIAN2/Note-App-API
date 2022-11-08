using System.Text.Json;
namespace NoteModel
{
    public class Note
    {
        ////
        public int id { get; private set; }
        public int ownerID { get; private set; }
        public string title { get; private set; }
        public string content { get; private set; }
        public long createDate { get; private set; }
        public long lastModifyDate { get; private set; }
        public Note(int id, int ownerID, string title, string content, long createDate, long lastModifyDate)
        {
            this.id = id;
            this.ownerID = ownerID;
            this.title = title;
            this.content = content;
            this.createDate = createDate;
            this.lastModifyDate = lastModifyDate;
        }
        public string getInfos()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    public class CreateNote
    {
        public string title { get; private set; }
        public string content { get; private set; }
        public CreateNote(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
    }
}