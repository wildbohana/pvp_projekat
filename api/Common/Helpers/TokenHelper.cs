using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// Ovo je iz Reddit-a
// Fale nugeti, i ne može da instancira SymmetricSecurityKey

namespace Common.Helpers
{
    //#region JwtKey
    //public class JwtKey
    //{
    //    private readonly string _secretKey;

    //    public JwtKey(string secretKey)
    //    {
    //        _secretKey = secretKey;
    //    }

    //    public SymmetricSecurityKey GetKey()
    //    {
    //        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    //    }
    //}
    //#endregion

    //#region JwtToken
    //public class JwtToken
    //{
    //    private static JwtKey _key;
    //    private static string _issuer; //Izdavač tokena, tj. entitet koji je generisao token
    //    private static string _audience; //Auditorijum tokena, tj. entitet koji je namenjen da ga koristi

    //    public JwtToken(string secretKey, string issuer, string audience)
    //    {
    //        _key = new JwtKey(secretKey);
    //        _issuer = issuer;
    //        _audience = audience;
    //    }

    //    public string GenerateToken(string email, string userType, int expiryMinutes = 60)
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = _key.GetKey();
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(new[]
    //            {
    //                new Claim(ClaimTypes.Email, email),
    //                new Claim("userType", userType),
    //            }),
    //            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
    //            Issuer = _issuer,
    //            Audience = _audience,
    //            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
    //        };
    //        var token = tokenHandler.CreateToken(tokenDescriptor);
    //        return tokenHandler.WriteToken(token);
    //    }
    //}
    //#endregion JwtToken

    //#region TokenHelper
    //public class JwtTokenReader
    //{
    //    public string ExtractTokenFromAuthorizationHeader(System.Net.Http.Headers.AuthenticationHeaderValue authorizationHeader)
    //    {
    //        if (authorizationHeader == null)
    //        {
    //            return null;
    //        }

    //        string authHeaderString = authorizationHeader.ToString();

    //        if (string.IsNullOrWhiteSpace(authHeaderString))
    //            return null;

    //        string[] parts = authHeaderString.Split(' ');
    //        if (parts.Length != 2 || parts[0] != "Bearer")
    //            return null;

    //        return parts[1];
    //    }

    //    public IEnumerable<Claim> GetClaimsFromToken(string token)
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var jwtToken = tokenHandler.ReadJwtToken(token);
    //        return jwtToken.Claims;
    //    }

    //    public string GetClaimValue(IEnumerable<Claim> claims, string claimType)
    //    {
    //        var claim = claims.FirstOrDefault(c => c.Type == claimType);
    //        return claim?.Value;
    //    }
    //}
    //#endregion
}
