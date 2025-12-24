using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.Services;
using projeto_orcamento_mvc.Services.ClienteService;
using projeto_orcamento_mvc.Services.ItemService;
using projeto_orcamento_mvc.Services.ReciboService;
using projeto_orcamento_mvc.Services.ReportService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IOrcamentoInterface, OrcamentoService>();
builder.Services.AddScoped<IReciboInterface, ReciboService>();
builder.Services.AddScoped<IClienteInterface, ClienteService>();
builder.Services.AddScoped<ItemInterface, ItemService>();
builder.Services.AddScoped<IReportInterface, ReportService>();

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
