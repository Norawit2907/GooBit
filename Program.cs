using GooBitAPI.Models;
using GooBitAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.IOTimeout = Timeout.InfiniteTimeSpan;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("GooBitDatabase"));

builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton<ParticipantService>();
builder.Services.AddSingleton<ReplyService>();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
