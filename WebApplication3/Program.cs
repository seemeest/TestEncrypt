using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using WebApplication3;
using WebApplication3.DataBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDataProtection()
          .PersistKeysToDbContext<ApplicationContext>()
          .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Register the EncryptionService
builder.Services.AddScoped<ChatEncryptionService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
