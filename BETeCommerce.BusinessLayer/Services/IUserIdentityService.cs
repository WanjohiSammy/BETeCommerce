
namespace BETeCommerce.BusinessLayer.Services
{
    public interface IUserIdentityService
    {
        bool IsAuthenticated();
        string GetEmail();
        string GetUserName();
        string GetUserId();
    }
}
