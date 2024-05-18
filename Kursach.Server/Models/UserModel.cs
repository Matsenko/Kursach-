namespace Kursach.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool isDeleted { get; set; } = false;
        public int RequestsCount { get; set; } = 0;
    }
}
