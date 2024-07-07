using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using ApiLibs;
using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera;

public class DirigeraController : Service
{
    public DirigeraController(string hostUrl) : base(hostUrl + ":8443/v1")
    {
        UserController = new UserController(this);
        DeviceController = new DeviceController(this);
    }

    public DirigeraController(string hostUrl, string token) : this(hostUrl)
    {
        AddStandardHeader(new Param("Authorization", "Bearer " + token));
    }

    public UserController UserController { get; set; }
    public DeviceController DeviceController { get; set; }

    public static readonly string CODE_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
    public static readonly int CODE_LENGTH = 128;
    public static readonly string AUDIENCE = "homesmart.local";
    public static readonly string CHALLENGE_METHOD = "S256";

    public static string generateCodeVerifier()
    {
        StringBuilder res = new();
        Random random = new();

        for (int i = 0; i < CODE_LENGTH; i++)
        {
            res.Append(CODE_ALPHABET.ElementAt(random.Next(0, CODE_ALPHABET.Length)));
        }
        return res.ToString();

    }

    public static string calculateCodeChallenge(string code)
    {
        SHA256 mySHA256 = SHA256.Create();
        byte[] byteHash = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(code));
        var base64 = Base64SafeEncode(byteHash);
        return base64.Substring(0, base64.Length - 1);
    }

    public Task<Authorize> Authorize(string challenge)
    {
        return MakeRequest<Authorize>("oauth/authorize", Call.GET, new List<Param> {
            new("audience", AUDIENCE),
            new("response_type", "code"),
            new("code_challenge", challenge),
            new("code_challenge_method", CHALLENGE_METHOD)
        });
    }

    public Task<TokenObject> Pair(string dirigeraCode, string generatedCode, string name)
    {
        return MakeRequest<TokenObject>("oauth/token", Call.POST, new List<Param> {
            new("code", dirigeraCode),
            new("name", name),
            new("grant_type", "authorization_code"),
            new("code_verifier", generatedCode),
        });
    }

    public static string Base64SafeEncode(byte[] encbuff)
    {
        return System.Convert.ToBase64String(encbuff).Replace("=", ",").Replace("+", "-").Replace("/", "_");
    }
}

public class Authorize
{
    [JsonProperty("code")]
    public required string Code { get; set; }
}

public class TokenObject
{
    [JsonProperty("access_token")]
    public required string AccessToken { get; set; }
}
