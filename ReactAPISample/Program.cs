using ReactAPISample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// web servisler scoped olarak IoC tan�mland�.
// project service s�n�f� instance �ret. controlerda �a��r�caz.
builder.Services.AddScoped<ProjectService>();

// api client haberle�mesi i�in CORS policy tan�mland�.
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
