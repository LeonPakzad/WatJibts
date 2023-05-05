using src.Data;
using Microsoft.EntityFrameworkCore;
using ExampleApplication;

var builder = WebApplication.CreateBuilder(args);

// add database
builder.Services.AddDbContext<WatDbContext>(options =>{
        options.UseMySql(
            builder.Configuration.GetConnectionString("WatJibtsDb"),
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.34-mysql")
        );
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

#if DEBUG
{
// watch for changes in css
builder.Services.AddHostedService(sp => new NpmWatchHostedService(
                enabled: sp.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
                logger: sp.GetRequiredService<ILogger<NpmWatchHostedService>>()));
}
#endif

var app = builder.Build();


// seed db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
