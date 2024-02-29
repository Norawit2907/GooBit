using GooBitAPI.Models;
using GooBitAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
