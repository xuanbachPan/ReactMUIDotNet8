using Microsoft.AspNetCore.Authentication.JwtBearer;
using ReactMUIWebAPIApplication.DBConnection;
using ReactMUIWebAPIApplication.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:19008", "http://localhost:3000", "https://localhost:7237/",
//                                              "http://www.contoso.com")
//                                           .AllowAnyHeader()
//                                           .AllowAnyMethod(); // add the allowed origins  
//                      });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DBContext>();
//builder.Services.AddScoped<IDataRepository, IDataRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => {
    o.AllowAnyOrigin();
    o.AllowAnyMethod();
    o.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
