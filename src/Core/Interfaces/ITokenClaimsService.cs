using Core.Dtos;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface ITokenClaimsService
{
    Task<List<string>> GetUserRolesAsync(string userName);
    Task<string> GetTokenAsync(LoginInfoDto loginInfo);
}
