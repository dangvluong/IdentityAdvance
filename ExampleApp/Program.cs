using ExampleApp;
using ExampleApp.Custom;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opts => {
    opts.DefaultScheme
  = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts => {
    opts.LoginPath = "/signin";
    opts.AccessDeniedPath = "/signin/403";
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
//app.UseMiddleware<CustomAuthentication>();
app.UseAuthentication();
app.UseMiddleware<RoleMemberships>();
app.UseRouting();
app.UseMiddleware<ClaimsReporter>();
//app.UseMiddleware<CustomAuthorization>();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => await context.Response.WriteAsync("Hello world!"));
    endpoints.MapGet("/secret", SecretEndpoint.Endpoint).WithDisplayName("secret");
    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
    //endpoints.Map("/signin", CustomSignInAndSignOut.SignIn);
    //endpoints.Map("/signout", CustomSignInAndSignOut.SignOut);
});


app.Run();
