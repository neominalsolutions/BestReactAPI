using ReactAPISample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
