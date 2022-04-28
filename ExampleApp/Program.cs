using ExampleApp;
using ExampleApp.Custom;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IAuthorizationHandler, CustomRequirementHandler>();
builder.Services.AddRazorPages(opt =>
{
    opt.Conventions.AuthorizePage("/Secret", "NotAdmins");
});
builder.Services.AddControllersWithViews(opts => {
    opts.Conventions.Add(new AuthorizationPolicyConvention("Home",
    policy: "NotAdmins"));
    opts.Conventions.Add(new AuthorizationPolicyConvention("Home",
    action: "Protected", policy: "UsersExceptBob"));
});
builder.Services.AddAuthentication(opts => {
    opts.DefaultScheme
  = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts => {
    opts.LoginPath = "/signin";
    opts.AccessDeniedPath = "/signin/403";
});
builder.Services.AddAuthorization(opts => {
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
app.UseMiddleware<AuthorizationReporter>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => await context.Response.WriteAsync("Hello world!"));
    //endpoints.MapGet("/secret", SecretEndpoint.Endpoint).WithDisplayName("secret");
    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
    //endpoints.Map("/signin", CustomSignInAndSignOut.SignIn);
    //endpoints.Map("/signout", CustomSignInAndSignOut.SignOut);
});


app.Run();
