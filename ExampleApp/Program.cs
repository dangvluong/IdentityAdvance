using ExampleApp;
using ExampleApp.Custom;
using ExampleApp.Identity;
using ExampleApp.Identity.Store;
using ExampleApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<IAuthorizationHandler, CustomRequirementHandler>();
builder.Services.AddSingleton<ILookupNormalizer, Normalizer>();
builder.Services.AddSingleton<IUserStore<AppUser>, UserStore>();
builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();
builder.Services.AddSingleton<ISMSSender, ConsoleSMSSender>();
builder.Services.AddIdentityCore<AppUser>(opts =>
{
    opts.Tokens.EmailConfirmationTokenProvider = "SimpleEmail";
    opts.Tokens.ChangeEmailTokenProvider = "SimpleEmail";
})
.AddTokenProvider<EmailConfirmationTokenGenerator>("SimpleEmail")
.AddTokenProvider<PhoneConfirmationTokenGenerator>(TokenOptions.DefaultPhoneProvider);
builder.Services.AddSingleton<IUserValidator<AppUser>, EmailValidator>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme
  = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts =>
{
    opts.LoginPath = "/signin";
    opts.AccessDeniedPath = "/signin/403";
});
builder.Services.AddAuthorization(opts =>
{
    AuthorizationPolicies.AddPolicies(opts);
});
//builder.Services.AddAuthorization();
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
//app.UseMiddleware<CustomAuthentication>();
app.UseAuthentication();
//app.UseMiddleware<RoleMemberships>();
app.UseRouting();
//app.UseMiddleware<ClaimsReporter>();
//app.UseMiddleware<CustomAuthorization>();
//app.UseAuthorization();
//app.UseMiddleware<AuthorizationReporter>();
app.UseMiddleware<RoleMemberships>();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    //endpoints.MapGet("/", async context => await context.Response.WriteAsync("Hello world!"));
    //endpoints.MapGet("/secret", SecretEndpoint.Endpoint).WithDisplayName("secret");
    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapFallbackToPage("/Secret");
    //endpoints.Map("/signin", CustomSignInAndSignOut.SignIn);
    //endpoints.Map("/signout", CustomSignInAndSignOut.SignOut);
});


app.Run();
