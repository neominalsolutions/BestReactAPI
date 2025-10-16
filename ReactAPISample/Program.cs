using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactAPISample.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();

// swagger da bearer token test etmek swagger ui eklentisini ayarladýk.
// JWT haberlþemesine Bearer Scheme ile haberleþme diyoruz.
builder.Services.AddSwaggerGen(opt =>
{

  var securityScheme = new OpenApiSecurityScheme()
  {
    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT" // Optional
  };

  var securityRequirement = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearerAuth"
            }
        },
        new string[] {}
    }
};

  opt.AddSecurityDefinition("bearerAuth", securityScheme);
  opt.AddSecurityRequirement(securityRequirement);
});


// web servisler scoped olarak IoC tanýmlandý.
// project service sýnýfý instance üret. controlerda çaðýrýcaz.
builder.Services.AddScoped<ProjectService>();

// api client haberleþmesi için CORS policy tanýmlandý.
builder.Services.AddCors(builder =>
{
  builder.AddDefaultPolicy(policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});


// 2. adým ise jwt doðrulama adýmý

var key = Encoding.ASCII.GetBytes("6686060c32afbfe6b4d22a38d5ee4a4cceb32e2e361ed20b61eed839a67688509396598f98eb186c51f5f4f913d2afb6628ec8b32e3236ab5a3c9e5aba9abd0d");

// Validator Service
// Jwt Authentication tanýmlandý.
builder.Services.AddAuthentication(x =>
{
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
  opt.RequireHttpsMetadata = true;
  opt.SaveToken = true;
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    LifetimeValidator = (notbefore, expires, securityToken, validationParamaters) =>
    {
      Console.Out.WriteLineAsync("LifetimeValidator Event");
      return expires != null && expires.Value > DateTime.UtcNow;
    }
  };

  opt.Events = new JwtBearerEvents()
  {
    OnAuthenticationFailed = c =>
    {
      // token problemi hiç kimlik doðrulayamadýk.
      Console.Out.WriteLineAsync("Authentication Failed" + c.Exception.Message);
      return Task.CompletedTask;
    },
    OnTokenValidated = c =>
    {
      // token validate oldu
      Console.Out.WriteLineAsync("Authentication Valiated" + c.Result);
      return Task.CompletedTask;
    },
    OnForbidden = c =>
    {
      // Token doðru ama yetkisi yok
      Console.Out.WriteAsync("Yetki Yok" + c.Principal?.Identity?.Name);
      return Task.CompletedTask;
    }
  };
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
