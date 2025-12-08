var builder = WebApplication.CreateBuilder(args);
//my new change
//second change
// Add services to the container.
//test 2
//test 3
//test 2 PR
//squash test PR 1
//squash test PR 4
//rebase PR 1
//rebase 4
builder.Services.AddControllersWithViews();

var app = builder.Build();
//test 4
//test 2 PR
//test 6 PR
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
