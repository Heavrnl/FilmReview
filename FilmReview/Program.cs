using FilmReview;
using FilmReview.Models;
using FilmReview.Models.FilmReview;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//连接数据库配置
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>() // 使用 User Secrets
    .Build();

string? connStr = configuration.GetConnectionString("ConnStr");

builder.Services.AddDbContext<DataContext>(opt =>
{

    //opt.UseSqlServer(connStr);
    opt.UseSqlServer("Data Source=.; Initial Catalog = FilmReview ;Integrated Security=SSPI;TrustServerCertificate=true;");
});


//identity配置
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 6;
    opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
});


var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);

idBuilder.AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>();

builder.Services.AddTransient<Seed>();


//builder.WebHost.UseKestrel(options =>
//{
//    options.ListenLocalhost(1433);
//});

var app = builder.Build();

//初始化数据
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

async void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        await service.SeedDataAsync();
    }
}

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
