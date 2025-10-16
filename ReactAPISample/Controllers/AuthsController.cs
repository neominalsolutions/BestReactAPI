using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReactAPISample.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactAPISample.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthsController : ControllerBase
  {

    // Not:1 aşama Token generate aşaması 
    // 2. aşama Token Validate ve Authentication aşaması
    // 3. aşamada Authorization aşaması (Role bazlı yetkilendirme)

    // api/auths/token
    [HttpPost("token")]
    public IActionResult token([FromBody] TokenRequestDto request)
    {
      //var record = new TokenRequestDto("Ali", "Tan");
      //var record2 = record with { username = "Mustafa" }; -> var olan record üzerinden yeni bir record tanımı

      // 1. fark sadece contructor üzerinden init setter
      // request.username = "Ali";
      // request.password = "3424324";

      // valid user -> sanki böyle bir kullanıcı varmış gibi bu kullanıcıya token üreteceğiz
      if (request.username == "test" && request.password == "test123")
      {
        // Not: -> User.Identity.isAuthenticated kodunun düzgün çalışması için Net tarafında ClaimTypes.Name,  ClaimTypes.Role bu şekilde olmalıdır.
        // claimler normalde veritabanından okunup set edilir. bir simüle ediyoruz.
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, "test"));
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
        // yani kullanıcıya yukarıdaki özelliklere sahip olan bir hesap tanımlaması yaptık. token claims kısmına bu değerleri göndereceğiz.
        var identity = new ClaimsIdentity(claims);

        // Generated SignInKey: 3+9k9J5z9X9Z5Y1z9k9J5z9X9Z5Y1z9k9J5z9X9Z5Y1z9k9J5z9X9Z5Y1z9k9==
        var key = Encoding.ASCII.GetBytes("6686060c32afbfe6b4d22a38d5ee4a4cceb32e2e361ed20b61eed839a67688509396598f98eb186c51f5f4f913d2afb6628ec8b32e3236ab5a3c9e5aba9abd0d");

        var tokenHandler = new JwtSecurityTokenHandler();
        // token oluşturma sınıfı
        // token değerlerini tanımlama, expiredate, claims, signInKey
        var descriptor = new SecurityTokenDescriptor
        {
          Subject = identity,
          Expires = DateTime.UtcNow.AddMinutes(5),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
        };
        var token = tokenHandler.CreateToken(descriptor);
        var accessToken = tokenHandler.WriteToken(token);

        return Ok(new { accessToken});
      }

      return BadRequest("Kullanıcı bigileri hatalı");
    }
  }
}
