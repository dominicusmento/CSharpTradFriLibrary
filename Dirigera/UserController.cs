using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using ApiLibs;
using ApiLibs.General;
using ApiLibs.GitHub;
using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera;

public class UserController : SubService
{
    public UserController(DirigeraController controller) : base(controller)
    {
    }

    public Task<List<User>> GetUsers() => MakeRequest<List<User>>("users/");
    public Task<User> GetMe() => MakeRequest<User>("users/me/");

}

public class User
{
    [JsonProperty("uid")]
    public required string UID { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("audience")]
    public required string Audience { get; set; }

    [JsonProperty("email")]
    public required string Email { get; set; }

    // [JsonProperty("createdTimestamp")]
    // public string CreatedTimestamp { get; set; }

    [JsonProperty("verifiedUid")]
    public required string VerifiedUid { get; set; }

    [JsonProperty("role")]
    public required string Role { get; set; }

}


