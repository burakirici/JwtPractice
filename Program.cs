using IdentityPractice.Context;
using IdentityPractice.Managers;
using IdentityPractice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer= builder.Configuration["Jwt:Issuer"], // Appsettings içindeki deðeri çektik.
                        ValidateAudience = true, // Audience validationu.
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true, // Geçerlilik zamaný validationý yap.
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)) // Appsettingsteki key.

                    };
                });

var cn = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<CustomIdentityDbContext>(options => options.UseSqlServer(cn));

builder.Services.AddScoped<IUserService, UserManager>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();
