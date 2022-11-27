namespace Fridge.Data.Models.Abstracts
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
