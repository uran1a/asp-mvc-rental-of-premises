using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RentalOfPremises.Models;
using RentalOfPremises.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
//����������� � ��
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(optionsBuilder => optionsBuilder.UseNpgsql(connection));
//�������
builder.Services.AddScoped<PlacementService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DealService>();
//��������� ������������ ����������� cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Configuration.Bind("Project", new Config());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //��������������
app.UseAuthorization();  //�����������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "/Admin",
    pattern: "{controller=Admin}/{action=Users}/{id?}");

app.Run();
