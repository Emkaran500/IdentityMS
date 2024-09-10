using IdentityMS.Data;
using IdentityMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IdentityMsDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("PostgreSqlIdentity");
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityMsDbContext>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();