namespace Fridge.Models.Requests
{
    public class AddUserSocialAuth
    {
        public string SocialId { get; set; }

        public string AuthVia { get; set; }

        public bool IsOwner { get; set; }
    }
}
