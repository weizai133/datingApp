namespace backend.Model
{
    public class User
    {
        public int userId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt  { get; set; }
    }
}