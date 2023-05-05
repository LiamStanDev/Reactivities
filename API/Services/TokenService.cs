using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

// Services means that this logic don't need to use the data
// from the database.

// What is JWT?
// JWT has three parts: header, payload, and signature.
// Algorithm and datatype are in header, and claims are in payload.
// After base64 "encode" Header and payload, then combine them to a string.
// The string "encrypt" with scret key into a signature.
// Signature is used to check that the data or header don't change by others.
public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(AppUser user)
    {
        // Claim is used to claim what you are.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // key is used to decrypt the message.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])); // it need be very longer

        // creds(signature) is used for preventing from third-party change the JWT content when transfer to the server.
        // creds is in the header of JWT.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
