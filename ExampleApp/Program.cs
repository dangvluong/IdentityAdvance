var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => await context.Response.WriteAsync("Hello world!"));
    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
});


app.Run();
