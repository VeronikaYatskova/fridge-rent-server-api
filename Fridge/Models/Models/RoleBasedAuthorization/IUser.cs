namespace Models.Models.RoleBasedAuthorization
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Role { get; }

        public string Token { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }
    }
}
