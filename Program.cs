var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Use(async (context, next) =>
        {
            var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            string _contentSecurityPolicy = $"default-src 'self'; script-src 'self' 'unsafe-inline' 'nonce-{nonce}'; style-src 'self'; img-src 'self';";

            context.Items["CSPNonce"] = nonce;
            context.Response.Headers.Add("Content-Security-Policy", _contentSecurityPolicy);
            await next();
        });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
