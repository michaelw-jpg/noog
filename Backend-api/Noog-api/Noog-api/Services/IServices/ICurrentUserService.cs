namespace Noog_api.Services.IServices
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; set; }
        public void SetUserId();

    }
}
