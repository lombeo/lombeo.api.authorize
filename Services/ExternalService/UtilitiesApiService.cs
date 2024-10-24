using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Helps;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Lombeo.Api.Authorize.Services.ExternalService
{
    public interface IUtilitiesApiService
    {
        Task<List<UserDTO>> GetUsersByIds(ICollection<int> listIds, bool isAllInfo = false);
        Task<UserDTO> GetUsersById(int userId, bool isAllInfo = false);
        Task<bool> IsInRole(int userId, List<string> roleNames);
        Task<List<UserDTO>> SearchUser(string searchName);
        Task<bool> CheckUserProfile(int userId);
    }
    public class UtilitiesApiService : IUtilitiesApiService
    {
        private readonly string _url;
        private ILogger<UtilitiesApiService> _logger;
        public UtilitiesApiService(ILogger<UtilitiesApiService> logger)
        {
            _logger = logger;
            _url = StaticVariable.UtilitiesApiUrl + "/identity/";
        }

        public async Task<UserDTO> GetUsersById(int userId, bool isAllInfo = false)
        {
            var tmp = await GetUsersByIds(new List<int> { userId }, isAllInfo);
            return (tmp.Any() ? tmp.First() : null);
        }

        public async Task<List<UserDTO>> GetUsersByIds(ICollection<int> listIds, bool isAllInfo = false)
        {
            string url = _url + "user/get-by-ids";

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticVariable.UtilitiesApiToken);
                    client.Timeout = new TimeSpan(0, 0, 3);

                    var jsonData = JsonConvert.SerializeObject(listIds.ToList());
                    var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("GetUsersByIds StatusCode = " + response.StatusCode);
                        _logger.LogError("GetUsersByIds IsSuccessStatusCode = " + response.IsSuccessStatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<UserDTO>>(data);

                    foreach (var item in result)
                    {
                        if (!isAllInfo)
                        {
                            item.UserName = item.UserName.ResolveUserName();
                            item.PhoneNumber = string.Empty;
                            item.Email = string.Empty;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("GetUsersByIds ex url " + url);
                    _logger.LogError("GetUsersByIds ex message " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return new List<UserDTO>();
        }

        public async Task<bool> IsInRole(int userId, List<string> roleNames)
        {
            if (!roleNames.Any())
            {
                return true;
            }
            roleNames = roleNames.Select(x => x.ToLower()).ToList();

            var listRole = await GetListRoleByUserId(userId);
            if (!listRole.Any())
            {
                return false;
            }

            var isRole = listRole.Select(t => t.RoleName.ToLower()).Intersect(roleNames).Any();
            return isRole;
        }

        private async Task<List<RoleUserDTO>> GetListRoleByUserId(int userId)
        {
            string url = $"{_url}roleuser/get-role-user?userId={userId}";
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticVariable.UtilitiesApiToken);
                    client.Timeout = new TimeSpan(0, 0, 3);

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("GetListRoleByUserId StatusCode = " + response.StatusCode);
                        _logger.LogError("GetListRoleByUserId IsSuccessStatusCode = " + response.IsSuccessStatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<RoleUserDTO>>(data);

                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError("GetListRoleByUserId ex url " + url);
                    _logger.LogError("GetListRoleByUserId ex message " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return new List<RoleUserDTO>();
        }

        public async Task<List<UserDTO>> SearchUser(string searchName)
        {
            string url = _url + $"user/search?userName={searchName}";

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticVariable.UtilitiesApiToken);
                    client.Timeout = new TimeSpan(0, 0, 3);

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("GetUsersByIds StatusCode = " + response.StatusCode);
                        _logger.LogError("GetUsersByIds IsSuccessStatusCode = " + response.IsSuccessStatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<UserDTO>>(data);

                    foreach (var item in result)
                    {
                        item.UserName = item.UserName.ResolveUserName();
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("GetUsersByIds ex url " + url);
                    _logger.LogError("GetUsersByIds ex message " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return new List<UserDTO>();
        }

        public async Task<bool> CheckUserProfile(int userId)
        {
            string url = _url + $"user/check-limit-action?userId={userId}";

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticVariable.UtilitiesApiToken);
                    client.Timeout = new TimeSpan(0, 0, 3);

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("CheckUserProfile StatusCode = " + response.StatusCode);
                        _logger.LogError("CheckUserProfile IsSuccessStatusCode = " + response.IsSuccessStatusCode);
                        return false;
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(data);

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CheckUserProfile ex url " + url);
                    _logger.LogError("CheckUserProfile ex message " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }
            return false;
        }
    }
}
