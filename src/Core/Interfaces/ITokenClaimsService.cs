using Core.Dtos;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ITokenClaimsService
{
    Task<List<string>> GetUserRolesAsync(string userName);
    Task<string> GetTokenAsync(LoginInfoDto loginInfo);
}
