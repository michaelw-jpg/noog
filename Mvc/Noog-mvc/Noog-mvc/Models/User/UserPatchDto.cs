namespace Noog_mvc.Models.User
{
    public class UserPatchDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
