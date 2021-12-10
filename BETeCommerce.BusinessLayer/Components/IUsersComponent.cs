using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;
using System.Security;
using System.Threading.Tasks;

namespace BETeCommerce.BusinessLayer.Components
{
    public interface IUsersComponent
    {
        Task<UserDetailsResponse> RegisterUser(AddOrUdateUserRequest request, SecureString securedPassword);
        Task<UserDetailsResponse> LoginUser(LoginUserRequest request, SecureString securedPassword);
    }
}
