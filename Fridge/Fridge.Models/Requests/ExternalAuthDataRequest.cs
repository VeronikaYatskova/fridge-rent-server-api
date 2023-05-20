namespace Fridge.Models.Requests
{
    public class ExternalAuthDataRequest
    {
        public string Code { get; set; }

        public bool IsOwner { get; set; }
    }
}
