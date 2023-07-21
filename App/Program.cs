using App.Interfaces;
using App.Services;

var builder = WebApplication.CreateBuilder ( args );
// Add services to the container.
builder.Services.AddControllersWithViews ( );
builder.Services.AddScoped<IDapperService, DapperService> ( );
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ( );
builder.Services.AddScoped<ILogin, LoginService> ( );
builder.Services.AddScoped<IRegister, RegisterService> ( );
builder.Services.AddScoped<IDropdownService, DropdownService> ( );
builder.Services.AddScoped<IStockManagement, StockManagementService> ( );
builder.Services.AddDistributedMemoryCache ( );
builder.Services.AddSession ( options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes ( 30 );
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
} );

var app = builder.Build ( );

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment ( ) )
{
    app.UseExceptionHandler ( "/Home/Error" );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ( );
}

app.UseHttpsRedirection ( );
app.UseStaticFiles ( );

app.UseRouting ( );

app.UseAuthorization ( );
app.UseSession ( );
app.MapControllerRoute (
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}" );
app.MapControllerRoute (
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}" );

app.Run ( );
