using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using System.Security;
using System.Threading.Tasks;

namespace BETeCommerce.DataLayer.Repositories
{
    public interface IUsersRepository
    {
        Task<UserDetailsDto> RegisterUser(AddOrUdateUserRequest request, SecureString securedPassword);
        Task<UserDetailsDto> LoginUser(LoginUserRequest request, SecureString securedPassword);
    }
}
