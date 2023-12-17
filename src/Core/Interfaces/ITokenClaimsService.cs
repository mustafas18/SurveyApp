using Domain.Dtos;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface ITokenClaimsService
{
    Task<List<string>> GetUserRolesAsync(string userName);
    Task<string> GetTokenAsync(LoginInfoDto loginInfo);
}
